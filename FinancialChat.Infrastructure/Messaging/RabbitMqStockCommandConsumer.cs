using System.Text;
using System.Text.Json;
using FinancialChat.Application.Interfaces;
using FinancialChat.Application.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FinancialChat.Infrastructure.Messaging;

public class RabbitMqStockCommandConsumer : BackgroundService {
    private const string QueueName = "stock-commands";
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConnection _connection;

    public RabbitMqStockCommandConsumer(
        IServiceScopeFactory scopeFactory,
        IConnection connection) {
        _scopeFactory = scopeFactory;
        _connection = connection;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken) {
        var channel = _connection.CreateModel();

        channel.QueueDeclare(
            queue: QueueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += async (_, ea) => {
            var body = ea.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);

            var command = JsonSerializer.Deserialize<StockCommandMessage>(json);

            if (command is null) return;

            using var scope = _scopeFactory.CreateScope();
            var bot = scope.ServiceProvider.GetRequiredService<IStockCommandConsumer>();

            await bot.HandleAsync(
                command.ChatRoomId,
                command.StockCode,
                stoppingToken);
        };

        channel.BasicConsume(
            queue: QueueName,
            autoAck: true,
            consumer: consumer);

        return Task.CompletedTask;
    }
}
