namespace TienLenAi2.Core.States.Game;

public enum GamePhase: int
{
    NotStarted = 0,
    Setup = 1,
    Dealing = 2,
    Starting = 3,
    TrickStart = 4,
    Playing = 5,
    TrickComplete = 6,
    GameComplete = 7
}