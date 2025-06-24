using TienLenAi2.Core.Hands;

namespace TienLenAi2.Core.States.Game;

public static class GameActionTypes
{
    public const string StartGame = "Game/StartGame";
    public const string UpdateGamePhase = "Game/UpdateGamePhase";
    public const string PlayHand = "Game/PlayHand";
}

public record StartGameAction(string Type, int StartingPlayerId, GamePhase Phase = GamePhase.Playing) : IAction { }
public record UpdateGamePhaseAction(string Type, GamePhase Phase) : IAction;
public record PlayHandAction(string Type, int PlayerId, Hand Hand) : IAction;