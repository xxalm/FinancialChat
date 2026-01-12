using System;
using FinancialChat.Domain.Entities;
using Xunit;

namespace FinancialChat.Domain.Tests.Entities;

public class ChatMessageTests {
    [Fact]
    public void Constructor_ShouldSetPropertiesCorrectly() {
        // Arrange
        var chatRoomId = Guid.NewGuid();
        var userId = "user123";
        var content = "Olá, mundo!";
        var isFromBot = true;

        // Act
        var message = new ChatMessage(chatRoomId, userId, content, isFromBot);

        // Assert
        Assert.Equal(chatRoomId, message.ChatRoomId);
        Assert.Equal(userId, message.UserId);
        Assert.Equal(content, message.Content);
        Assert.True(message.IsFromBot);
        Assert.True((DateTime.UtcNow - message.CreatedAt).TotalSeconds < 5);
        Assert.NotEqual(Guid.Empty, message.Id);
    }

    [Fact]
    public void Constructor_ShouldSetIsFromBotToFalseByDefault() {
        // Arrange
        var chatRoomId = Guid.NewGuid();
        var userId = "user123";
        var content = "Mensagem padrão";

        // Act
        var message = new ChatMessage(chatRoomId, userId, content);

        // Assert
        Assert.False(message.IsFromBot);
    }
}
