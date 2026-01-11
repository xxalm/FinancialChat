using System.Text;
using System.Text.Json;
using FinancialChat.Application.Interfaces;
using FinancialChat.Application.Messaging;
using RabbitMQ.Client;

namespace FinancialChat.Infrastructure.Messaging;

public class RabbitMqStockCommandPublisher : IStockCommandPublisher {
    private const string QueueName = "stock-commands";
    private readonly IConnection _connection;

    public RabbitMqStockCommandPublisher(IConnection connection) {
        _connection = connection;
    }

    public Task PublishAsync(
        Guid chatRoomId,
        string stockCode,
        CancellationToken cancellationToken = default) {
        using var channel = _connection.CreateModel();

        channel.QueueDeclare(
            queue: QueueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var message = new StockCommandMessage(chatRoomId, stockCode);
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        channel.BasicPublish(
            exchange: "",
            routingKey: QueueName,
            basicProperties: null,
            body: body);

        return Task.CompletedTask;
    }
}
