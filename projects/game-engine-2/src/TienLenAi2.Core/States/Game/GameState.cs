using System.Collections.Immutable;
using TienLenAi2.Core.Hands;

namespace TienLenAi2.Core.States.Game;

public record GameState(
    int? CurrentPlayerId,
    int TrickNumber,
    GamePhase Phase,
    ImmutableList<Hand> PlayedHands,
    Hand? CurrentHand,
    int? WinnerId,
    int GameNumber
)
{
    // todo - add a static method to create a default game state
    public static GameState CreateDefault()
    {
        return new GameState(
            CurrentPlayerId: null,
            TrickNumber: 0,
            Phase: GamePhase.NotStarted,
            PlayedHands: [],
            CurrentHand: null,
            WinnerId: null,
            GameNumber: 0
        );
    }
};