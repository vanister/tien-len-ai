using System.Collections.Immutable;
using TienLenAi2.Core.Cards;

namespace TienLenAi2.Core.States.Players;

public static class PlayerStateUpdater
{
    public static PlayersState AddPlayers(PlayersState state, AddPlayersAction action)
    {
        ArgumentNullException.ThrowIfNull(action);

        var playersDict = state.Players;

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

        return state with { Players = playersDict };
    }
}
