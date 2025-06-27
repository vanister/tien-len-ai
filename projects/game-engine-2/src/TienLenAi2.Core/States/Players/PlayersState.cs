using System.Collections.Immutable;

namespace TienLenAi2.Core.States.Players;

/// <summary>
/// Represents the state of all players in the game.
/// </summary>
public record PlayersState(ImmutableDictionary<int, PlayerState> ByIds)
{
    public int TotalPlayers => ByIds.Count;
    public IImmutableList<int> Ids => ByIds.Keys.ToImmutableList();

    public static PlayersState CreateDefault()
    {
        return new PlayersState(ImmutableDictionary<int, PlayerState>.Empty);
    }

}
