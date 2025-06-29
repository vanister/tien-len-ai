namespace TienLenAi2.Core.States.Game;

public static class GameReducer
{
    public static GameState Reduce(GameState currentState, IAction action)
    {
        return action.Type switch
        {
            GameActionTypes.StartGame => GameStateUpdater.StartGame(currentState, (StartGameAction)action),
            GameActionTypes.UpdateGamePhase => GameStateUpdater.UpdateGamePhase(currentState, (UpdateGamePhaseAction)action),
            GameActionTypes.PlayHand => GameStateUpdater.PlayHand(currentState, (PlayHandAction)action),
            GameActionTypes.Pass => GameStateUpdater.Pass(currentState, (PassAction)action),
            _ => currentState
        };
    }
}
