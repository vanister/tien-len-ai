using System.Collections.Immutable;
using TienLenAi2.Core.Hands;

namespace TienLenAi2.Core.States.Game;

public record GameState(
    int GameNumber,
    int TrickNumber,
    GamePhase Phase,
    ImmutableList<Hand> PlayedHands,
    int? CurrentPlayerId,
    HandType? TrickType,
    Hand? CurrentHand,
    int? WinningPlayerId
)
{
    public static GameState CreateDefault()
    {
        return new GameState(
            CurrentPlayerId: null,
            TrickNumber: 0,
            TrickType: null,
            Phase: GamePhase.NotStarted,
            PlayedHands: [],
            CurrentHand: null,
            WinningPlayerId: null,
            GameNumber: 0
        );
    }
};