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

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapHub<ChatHub>("/chat");
app.MapControllers();

app.Run();
