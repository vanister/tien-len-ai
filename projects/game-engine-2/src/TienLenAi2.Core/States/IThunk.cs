namespace TienLenAi2.Core.States;

public interface IThunk
{
    IEnumerable<IAction> Execute(RootState currentState);
}
