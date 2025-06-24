using System.Reflection;

namespace TienLenAi2.Core.States.Game;

public static class GameStateUpdater
{
    public static GameState StartGame(GameState state, StartGameAction action)
    {
        return state with
        {
            CurrentPlayerId = action.StartingPlayerId,
            // transition to the next phase of the game
            Phase = action.Phase,
        };
    }

    public static GameState UpdateGamePhase(GameState state, UpdateGamePhaseAction action)
    {
        return state with
        {
            Phase = action.Phase
        };
    }
}