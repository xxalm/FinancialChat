using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FinancialChat.Application.Interfaces;
using FinancialChat.Application.Services;
using FinancialChat.Domain.Entities;
using Moq;
using Xunit;

namespace FinancialChat.Application.Tests.Services;

public class ChatServiceTests {
    [Fact]
    public async Task SendMessageAsync_ShouldAddAndNotify_WhenMessageIsNormal() {
        // Arrange
        var chatRoomId = Guid.NewGuid();
        var userId = "user1";
        var content = "Olá!";
        var repositoryMock = new Mock<IChatMessageRepository>();
        var notifierMock = new Mock<IChatNotifier>();
        var stockPublisherMock = new Mock<IStockCommandPublisher>();

        ChatMessage? addedMessage = null;
        repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<ChatMessage>(), It.IsAny<CancellationToken>()))
            .Callback<ChatMessage, CancellationToken>((msg, _) => addedMessage = msg)
            .Returns(Task.CompletedTask);

        ChatMessage? notifiedMessage = null;
        notifierMock
            .Setup(n => n.NotifyMessageAsync(It.IsAny<ChatMessage>(), It.IsAny<CancellationToken>()))
            .Callback<ChatMessage, CancellationToken>((msg, _) => notifiedMessage = msg)
            .Returns(Task.CompletedTask);

        var service = new ChatService(repositoryMock.Object, notifierMock.Object, stockPublisherMock.Object);

        // Act
        await service.SendMessageAsync(chatRoomId, userId, content);

        // Assert
        Assert.NotNull(addedMessage);
        Assert.NotNull(notifiedMessage);
        Assert.Equal(addedMessage, notifiedMessage);
        Assert.Equal(chatRoomId, addedMessage!.ChatRoomId);
        Assert.Equal(userId, addedMessage.UserId);
        Assert.Equal(content, addedMessage.Content);
        Assert.False(addedMessage.IsFromBot);
        stockPublisherMock.Verify(sp => sp.PublishAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task SendMessageAsync_ShouldPublishStockCommand_WhenContentIsStockCommand() {
        // Arrange
        var chatRoomId = Guid.NewGuid();
        var userId = "user2";
        var stockCode = "PETR4";
        var content = $"/stock={stockCode}";
        var repositoryMock = new Mock<IChatMessageRepository>();
        var notifierMock = new Mock<IChatNotifier>();
        var stockPublisherMock = new Mock<IStockCommandPublisher>();

        var service = new ChatService(repositoryMock.Object, notifierMock.Object, stockPublisherMock.Object);

        // Act
        await service.SendMessageAsync(chatRoomId, userId, content);

        // Assert
        stockPublisherMock.Verify(sp => sp.PublishAsync(chatRoomId, stockCode, It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(r => r.AddAsync(It.IsAny<ChatMessage>(), It.IsAny<CancellationToken>()), Times.Never);
        notifierMock.Verify(n => n.NotifyMessageAsync(It.IsAny<ChatMessage>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetLastMessagesAsync_ShouldReturnMessagesFromRepository() {
        // Arrange
        var chatRoomId = Guid.NewGuid();
        var messages = new List<ChatMessage>
        {
            new ChatMessage(chatRoomId, "user1", "msg1"),
            new ChatMessage(chatRoomId, "user2", "msg2")
        };
        var repositoryMock = new Mock<IChatMessageRepository>();
        repositoryMock
            .Setup(r => r.GetLastMessagesAsync(chatRoomId, 50, It.IsAny<CancellationToken>()))
            .ReturnsAsync(messages);

        var notifierMock = new Mock<IChatNotifier>();
        var stockPublisherMock = new Mock<IStockCommandPublisher>();
        var service = new ChatService(repositoryMock.Object, notifierMock.Object, stockPublisherMock.Object);

        // Act
        var result = await service.GetLastMessagesAsync(chatRoomId);

        // Assert
        Assert.Equal(messages, result);
    }
}
