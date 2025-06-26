using System.Collections.Immutable;
using TienLenAi2.Core.Cards;
using TienLenAi2.Core.Engine;
using TienLenAi2.Core.Hands;
using TienLenAi2.Core.States;
using TienLenAi2.Core.States.Players;
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
        var initialState = CreateStateWithPlayers();
        var store = new Store(initialState);
        var engine = new GameEngine(store);
        var testDeck = CreateTestDeck();

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

    private static ImmutableList<Card> CreateTestDeck()
    {
        // Create all cards in deterministic order (3♠ will be first naturally)
        var deck = new[] { Suit.Spades, Suit.Clubs, Suit.Diamonds, Suit.Hearts }
            .SelectMany(suit => Enum.GetValues<Rank>().Select(rank => new Card(rank, suit)));

        return [.. deck];
    }

    private static RootState CreateStateWithPlayers(int playerCount = 4)
    {
        // Create players state with 4 players
        var playersDict = Enumerable.Range(1, playerCount)
            .ToImmutableDictionary(
                i => i,
                i => new PlayerState(i, $"Player {i}", []));

        var playersState = new PlayersState(playersDict);

        // Create game state in Dealing phase
        var gameState = GameState.CreateDefault() with
        {
            Phase = GamePhase.Dealing
        };

        return new RootState(gameState, playersState, History: [], ActionSequenceNumber: 0);
    }
}
