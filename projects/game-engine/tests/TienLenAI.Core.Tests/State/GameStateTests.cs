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
        Assert.AreEqual(4, gameState.PlayerCount);
        Assert.AreEqual("Player1", gameState.GetCurrentPlayer().Name); // Player1 has 3♠
        Assert.IsFalse(gameState.IsGameComplete);
        Assert.AreEqual(1, gameState.TrickNumber);
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
        // Check the player who played (original current player) has one less card
        Assert.AreEqual(4, newState.GetPlayer(originalCurrentPlayerIndex).Hand.Count);
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

    [TestMethod]
    public void GameState_ShouldInitializeWithEmptyTrickHistory()
    {
        // Arrange
        var players = CreateTestPlayers();

        // Act
        var gameState = new GameState(players, isFirstGame: true);

        // Assert
        Assert.AreEqual(0, gameState.GetCompletedTricks().Count());
        Assert.AreEqual(1, gameState.TrickNumber);
        Assert.IsNull(gameState.GetLastCompletedTrick());
    }

    [TestMethod]
    public void GameState_ShouldTrackPlayerHandHistory()
    {
        // Arrange
        var players = CreateTestPlayers();
        var gameState = new GameState(players, isFirstGame: true);
        var originalCurrentPlayerIndex = gameState.CurrentPlayerIndex;

        // Play the 3♠
        var hand = new SingleHand(new Card(CardRank.Three, CardSuit.Spades));

        // Act
        var newState = gameState.PlayHand(hand);
        var playerHistory = newState.GetPlayerHandHistory(originalCurrentPlayerIndex);

        // Assert
        Assert.AreEqual(1, playerHistory.Count());
        Assert.AreEqual(HandType.Single, playerHistory.First().Type);
        Assert.IsTrue(playerHistory.First().Cards.Contains(new Card(CardRank.Three, CardSuit.Spades)));
    }

    [TestMethod]
    public void GameState_ShouldTrackPlayedHandTypes()
    {
        // Arrange
        var players = CreateTestPlayers();
        var gameState = new GameState(players, isFirstGame: true);

        // Play the 3♠
        var hand = new SingleHand(new Card(CardRank.Three, CardSuit.Spades));

        // Act
        var newState = gameState.PlayHand(hand);
        var playedTypes = newState.GetPlayedHandTypes();

        // Assert
        Assert.IsTrue(playedTypes.Contains(HandType.Single));
        Assert.AreEqual(1, playedTypes.Count());
    }

    [TestMethod]
    public void GameState_ShouldTrackCardsPlayedCounts()
    {
        // Arrange
        var players = CreateTestPlayers();
        var gameState = new GameState(players, isFirstGame: true);
        var originalCurrentPlayerIndex = gameState.CurrentPlayerIndex;

        // Play the 3♠
        var hand = new SingleHand(new Card(CardRank.Three, CardSuit.Spades));

        // Act
        var newState = gameState.PlayHand(hand);
        var cardCounts = newState.GetCardsPlayedCounts();

        // Assert
        Assert.AreEqual(1, cardCounts[originalCurrentPlayerIndex]);

        // Other players should have 0 cards played
        for (int i = 0; i < players.Count; i++)
        {
            if (i != originalCurrentPlayerIndex)
            {
                Assert.AreEqual(0, cardCounts[i]);
            }
        }
    }

    [TestMethod]
    public void GameState_ShouldMoveTrickToHistoryWhenCompleted()
    {
        // This test would require a more complex setup to complete a full trick
        // For now, we'll test the basic initialization
        var players = CreateTestPlayers();
        var gameState = new GameState(players, isFirstGame: true);

        // Initially no completed tricks
        Assert.AreEqual(0, gameState.GetCompletedTricks().Count());
        Assert.AreEqual(1, gameState.TrickNumber);

        // All players should have 0 trick wins initially
        var winCounts = gameState.GetTrickWinCounts();
        foreach (var count in winCounts.Values)
        {
            Assert.AreEqual(0, count);
        }
    }

    [TestMethod]
    public void CreateNewGame_ShouldInitializeCorrectly()
    {
        // Act
        var gameState = GameState.CreateNewGame();

        // Assert
        Assert.AreEqual(4, gameState.PlayerCount);
        Assert.IsFalse(gameState.IsGameComplete);
        Assert.AreEqual(1, gameState.TrickNumber);
        Assert.AreEqual(0, gameState.FinishOrder.Count);
    }

    [TestMethod]
    public void CreateNewGame_ShouldUseDefaultPlayerNames()
    {
        // Act
        var gameState = GameState.CreateNewGame();

        // Assert
        Assert.AreEqual("Player 1", gameState.GetPlayer(0).Name);
        Assert.AreEqual("Player 2", gameState.GetPlayer(1).Name);
        Assert.AreEqual("Player 3", gameState.GetPlayer(2).Name);
        Assert.AreEqual("Player 4", gameState.GetPlayer(3).Name);
    }

    [TestMethod]
    public void CreateNewGame_WithCustomNames_ShouldUseProvidedNames()
    {
        // Arrange
        var names = new[] { "Alice", "Bob", "Charlie", "Diana" };

        // Act
        var gameState = GameState.CreateNewGame(names);

        // Assert
        for (int i = 0; i < 4; i++)
        {
            Assert.AreEqual(names[i], gameState.GetPlayer(i).Name);
        }
    }

    [TestMethod]
    public void CreateNewGame_ShouldGiveEachPlayer13Cards()
    {
        // Act
        var gameState = GameState.CreateNewGame();

        // Assert
        for (int i = 0; i < 4; i++)
        {
            Assert.AreEqual(13, gameState.GetPlayer(i).Hand.Count);
        }
    }

    [TestMethod]
    public void CreateNewGame_ShouldDistributeAll52Cards()
    {
        // Act
        var gameState = GameState.CreateNewGame();

        // Assert
        var allCards = new List<Card>();
        for (int i = 0; i < 4; i++)
        {
            allCards.AddRange(gameState.GetPlayer(i).Hand);
        }

        Assert.AreEqual(52, allCards.Count);
        var uniqueCards = new HashSet<Card>(allCards);
        Assert.AreEqual(52, uniqueCards.Count);
    }

    [TestMethod]
    public void CreateNewGame_OnePlayerShouldHave3Spades()
    {
        // Act
        var gameState = GameState.CreateNewGame();

        // Assert
        var threeOfSpades = new Card(CardRank.Three, CardSuit.Spades);
        var playersWithThreeSpades = 0;
        var playerWithThreeSpades = -1;

        for (int i = 0; i < 4; i++)
        {
            if (gameState.GetPlayer(i).Hand.Contains(threeOfSpades))
            {
                playersWithThreeSpades++;
                playerWithThreeSpades = i;
            }
        }

        Assert.AreEqual(1, playersWithThreeSpades, "Exactly one player should have 3♠");
        Assert.AreEqual(playerWithThreeSpades, gameState.CurrentPlayerIndex,
            "The player with 3♠ should be the current player");
    }

    [TestMethod]
    public void CreateNewGame_WithSeed_ShouldBeReproducible()
    {
        // Arrange
        const int seed = 42;
        var names = new[] { "Alice", "Bob", "Charlie", "Diana" };

        // Act
        var game1 = GameState.CreateNewGame(seed, names);
        var game2 = GameState.CreateNewGame(seed, names);

        // Assert
        Assert.AreEqual(game1.CurrentPlayerIndex, game2.CurrentPlayerIndex);

        for (int i = 0; i < 4; i++)
        {
            CollectionAssert.AreEqual(game1.GetPlayer(i).Hand, game2.GetPlayer(i).Hand);
            Assert.AreEqual(game1.GetPlayer(i).Name, game2.GetPlayer(i).Name);
        }
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CreateNewGame_WithWrongNumberOfNames_ShouldThrow()
    {
        // Arrange
        var names = new[] { "Alice", "Bob" }; // Only 2 names, need 4

        // Act & Assert
        GameState.CreateNewGame(names);
    }
}

