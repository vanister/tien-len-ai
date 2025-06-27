using TienLenAi2.Core.Tests.Helpers;
using TienLenAi2.Core.Cards;
using TienLenAi2.Core.Engine;
using TienLenAi2.Core.States;
using TienLenAi2.Core.States.Players;

namespace TienLenAi2.Core.Tests.Integrations;

[TestClass]
public class GameEngineSetupTests
{
    [TestMethod]
    public void AddPlayers_FourPlayers_AddsPlayersToState()
    {
        // Arrange
        var store = new Store();
        var engine = new GameEngine(store);

        // Act
        engine.AddPlayers(4);

        // Assert
        var playersState = engine.CurrentState.Players;
        var expectedPlayerIds = Enumerable.Range(1, 4).ToArray();

        Assert.AreEqual(4, playersState.TotalPlayers);
        CollectionAssert.AreEqual(expectedPlayerIds, playersState.Ids.OrderBy(id => id).ToArray());

        // Verify player names
        foreach (var (_, player) in playersState.ByIds)
        {
            Assert.IsNotNull(player);
            Assert.AreEqual($"Player {player.Id}", player.Name);
        }
    }

    [TestMethod]
    public void DealCards_FourPlayers_DistributesFullDeck()
    {
        // Arrange - Create store with 4 players already added and in Dealing phase
        var initialState = GameEngineTestHelpers.CreateStateReadyForDealing();
        var store = new Store(initialState);
        var engine = new GameEngine(store);
        var testDeck = GameEngineTestHelpers.CreateTestDeck();

        // Act
        engine.DealCards(13, testDeck);

        var playersState = engine.CurrentState.Players;

        // Assert
        // Verify each player has exactly 13 cards
        foreach (var (_, player) in playersState.ByIds)
        {
            Assert.IsNotNull(player);
            Assert.AreEqual(13, player.Cards.Count, $"Player {player.Id} should have 13 cards");
        }

        // Verify all 52 cards are distributed (no duplicates)
        var allPlayerCards = playersState.ByIds.Values
            .Where(p => p != null)
            .SelectMany(p => p!.Cards)
            .ToList();

        Assert.AreEqual(52, allPlayerCards.Count, "Total cards distributed should be 52");
        Assert.AreEqual(52, allPlayerCards.Distinct().Count(), "All cards should be unique (no duplicates)");

        // Verify round-robin distribution: Player 1 gets 3♠ (first card)
        var player1 = PlayerSelectors.FindPlayerById(engine.CurrentState, 1);
        Assert.IsTrue(player1!.Cards.Contains(Card.ThreeOfSpades), "Player 1 should have 3♠ (first card dealt)");

        // Verify Player 1 gets cards at positions 0, 4, 8, 12, ... (round-robin pattern)
        var player1ExpectedCards = testDeck.Where((_, index) => index % 4 == 0).ToList();
        CollectionAssert.AreEquivalent(player1ExpectedCards, player1.Cards.ToList());
    }

    [TestMethod]
    public void StartGame_FourPlayers_SetStartingPlayer()
    {
        // Arrange - Create store with 4 players already added and in Dealing phase
        var initialState = GameEngineTestHelpers.CreateStateReadyForStarting();
        var store = new Store(initialState);
        var engine = new GameEngine(store);

        // Act
        engine.StartGame();

        // Assert
        var gameState = engine.CurrentState.Game;
        Assert.IsNotNull(gameState.CurrentPlayerId, "CurrentPlayerId should not be null");
        Assert.AreEqual(1, gameState.CurrentPlayerId, "Starting player should be Player 1");

        var startingPlayer = PlayerSelectors.FindPlayerById(engine.CurrentState, gameState.CurrentPlayerId.Value);
        Assert.IsNotNull(startingPlayer);
        Assert.IsTrue(startingPlayer!.Cards.Contains(Card.ThreeOfSpades), "Starting player should have 3 of Spades");
    }
}
