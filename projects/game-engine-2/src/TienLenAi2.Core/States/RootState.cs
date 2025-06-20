using System.Collections.Immutable;

namespace TienLenAi2.Core.States;

public record RootState(
    GameState Game,
    PlayersState Players
)
{ }