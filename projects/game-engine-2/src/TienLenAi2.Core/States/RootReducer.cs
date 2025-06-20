using TienLenAi2.Core.States.Game;
using TienLenAi2.Core.States.Players;

namespace TienLenAi2.Core.States;

public static class RootReducer
{
    public static RootState Reduce(RootState state, IAction action)
    {
        return state with
        {
            Game = GameReducer.Reduce(state.Game, action),
            Players = PlayersReducer.Reduce(state.Players, action)
        };
    }
}