namespace TienLenAi2.Core.States;

public class Store
{
    private RootState _currentState;

    public RootState CurrentState => _currentState;

    public Store() : this(RootState.CreateDefault()) { }

    public Store(RootState initialState)
    {
        ArgumentNullException.ThrowIfNull(initialState);
        _currentState = initialState;
    }

    public void Dispatch(IAction action)
    {
        ArgumentNullException.ThrowIfNull(action);

        _currentState = RootReducer.Reduce(_currentState, action);
    }
}