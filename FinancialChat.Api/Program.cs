using FinancialChat.Application.Services;
using FinancialChat.Application.Interfaces;
using FinancialChat.Infrastructure.Repositories;
using FinancialChat.Infrastructure.Persistence;
using FinancialChat.Api.Hubs;
using FinancialChat.Api.Notifiers;
using FinancialChat.Infrastructure.Messaging;
using FinancialChat.Infrastructure.Clients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using FinancialChat.Infrastructure.Messaging.Consumers;
using RabbitMQ.Client;
using FinancialChat.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<ChatDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options => {
        options.Password.RequireDigit = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 6;
    })
    .AddEntityFrameworkStores<ChatDbContext>()
    .AddDefaultTokenProviders();

// =======================
// Application
// =======================
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IStockBotService, StockBotService>();

// =======================
// Infrastructure
// =======================
builder.Services.AddScoped<IChatMessageRepository, ChatMessageRepository>();
builder.Services.AddScoped<IChatNotifier, SignalRChatNotifier>();

// HTTP client (Stooq)
builder.Services.AddHttpClient<IStockQuoteClient, StooqStockQuoteClient>();

// =======================
// RabbitMQ
// =======================
builder.Services.AddSingleton<IConnectionFactory>(_ =>
    new ConnectionFactory {
        HostName = "localhost",
        UserName = "guest",
        Password = "guest",
        DispatchConsumersAsync = true
    });

builder.Services.AddSingleton<IConnection>(sp =>
    sp.GetRequiredService<IConnectionFactory>().CreateConnection()
);

// Publisher (produce messages)
builder.Services.AddScoped<IStockCommandPublisher, RabbitMqStockCommandPublisher>();

// Consumer (background worker)
builder.Services.AddHostedService<StockBotConsumer>();

// =======================
builder.Services.AddSignalR();
builder.Services.AddSingleton<IUserIdProvider, NameUserIdProvider>();

builder.Services
    .AddAuthentication(options => {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };

        options.Events = new JwtBearerEvents {
            OnMessageReceived = context => {
                var accessToken = context.Request.Query["access_token"];

                if (!string.IsNullOrEmpty(accessToken) &&
                    context.HttpContext.Request.Path.StartsWithSegments("/chat")) {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", context => {
    context.Response.Redirect("/login.html");
    return Task.CompletedTask;
});

app.MapHub<ChatHub>("/chat");
app.MapControllers();

app.Run();

