using TienLenAi2.Core.Tests.Helpers;
using TienLenAi2.Core.Cards;
using TienLenAi2.Core.Engine;
using TienLenAi2.Core.States;
using TienLenAi2.Core.States.Players;
using TienLenAi2.Core.Hands;
using TienLenAi2.Core.States.Game;

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
        foreach (var (_, player) in playersState.Players)
        {
            Assert.IsNotNull(player);
            Assert.AreEqual($"Player {player.Id}", player.Name);
        }
    }

    [TestMethod]
    public void DealCards_FourPlayers_DistributesFullDeck()
    {
        // Arrange - Create store with 4 players already added and in Dealing phase
        var initialState = GameEngineTestHelpers.CreateStateForDealCards();
        var store = new Store(initialState);
        var engine = new GameEngine(store);
        var testDeck = GameEngineTestHelpers.CreateTestDeck();

        // Act
        engine.DealCards(13, testDeck);

        var playersState = engine.CurrentState.Players;

        // Assert
        // Verify each player has exactly 13 cards
        foreach (var (_, player) in playersState.Players)
        {
            Assert.IsNotNull(player);
            Assert.AreEqual(13, player.Cards.Count, $"Player {player.Id} should have 13 cards");
        }

        // Verify all 52 cards are distributed (no duplicates)
        var allPlayerCards = playersState.Players.Values
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
        var initialState = GameEngineTestHelpers.CreateStateForStartGame();
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

    [TestMethod]
    public void PlayHand_FourPlayers_ValidHand_FirstPlay()
    {
        // Arrange - Create store with 4 players and a valid game state
        var initialState = GameEngineTestHelpers.CreateStateForPlayHand();
        var store = new Store(initialState);
        var engine = new GameEngine(store);

        // Act - Player 1 plays a valid hand
        var playerId = 1;
        var hand = new Hand(HandType.Single, [Card.ThreeOfSpades]);

        engine.PlayHand(playerId, hand);

        // Assert - Check that the hand was played successfully
        var gameState = engine.CurrentState.Game;
        Assert.AreEqual(GamePhase.Playing, gameState.Phase, "Game should still be in Playing phase");
        Assert.AreEqual(playerId, gameState.CurrentPlayerId, "Last player should be Player 1");
        Assert.IsTrue(!gameState.PlayedHands.IsEmpty, "There should be at least one played hand");
        Assert.AreEqual(1, gameState.TrickNumber, "Trick number should be 1 for first play");
        Assert.AreEqual(1, gameState.GameNumber, "Game number should be 1 for the first game");
        Assert.AreEqual(playerId, gameState.StartingTrickPlayerId, $"Starting trick player should be player {playerId}");

        // Verify that the player's cards were updated correctly
        var player1 = PlayerSelectors.FindPlayerById(engine.CurrentState, playerId);
        Assert.IsNotNull(player1);
        CollectionAssert.DoesNotContain(player1!.Cards, Card.ThreeOfSpades, "Player 1 should no longer have 3♠");
    }

    [TestMethod]
    public void Pass_FourPlayers_MidTrick()
    {
        // Arrange - Create store with 4 players and a valid game state
        var initialState = GameEngineTestHelpers.CreateStateForPassTurn();
        var store = new Store(initialState);
        var engine = new GameEngine(store);

        // Act - Player 1 passes their turn
        var playerId = 1;
        engine.Pass(playerId);

        // Assert - Check that the turn was passed successfully
        var gameState = engine.CurrentState.Game;
        Assert.AreEqual(GamePhase.Playing, gameState.Phase, "Game should still be in Playing phase");
        Assert.IsTrue(gameState.PlayersPassed.Contains(playerId), "Player 1 should be marked as passed");
        Assert.IsFalse(GameSelectors.IsTrickOver(engine.CurrentState));
    }

    [TestMethod]
    public void UpdateGamePhase_ValidPhases()
    {
        var store = new Store();
        var engine = new GameEngine(store);

        Assert.AreEqual(GamePhase.NotStarted, engine.CurrentState.Game.Phase, "Initial phase should be NotStarted");

        engine.UpdateGamePhase(GamePhase.AddPlayer);
        Assert.AreEqual(GamePhase.AddPlayer, engine.CurrentState.Game.Phase, "Phase should be updated to AddPlayer");

        engine.UpdateGamePhase(GamePhase.Dealing);
        Assert.AreEqual(GamePhase.Dealing, engine.CurrentState.Game.Phase, "Phase should be updated to Dealing");

        engine.UpdateGamePhase(GamePhase.Starting);
        Assert.AreEqual(GamePhase.Starting, engine.CurrentState.Game.Phase, "Phase should be updated to Starting");

        engine.UpdateGamePhase(GamePhase.Playing);
        Assert.AreEqual(GamePhase.Playing, engine.CurrentState.Game.Phase, "Phase should be updated to Playing");

        engine.UpdateGamePhase(GamePhase.GameCompleted);
        Assert.AreEqual(GamePhase.GameCompleted, engine.CurrentState.Game.Phase, "Phase should be updated to GameCompleted");
    }
}
