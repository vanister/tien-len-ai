namespace TienLenAi2.Core.States.Game;

public static class GameActionTypes
{
    public const string SetupGame = "Game/SetupGame";
    public const string StartGame = "Game/StartGame";
    public const string UpdateGamePhase = "Game/UpdateGamePhase";
}

public record SetupGameAction(string Type, int StartingPlayerId) : IAction { }
public record StartGameAction(string Type) : IAction { }
public record UpdateGamePhaseAction(string Type, GamePhase Phase) : IAction;
