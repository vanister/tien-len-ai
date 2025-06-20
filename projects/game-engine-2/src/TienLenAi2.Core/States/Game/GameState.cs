using System.Collections.Immutable;
using TienLenAi2.Core.Hands;

namespace TienLenAi2.Core.States.Game;

public record GameState(
    int? CurrentPlayerId,
    int TrickNumber,
    GamePhase Phase,
    ImmutableList<Hand> PlayedHands,
    Hand? CurrentHand,
    ImmutableList<IAction> History
)
{
    public override string ToString()
    {
        return $"GameState(TrickNumber: {TrickNumber}, Phase: {Phase}, " +
               $"CurrentPlayerId: {CurrentPlayerId}, " +
               $"PlayedHandsCount: {PlayedHands.Count}, " +
               $"CurrentHand: {(CurrentHand != null ? CurrentHand.ToString() : "None")})";
    }
};