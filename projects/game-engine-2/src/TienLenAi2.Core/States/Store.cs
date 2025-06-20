using System.Collections.Immutable;
using TienLenAi2.Core.States.Game;
using TienLenAi2.Core.States.Players;

namespace TienLenAi2.Core.States;

public class Store
{
    private RootState _currentState;

    public RootState CurrentState => _currentState;

    public Store()
    {
        var game = new GameState(
            CurrentPlayerId: null,
            TrickNumber: 0,
            Phase: GamePhase.NotStarted,
            PlayedHands: [],
            CurrentHand: null,
            History: []
        );

        var players = new PlayersState(ImmutableDictionary<int, PlayerState>.Empty);

        _currentState = new RootState(
            Game: game,
            Players: players
        );
    }

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