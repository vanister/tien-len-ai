using System.Collections.Immutable;
using TienLenAi2.Core.Hands;

namespace TienLenAi2.Core.States.Game;

public record GameState(
    GamePhase Phase = GamePhase.NotStarted,
    int GameNumber = 0,
    bool IsTrickOver = false,
    int TotalPlayers = 0,
    CurrentTrick? CurrentTrick = null,
    int? CurrentPlayerId = null,
    int? WinningPlayerId = null
)
{
    public ImmutableList<Hand> PlayedHands { get; init; } = [];
    public ImmutableHashSet<int> PlayersPassed { get; init; } = [];

    public static GameState CreateDefault() => new();
};

/// <summary>
/// Represents the current trick in the game.
/// </summary>
/// <param name="PlayerId">The player with the hand to beat.</param>
/// <param name="Hand">The hand to beat.</param>
public record CurrentTrick(int PlayerId, Hand Hand) { }