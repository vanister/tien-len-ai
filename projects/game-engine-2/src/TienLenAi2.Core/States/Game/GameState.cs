using System.Collections.Immutable;
using TienLenAi2.Core.Hands;

namespace TienLenAi2.Core.States.Game;

public record GameState(
    GamePhase Phase = GamePhase.NotStarted,
    int GameNumber = 0,
    int TrickNumber = 0,
    bool IsTrickOver = false,
    // todo - add total palyers
    HandType? CurrentHandType = null,
    Hand? CurrentHand = null,
    int? StartingTrickPlayerId = null,
    int? CurrentPlayerId = null,
    int? WinningPlayerId = null
)
{
    public ImmutableList<Hand> PlayedHands { get; init; } = [];
    public ImmutableHashSet<int> PlayersPassed { get; init; } = [];

    public static GameState CreateDefault() => new();
};