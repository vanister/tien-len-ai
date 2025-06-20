namespace TienLenAi2.Core.States.Game;

public record UpdateGamePhaseAction(
    string Type,
    GamePhase Phase
) : IAction;
