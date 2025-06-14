namespace TienLenAI.Core.State;

public class GameState
{
    public List<Player> Players { get; private set; }
    public int CurrentPlayerIndex { get; private set; }

    public GameState(List<Player> players)
    {
        Players = players;
        CurrentPlayerIndex = 0; // Start with the first player
    }

    public Player GetCurrentPlayer()
    {
        return Players[CurrentPlayerIndex];
    }

    public void AdvanceTurn()
    {
        CurrentPlayerIndex = (CurrentPlayerIndex + 1) % Players.Count;
    }
}
