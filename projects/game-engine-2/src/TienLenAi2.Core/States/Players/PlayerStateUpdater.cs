using System.Collections.Immutable;
using TienLenAi2.Core.Cards;

namespace TienLenAi2.Core.States.Players;

public static class PlayerStateUpdater
{
    public static PlayersState AddPlayers(PlayersState state, AddPlayersAction action)
    {
        ArgumentNullException.ThrowIfNull(action);

        var playersDict = state.ByIds;

        foreach (var playerInfo in action.Players)
        {
            var player = PlayerState.WithIdAndName(playerInfo.Id, playerInfo.Name);

            playersDict = playersDict.Add(playerInfo.Id, player);
        }

        return state with { ByIds = playersDict };
    }

    public static PlayersState UpdatePlayerCards(PlayersState state, UpdatePlayerCardsAction action)
    {
        ArgumentNullException.ThrowIfNull(action);

        if (!state.ByIds.ContainsKey(action.PlayerId))
        {
            throw new ArgumentException($"Player with ID {action.PlayerId} does not exist", nameof(action));
        }

        var currentPlayer = state.ByIds[action.PlayerId];
        var updatedPlayer = currentPlayer with { Cards = action.Cards };
        var updatedPlayers = state.ByIds.SetItem(action.PlayerId, updatedPlayer);

        return state with { ByIds = updatedPlayers };
    }
}
