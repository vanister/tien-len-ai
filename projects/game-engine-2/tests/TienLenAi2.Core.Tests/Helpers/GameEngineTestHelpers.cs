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

    public static RootState CreateStateWithPlayers(int playerCount = 4)
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