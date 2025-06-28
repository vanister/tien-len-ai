using System.Collections.Immutable;

namespace TienLenAi2.Core.States.Players;

/// <summary>
/// Represents the state of all players in the game.
/// </summary>
public record PlayersState(ImmutableDictionary<int, PlayerState> Players)
{
    public int TotalPlayers => Players.Count;
    public ImmutableList<int> Ids => [.. Players.Keys];

    public static PlayersState CreateDefault()
    {
        return new PlayersState(ImmutableDictionary<int, PlayerState>.Empty);
    }

}
