using System.Collections.Immutable;

namespace TienLenAi2.Core.States.Players;

public record PlayerInfo(int Id, string Name);

public record AddPlayersAction(
    string Type,
    ImmutableList<PlayerInfo> Players
) : IAction
{
    public static AddPlayersAction Add(int playerCount)
    {
        if (playerCount < 2 || playerCount > 4)
        {
            throw new ArgumentException("Player count must be between 2 and 4", nameof(playerCount));
        }

        var players = Enumerable.Range(1, playerCount).Select(id => new PlayerInfo(id, $"Player {id}"));

        return new AddPlayersAction(PlayerActionTypes.AddPlayers, [.. players]);
    }
}
