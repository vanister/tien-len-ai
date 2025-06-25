namespace TienLenAi2.Core.States.Game;

public static class GameSelectors
{
    public static bool IsFirstGame(RootState state)
    {
        return state.Game.GameNumber == 0;
    }
}