using TienLenAI.Core.Cards;

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

public class Player
{
    public string Name { get; set; }
    public List<Card> Hand { get; set; }

    public Player(string name, List<Card> hand)
    {
        Name = name;
        Hand = hand;
    }
}
