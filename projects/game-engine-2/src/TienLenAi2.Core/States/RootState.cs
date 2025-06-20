using TienLenAi2.Core.States.Game;
using TienLenAi2.Core.States.Players;

namespace TienLenAi2.Core.States;

public record RootState(
    GameState Game,
    PlayersState Players
)
{ }