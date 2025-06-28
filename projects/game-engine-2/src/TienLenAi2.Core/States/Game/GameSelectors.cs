namespace TienLenAi2.Core.States.Game;

public static class GameSelectors
{
    public static bool IsFirstGame(RootState state)
    {
        return state.Game.GameNumber == 0;
    }

    public static bool IsTrickOver(RootState state)
    {
        // check if all other players have passed except the starting trick player
        var currentPlayerId = state.Game.CurrentPlayerId;

        if (currentPlayerId == null)
        {
            // If there is no current player, the trick is over since no one can play
            return false;
        }

        var totalPlayers = state.Players.TotalPlayers;
        var playersPassed = state.Game.PlayersPassed;

        if (playersPassed.Count == totalPlayers - 1 && !playersPassed.Contains(currentPlayerId.Value))
        {
            // If all players except the current player have passed, the trick is over
            return true;
        }

        return false;
    }
}