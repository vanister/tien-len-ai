using System.Collections.Immutable;
using TienLenAi2.Core.Hands;

namespace TienLenAi2.Core.States.Game;

public record GameState(
    GamePhase Phase = GamePhase.NotStarted,
    int GameNumber = 0,
    int TrickNumber = 0,
    HandType? CurrentHandType = null,
    Hand? CurrentHand = null,
    int? StartingTrickPlayerId = null,
    int? CurrentPlayerId = null,
    int? WinningPlayerId = null
)
{
    public ImmutableList<Hand> PlayedHands { get; init; } = [];

    public static GameState CreateDefault() => new();
};