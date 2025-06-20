using System.Collections.Immutable;
using TienLenAi2.Core.Cards;

namespace TienLenAi2.Core.States.Players;

public static class PlayersReducer
{
    public static PlayersState Reduce(PlayersState state, IAction action)
    {
        return action.Type switch
        {
            PlayerActionTypes.AddPlayers => AddPlayers(state, (AddPlayersAction)action),
            _ => state
        };
    }

    private static PlayersState AddPlayers(PlayersState state, AddPlayersAction action)
    {
        ArgumentNullException.ThrowIfNull(action);

        var playersDict = ImmutableDictionary<int, PlayerState>.Empty;

        foreach (var playerInfo in action.Players)
        {
            var playerState = new PlayerState
            {
                Id = playerInfo.Id,
                Name = playerInfo.Name,
                Cards = ImmutableList<Card>.Empty,
                HasPassed = false
            };

            playersDict = playersDict.Add(playerInfo.Id, playerState);
        }

        return new PlayersState(playersDict);
    }
}
