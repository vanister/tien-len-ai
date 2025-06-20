using System.Collections.Immutable;

namespace TienLenAi2.Core.States.Players;

public static class PlayersReducer
{
    public static PlayersState Reduce(PlayersState state, IAction action)
    {
        return action.Type switch
        {
            PlayerActionTypes.AddPlayers => PlayerStateUpdater.AddPlayers(state, (AddPlayersAction)action),
            _ => state
        };
    }
}
