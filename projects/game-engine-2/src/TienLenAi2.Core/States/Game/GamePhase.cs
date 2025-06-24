namespace TienLenAi2.Core.States.Game;

public enum GamePhase: byte
{
    NotStarted = 0,
    AddPlayer = 1,
    Dealing = 2,
    Starting = 3,
    Playing = 4,
    TrickStart = 5,
    TrickComplete = 6,
    GameComplete = 7
}