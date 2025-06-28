using System.Collections.Immutable;
using TienLenAi2.Core.Cards;

namespace TienLenAi2.Core.States.Players;

public static class PlayerStateUpdater
{
    public static PlayersState AddPlayers(PlayersState state, AddPlayersAction action)
    {
        var playersDict = state.Players;

        foreach (var playerInfo in action.Players)
        {
            var player = PlayerState.WithIdAndName(playerInfo.Id, playerInfo.Name);

            playersDict = playersDict.Add(playerInfo.Id, player);
        }

        return state with { Players = playersDict };
    }

    public static PlayersState UpdatePlayerCards(PlayersState state, UpdatePlayerCardsAction action)
    {
        var currentPlayer = state.Players.GetValueOrDefault(action.PlayerId)
            ?? throw new InvalidOperationException($"Player with ID {action.PlayerId} not found.");

        var updatedPlayer = currentPlayer with { Cards = action.Cards };
        var updatedPlayers = state.Players.SetItem(action.PlayerId, updatedPlayer);

        return state with { Players = updatedPlayers };
    }

    public static PlayersState RemovePlayerCards(PlayersState state, RemovePlayerCardsAction action)
    {
        var currentPlayer = state.Players.GetValueOrDefault(action.PlayerId)
            ?? throw new InvalidOperationException($"Player with ID {action.PlayerId} not found.");

        var updatedPlayer = currentPlayer with { Cards = currentPlayer.Cards.RemoveRange(action.Cards) };
        var updatedPlayers = state.Players.SetItem(action.PlayerId, updatedPlayer);

        return state with { Players = updatedPlayers };
    }
}
