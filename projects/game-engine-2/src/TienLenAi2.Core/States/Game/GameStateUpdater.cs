namespace TienLenAi2.Core.States.Game;

public static class GameStateUpdater
{
    public static GameState SetupGame(GameState state, GameSetupAction action)
    {
        return state;
    }

    public static GameState UpdateGamePhase(GameState state, UpdateGamePhaseAction action)
    {
        ArgumentNullException.ThrowIfNull(action);
        return state with { Phase = action.Phase };
    }
}