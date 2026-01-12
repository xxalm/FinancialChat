using System;
using FinancialChat.Domain.Entities;
using Xunit;

namespace FinancialChat.Domain.Tests.Entities;

public class ChatRoomTests {
    [Fact]
    public void Constructor_WithName_ShouldSetPropertiesCorrectly() {
        // Arrange
        var name = "Sala de Teste";

        // Act
        var room = new ChatRoom(name);

        // Assert
        Assert.Equal(name, room.Name);
        Assert.NotEqual(Guid.Empty, room.Id);
    }

    [Fact]
    public void Constructor_WithIdAndName_ShouldSetPropertiesCorrectly() {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Sala Avançada";

        // Act
        var room = new ChatRoom(id, name);

        // Assert
        Assert.Equal(id, room.Id);
        Assert.Equal(name, room.Name);
    }
}
