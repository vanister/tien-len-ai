using TienLenAi2.Core.States.Actions;

namespace TienLenAi2.Core.States.Reducers;

public static class GameReducer
{
    public static GameState Reduce(GameState currentState, IAction action)
    {
        return action.Type switch
        {
            GameActionTypes.SetupGame => GameStateUpdater.SetupGame(currentState, (GameSetupAction)action),
            _ => currentState
        };
    }
}

public static class GameActionTypes
{
    public const string SetupGame = "Game/SetupGame";
}