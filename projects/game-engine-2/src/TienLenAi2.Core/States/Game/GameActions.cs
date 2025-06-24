using TienLenAi2.Core.Hands;

namespace TienLenAi2.Core.States.Game;

public static class GameActionTypes
{
    public const string SetupGame = "Game/SetupGame";
    public const string StartGame = "Game/StartGame";
    public const string UpdateGamePhase = "Game/UpdateGamePhase";
    public const string PlayHand = "Game/PlayHand";
}

public record SetupGameAction(string Type, int StartingPlayerId) : IAction { }
public record StartGameAction(string Type) : IAction { }
public record UpdateGamePhaseAction(string Type, GamePhase Phase) : IAction;
public record PlayHandAction(string Type, int PlayerId, Hand Hand) : IAction;