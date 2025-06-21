using TienLenAi2.Core.States.Game;
using TienLenAi2.Core.States.Players;

namespace TienLenAi2.Core.States;

public static class RootReducer
{
    /// <summary>
    /// Reduces the current state based on the action provided and adds a history entry entry.
    /// </summary>
    public static RootState Reduce(RootState state, IAction action)
    {
        var newState = ReduceCore(state, action);

        // log the action in the history
        return newState with
        {
            History = state.History.Add(new HistoryState(state.ActionSequenceNumber, action)),
            ActionSequenceNumber = state.ActionSequenceNumber + 1
        };
    }

    /// <summary>
    /// Core reducer that updates the game and players state based on the action without adding history.
    /// </summary>
    public static RootState ReduceCore(RootState state, IAction action)
    {
        return state with
        {
            Game = GameReducer.Reduce(state.Game, action),
            Players = PlayersReducer.Reduce(state.Players, action)
        };
    }
}