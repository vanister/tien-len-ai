namespace TienLenAi2.Core.States.Game;

public static class GameReducer
{
    public static GameState Reduce(GameState currentState, IAction action)
    {
        return action.Type switch
        {
            GameActionTypes.SetupGame => GameStateUpdater.SetupGame(currentState, (SetupGameAction)action),
            GameActionTypes.StartGame => GameStateUpdater.StartGame(currentState, (StartGameAction)action),
            GameActionTypes.UpdateGamePhase => GameStateUpdater.UpdateGamePhase(currentState, (UpdateGamePhaseAction)action),
            _ => currentState
        };
    }
}
