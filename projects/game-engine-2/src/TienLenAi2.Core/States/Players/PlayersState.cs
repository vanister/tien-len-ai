using System.Collections.Immutable;

namespace TienLenAi2.Core.States;

public record PlayersState(ImmutableDictionary<int, PlayerState> Players);
