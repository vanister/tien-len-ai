using System.Reflection;

namespace TienLenAi2.Core.States.Game;

public static class GameStateUpdater
{
    public static GameState SetupGame(GameState state, GameSetupAction action)
    {
        return state;
    }

    public static GameState StartGame(GameState state, StartGameAction action)
    {
        // check if this is the first game
        // if so, make sure the player holding the 3 of spades is the first player
        // if not, set the first player to the last player that won

        return state;
    }

    public static GameState UpdateGamePhase(GameState state, UpdateGamePhaseAction action)
    {
        ArgumentNullException.ThrowIfNull(action);
        return state with { Phase = action.Phase };
    }
}