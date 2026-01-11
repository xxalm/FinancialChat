using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using FinancialChat.Application.Interfaces;
using FinancialChat.Infrastructure.Messaging.Contracts;

namespace FinancialChat.Infrastructure.Messaging.Consumers;

public sealed class StockBotConsumer : BackgroundService 
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConnectionFactory _connectionFactory;

    private IConnection? _connection;
    private IModel? _channel;

    private const string QueueName = "stock-commands";

    public StockBotConsumer(IServiceScopeFactory scopeFactory, IConnectionFactory connectionFactory) 
    {
        _scopeFactory = scopeFactory;
        _connectionFactory = connectionFactory;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken) 
    {
        _connection = _connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(queue: QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.Received += async (_, eventArgs) => {
            using var scope = _scopeFactory.CreateScope();
            var stockBotService = scope.ServiceProvider
                .GetRequiredService<IStockBotService>();

            var body = eventArgs.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);

            try 
            {
                var message = JsonSerializer.Deserialize<StockCommandMessage>(json);

                if (message is null) {
                    _channel.BasicNack(eventArgs.DeliveryTag, false, false);
                    return;
                }

                await stockBotService.HandleAsync(
                    message.ChatRoomId,
                    message.StockCode,
                    stoppingToken
                );

                _channel.BasicAck(eventArgs.DeliveryTag, false);
            } 
            catch 
            {
                _channel.BasicNack(eventArgs.DeliveryTag, false, false);
            }
        };

        _channel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);

        return Task.CompletedTask;
    }

    public override void Dispose() {
        _channel?.Close();
        _connection?.Close();
        base.Dispose();
    }
}