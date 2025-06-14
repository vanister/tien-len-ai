using TienLenAI.Core.Cards;
using TienLenAI.Core.State;

namespace TienLenAI.Core.Tests.State;

[TestClass]
public class GameStateTests
{
    [TestMethod]
    public void GameState_ShouldInitializeCorrectly()
    {
        // Arrange
        var players = new List<Player>
            {
                new Player("Player1", new List<Card>()),
                new Player("Player2", new List<Card>())
            };

        // Act
        var gameState = new GameState(players);

        // Assert
        Assert.AreEqual(2, gameState.Players.Count);
        Assert.AreEqual("Player1", gameState.GetCurrentPlayer().Name);
    }

    [TestMethod]
    public void GameState_ShouldAdvanceTurnCorrectly()
    {
        // Arrange
        var players = new List<Player>
            {
                new Player("Player1", new List<Card>()),
                new Player("Player2", new List<Card>())
            };
        var gameState = new GameState(players);

        // Act
        gameState.AdvanceTurn();

        // Assert
        Assert.AreEqual("Player2", gameState.GetCurrentPlayer().Name);

        // Act
        gameState.AdvanceTurn();

        // Assert
        Assert.AreEqual("Player1", gameState.GetCurrentPlayer().Name);
    }
}

