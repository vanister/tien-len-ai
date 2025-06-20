using System.Collections.Immutable;

namespace TienLenAi2.Core.States.Reducers;

public static class PlayersReducer
{
    public static PlayersState Reduce(PlayersState state, IAction action)
    {
        return action.Type switch
        {
            _ => state
        };
    }
}
