using System.Collections.Immutable;

namespace TienLenAi2.Core.States.Players;

public record PlayersState(ImmutableDictionary<int, PlayerState> Players)
{
    public int NubmerOfPlayers => Players.Count;
}
