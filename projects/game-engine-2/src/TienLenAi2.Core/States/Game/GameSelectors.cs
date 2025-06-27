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
        
        throw new NotImplementedException("IsTrickOver selector is not implemented yet.");
    }
}