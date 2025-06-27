using System.Collections.Immutable;
using TienLenAi2.Core.Cards;
using TienLenAi2.Core.States;
using TienLenAi2.Core.States.Game;
using TienLenAi2.Core.States.Players;

namespace TienLenAi2.Core.Tests.Helpers;

public static class GameEngineTestHelpers
{
    public static ImmutableList<Card> CreateTestDeck()
    {
        // Create all cards in deterministic order (3♠ will be first naturally)
        var deck = new[] { Suit.Spades, Suit.Clubs, Suit.Diamonds, Suit.Hearts }
            .SelectMany(suit => Enum.GetValues<Rank>().Select(rank => new Card(rank, suit)));

        return [.. deck];
    }

    public static RootState CreateStateReadyForDealing()
    {
        // Create players state with 4 players
        var players = CreatePlayers();
        var playersState = new PlayersState(players);

        // Create game state in Dealing phase
        var gameState = GameState.CreateDefault() with
        {
            Phase = GamePhase.Dealing
        };

        return new RootState(gameState, playersState, History: []);
    }

    public static RootState CreateStateReadyForStarting(int playerCount = 4, int cardsPerPlayer = 13)
    {
        var playerCards = CreateTestCardsForPlayer(playerCount, cardsPerPlayer);
        var players = CreatePlayers(playerCards);
        var playersState = new PlayersState(players);

        var gameState = GameState.CreateDefault() with
        {
            Phase = GamePhase.Starting
        };

        return new RootState(gameState, playersState, History: []);
    }

    public static ImmutableDictionary<int, ImmutableList<Card>> CreateTestCardsForPlayer(int playerCount = 4, int cardsPerPlayer = 13)
    {
        var testDeck = CreateTestDeck();
        var playerCards = Enumerable.Range(1, playerCount)
            .ToImmutableDictionary(
                i => i,
                i => testDeck.Skip((i - 1) * cardsPerPlayer).Take(cardsPerPlayer).ToImmutableList());

        return playerCards;
    }

    public static ImmutableDictionary<int, PlayerState> CreatePlayers(ImmutableDictionary<int, ImmutableList<Card>>? playerCards = null)
    {
        var playersDict = Enumerable.Range(1, 4).ToImmutableDictionary(
            i => i,
            i => new PlayerState(i, $"Player {i}", playerCards?.GetValueOrDefault(i) ?? []));

        return playersDict;
    }
}