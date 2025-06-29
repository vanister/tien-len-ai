using System.Collections.Immutable;
using TienLenAi2.Core.States.Game;
using TienLenAi2.Core.States.Players;

namespace TienLenAi2.Core.States;

/// <summary>
/// Represents the root state of the game, containing the game state, players state, history of actions, and a sequence number.
/// </summary>
/// <param name="Game">The state of the game.</param>
/// <param name="Players">The players in th game.</param>
/// <param name="History">The complete list of actions taken in the game.</param>
/// <param name="ActionSequenceNumber">The sequence number of the action.</param>
public record RootState(
    GameState Game,
    PlayersState Players,
    long ActionSequenceNumber = 0
)
{
    public ImmutableList<HistoryState> History { get; init; } = [];

    public static RootState CreateDefault()
    {
        var game = GameState.CreateDefault();
        var players = PlayersState.CreateDefault();

        return new RootState(game, players);
    }
}