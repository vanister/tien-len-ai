using System.Collections.Immutable;

namespace TienLenAi2.Core.States;

public record RootState(
    GameState Game,
    ImmutableDictionary<int, PlayerState> Players
)
{ }