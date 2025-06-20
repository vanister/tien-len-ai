using System.Collections.Immutable;

namespace TienLenAi2.Core.States.Reducers;

public static class PlayersReducer
{
    public static ImmutableDictionary<int, PlayerState> Reduce(ImmutableDictionary<int, PlayerState> state, IAction action)
    {
        return action.Type switch
        {
            _ => state
        };
    }
}
