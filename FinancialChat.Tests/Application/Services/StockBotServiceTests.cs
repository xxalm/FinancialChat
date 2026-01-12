using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using FinancialChat.Application.Interfaces;
using FinancialChat.Application.Services;
using FinancialChat.Domain.Entities;
using Moq;
using Xunit;

namespace FinancialChat.Application.Tests.Services;

public class StockBotServiceTests {
    [Fact]
    public async Task HandleAsync_ShouldSendQuoteMessage_WhenQuoteExists() {
        // Arrange
        var chatRoomId = Guid.NewGuid();
        var stockCode = "aapl";
        var quote = 123.45m;

        var notifierMock = new Mock<IChatNotifier>();
        var stockClientMock = new Mock<IStockQuoteClient>();
        stockClientMock
            .Setup(x => x.GetQuoteAsync(stockCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync(quote);

        ChatMessage? sentMessage = null;
        notifierMock
            .Setup(x => x.NotifyMessageAsync(It.IsAny<ChatMessage>(), It.IsAny<CancellationToken>()))
            .Callback<ChatMessage, CancellationToken>((msg, _) => sentMessage = msg)
            .Returns(Task.CompletedTask);

        var service = new StockBotService(notifierMock.Object, stockClientMock.Object);

        // Act
        await service.HandleAsync(chatRoomId, stockCode);

        // Assert
        Assert.NotNull(sentMessage);
        Assert.Equal(chatRoomId, sentMessage!.ChatRoomId);
        Assert.Equal("bot", sentMessage.UserId);
        Assert.True(sentMessage.IsFromBot);
        Assert.Contains(stockCode.ToUpper(), sentMessage.Content);
        Assert.Contains(quote.ToString(CultureInfo.InvariantCulture), sentMessage.Content);
    }

    [Fact]
    public async Task HandleAsync_ShouldSendErrorMessage_WhenQuoteDoesNotExist() {
        // Arrange
        var chatRoomId = Guid.NewGuid();
        var stockCode = "invalid";
        decimal? quote = null;

        var notifierMock = new Mock<IChatNotifier>();
        var stockClientMock = new Mock<IStockQuoteClient>();
        stockClientMock
            .Setup(x => x.GetQuoteAsync(stockCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync(quote);

        ChatMessage? sentMessage = null;
        notifierMock
            .Setup(x => x.NotifyMessageAsync(It.IsAny<ChatMessage>(), It.IsAny<CancellationToken>()))
            .Callback<ChatMessage, CancellationToken>((msg, _) => sentMessage = msg)
            .Returns(Task.CompletedTask);

        var service = new StockBotService(notifierMock.Object, stockClientMock.Object);

        // Act
        await service.HandleAsync(chatRoomId, stockCode);

        // Assert
        Assert.NotNull(sentMessage);
        Assert.Equal(chatRoomId, sentMessage!.ChatRoomId);
        Assert.Equal("bot", sentMessage.UserId);
        Assert.True(sentMessage.IsFromBot);
        Assert.Contains("Could not retrieve quote", sentMessage.Content);
        Assert.Contains(stockCode.ToUpper(), sentMessage.Content);
    }
}
