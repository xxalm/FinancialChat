using FinancialChat.Application.Services;
using FinancialChat.Application.Interfaces;
using FinancialChat.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using FinancialChat.Infrastructure.Persistence;
using FinancialChat.Api.Hubs;
using FinancialChat.Api.Notifiers;
using FinancialChat.Infrastructure.Messaging;


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

// Application
builder.Services.AddScoped<IChatService, ChatService>();

// Infrastructure
builder.Services.AddScoped<IChatMessageRepository, ChatMessageRepository>();
builder.Services.AddScoped<IChatNotifier, SignalRChatNotifier>();
builder.Services.AddScoped<IStockCommandPublisher, InMemoryStockCommandPublisher>();

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
