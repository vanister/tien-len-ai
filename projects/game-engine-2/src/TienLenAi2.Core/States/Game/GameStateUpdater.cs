using System.Reflection;

namespace TienLenAi2.Core.States.Game;

public static class GameStateUpdater
{
    public static GameState StartGame(GameState state, StartGameAction action)
    {
        return state with
        {
            CurrentPlayerId = action.StartingPlayerId,
            GameNumber = state.GameNumber + 1,
        };
    }

    public static GameState UpdateGamePhase(GameState state, UpdateGamePhaseAction action)
    {
        return state with
        {
            Phase = action.Phase
        };
    }

    public static GameState PlayHand(GameState state, PlayHandAction action)
    {
        var hand = action.Hand;
        // when a hand is played, that means its the start of a new trick
        var currentTrick = new CurrentTrick(action.PlayerId, hand);

        return state with
        {
            PlayedHands = state.PlayedHands.Add(hand),
            CurrentTrick = currentTrick
        };
    }

    public static GameState Pass(GameState state, PassAction action)
    {
        var playerId = action.PlayerId;
        // Add the player to the set of players who have passed
        var updatedPlayersPassed = state.PlayersPassed.Add(playerId);
        // the trick is over if all but one player has passed   
        var isTrickOver = updatedPlayersPassed.Count == action.TotalPlayers - 1;

        return state with
        {
            PlayersPassed = updatedPlayersPassed,
            IsTrickOver = isTrickOver
        };
    }
}