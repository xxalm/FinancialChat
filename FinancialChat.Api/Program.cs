using FinancialChat.Application.Services;
using FinancialChat.Application.Interfaces;
using FinancialChat.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using FinancialChat.Infrastructure.Persistence;

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

// Application services
builder.Services.AddScoped<ChatService>();

// Infrastructure
builder.Services.AddScoped<IChatMessageRepository, ChatMessageRepository>();

var app = builder.Build();

// HTTP pipeline
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
