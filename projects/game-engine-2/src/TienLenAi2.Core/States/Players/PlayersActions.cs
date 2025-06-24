using System.Collections.Immutable;
using TienLenAi2.Core.Cards;

namespace TienLenAi2.Core.States.Players;

#region Action Types
public static class PlayerActionTypes
{
    public const string AddPlayers = "Players/AddPlayers";
    public const string UpdatePlayerCards = "Players/UpdatePlayerCards";
    public const string RemovePlayerCards = "Players/RemovePlayerCards";
}

#endregion

#region Actions

public record AddPlayersAction(string Type, ImmutableList<PlayerInfo> Players) : IAction
{
    public static AddPlayersAction Add(int playerCount)
    {
        var players = Enumerable.Range(1, playerCount).Select(id => new PlayerInfo(id, $"Player {id}"));

        return new AddPlayersAction(PlayerActionTypes.AddPlayers, [.. players]);
    }
}

public record UpdatePlayerCardsAction(string Type, int PlayerId, ImmutableList<Card> Cards) : IAction;
public record RemovePlayerCardsAction(string Type, int PlayerId, ImmutableList<Card> Cards) : IAction;  

#endregion

public record PlayerInfo(int Id, string Name);
