using TienLenAi2.Core.Hands;

namespace TienLenAi2.Core.States.Game;

public static class GameActionTypes
{
    public const string StartGame = "Game/StartGame";
    public const string UpdateGamePhase = "Game/UpdateGamePhase";
    public const string PlayHand = "Game/PlayHand";
    public const string Pass = "Game/Pass";
    public const string NextTurn = "Game/NextTurn";
    public const string UpdateWinner = "Game/UpdateWinner";
    public const string NewGame = "Game/NewGame";
}

public record StartGameAction(string Type, int StartingPlayerId) : IAction { }
public record UpdateGamePhaseAction(string Type, GamePhase Phase) : IAction;
public record PlayHandAction(string Type, int PlayerId, Hand Hand) : IAction;
public record PassAction(string Type, int PlayerId) : IAction;
public record NextTurnAction(string Type) : IAction;
public record UpdateWinnerAction(string Type, int PlayerId) : IAction;
public record NewGameAction(string Type, int? WinningPlayerId = null) : IAction;