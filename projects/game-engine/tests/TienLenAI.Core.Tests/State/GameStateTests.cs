using TienLenAI.Core.Cards;
using TienLenAI.Core.Hands;
using TienLenAI.Core.State;

namespace TienLenAI.Core.Tests.State;

[TestClass]
public class GameStateTests
{
    private static List<Player> CreateTestPlayers()
    {
        return
        [
            new Player("Player1", CreateTestHand1()),
            new Player("Player2", CreateTestHand2()),
            new Player("Player3", CreateTestHand3()),
            new Player("Player4", CreateTestHand4())
        ];
    }

    private static List<Card> CreateTestHand1()
    {
        // Give Player1 the 3♠ to be the starting player
        return
        [
            new Card(CardRank.Three, CardSuit.Spades),
            new Card(CardRank.Four, CardSuit.Hearts),
            new Card(CardRank.Five, CardSuit.Diamonds),
            new Card(CardRank.Six, CardSuit.Clubs),
            new Card(CardRank.Seven, CardSuit.Spades)
        ];
    }

    private static List<Card> CreateTestHand2()
    {
        return
        [
            new Card(CardRank.Eight, CardSuit.Hearts),
            new Card(CardRank.Nine, CardSuit.Diamonds),
            new Card(CardRank.Ten, CardSuit.Clubs),
            new Card(CardRank.Jack, CardSuit.Spades),
            new Card(CardRank.Queen, CardSuit.Hearts)
        ];
    }

    private static List<Card> CreateTestHand3()
    {
        return
        [
            new Card(CardRank.King, CardSuit.Diamonds),
            new Card(CardRank.Ace, CardSuit.Clubs),
            new Card(CardRank.Two, CardSuit.Spades),
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Four, CardSuit.Diamonds)
        ];
    }

    private static List<Card> CreateTestHand4()
    {
        return
        [
            new Card(CardRank.Five, CardSuit.Clubs),
            new Card(CardRank.Six, CardSuit.Hearts),
            new Card(CardRank.Seven, CardSuit.Diamonds),
            new Card(CardRank.Eight, CardSuit.Clubs),
            new Card(CardRank.Nine, CardSuit.Spades)
        ];
    }

    [TestMethod]
    public void GameState_ShouldInitializeCorrectly()
    {
        // Arrange
        var players = CreateTestPlayers();

        // Act
        var gameState = new GameState(players, isFirstGame: true);

        // Assert
        Assert.AreEqual(4, gameState.Players.Count);
        Assert.AreEqual("Player1", gameState.GetCurrentPlayer().Name); // Player1 has 3♠
        Assert.IsFalse(gameState.IsGameComplete);
        Assert.IsTrue(gameState.IsFirstGame);
        Assert.IsFalse(gameState.HasFirstPlayBeenMade);
        Assert.AreEqual(0, gameState.FinishOrder.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void GameState_ShouldThrowWhenNotExactly4Players()
    {
        // Arrange
        var players = new List<Player>
        {
            new Player("Player1", []),
            new Player("Player2", [])
        };

        // Act & Assert
        new GameState(players);
    }

    [TestMethod]
    public void GameState_ShouldFindStartingPlayerWith3Spades()
    {
        // Arrange
        var players = new List<Player>
        {
            new Player("Player1", [new Card(CardRank.Four, CardSuit.Hearts)]),
            new Player("Player2", [new Card(CardRank.Three, CardSuit.Spades)]), // Has 3♠
            new Player("Player3", [new Card(CardRank.Five, CardSuit.Diamonds)]),
            new Player("Player4", [new Card(CardRank.Six, CardSuit.Clubs)])
        };

        // Act
        var gameState = new GameState(players, isFirstGame: true);

        // Assert
        Assert.AreEqual("Player2", gameState.GetCurrentPlayer().Name);
        Assert.AreEqual(1, gameState.CurrentPlayerIndex);
    }

    [TestMethod]
    public void GameState_ShouldValidateFirstPlayMustInclude3Spades()
    {
        // Arrange
        var players = CreateTestPlayers();
        var gameState = new GameState(players, isFirstGame: true);

        // Try to play a hand without 3♠
        var invalidHand = new SingleHand(new Card(CardRank.Four, CardSuit.Hearts));

        // Act & Assert
        Assert.IsFalse(gameState.IsValidPlay(invalidHand));
    }

    [TestMethod]
    public void GameState_ShouldAllowFirstPlayWith3Spades()
    {
        // Arrange
        var players = CreateTestPlayers();
        var gameState = new GameState(players, isFirstGame: true);

        // Play the 3♠
        var validHand = new SingleHand(new Card(CardRank.Three, CardSuit.Spades));

        // Act & Assert
        Assert.IsTrue(gameState.IsValidPlay(validHand));
    }

    [TestMethod]
    public void GameState_ShouldUpdateStateAfterPlay()
    {
        // Arrange
        var players = CreateTestPlayers();
        var gameState = new GameState(players, isFirstGame: true);
        var originalCurrentPlayerIndex = gameState.CurrentPlayerIndex;

        // Play the 3♠
        var hand = new SingleHand(new Card(CardRank.Three, CardSuit.Spades));

        // Act
        var newState = gameState.PlayHand(hand);

        // Assert
        Assert.IsTrue(newState.HasFirstPlayBeenMade);
        // Check the player who played (original current player) has one less card
        Assert.AreEqual(4, newState.Players[originalCurrentPlayerIndex].Hand.Count);
        Assert.IsNotNull(newState.CurrentTrick.CurrentHand);
        Assert.AreEqual(HandType.Single, newState.CurrentTrick.RequiredHandType);
    }

    [TestMethod]
    public void GameState_ShouldAdvanceToNextPlayerAfterPlay()
    {
        // Arrange
        var players = CreateTestPlayers();
        var gameState = new GameState(players, isFirstGame: true);

        // Play the 3♠
        var hand = new SingleHand(new Card(CardRank.Three, CardSuit.Spades));

        // Act
        var newState = gameState.PlayHand(hand);

        // Assert - Should advance to next active player
        var activePlayers = newState.GetActivePlayers().ToList();
        Assert.IsTrue(activePlayers.Contains(1)); // Player2 should be active
    }
}

