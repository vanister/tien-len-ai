namespace TienLenAi2.Core.States;

public record HistoryState(
    long SequenceNumber,
    IAction Action
)
{ }