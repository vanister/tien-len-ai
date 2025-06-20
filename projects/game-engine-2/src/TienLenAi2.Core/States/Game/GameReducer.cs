namespace TienLenAi2.Core.States.Game;

public static class GameReducer
{
    public static GameState Reduce(GameState currentState, IAction action)
    {
        return action.Type switch
        {
            GameActionTypes.SetupGame => GameStateUpdater.SetupGame(currentState, (GameSetupAction)action),
            GameActionTypes.UpdateGamePhase => GameStateUpdater.UpdateGamePhase(currentState, (UpdateGamePhaseAction)action),
            _ => currentState
        };
    }
}

public static class GameActionTypes
{
    public const string SetupGame = "Game/SetupGame";
    public const string UpdateGamePhase = "Game/UpdateGamePhase";
}