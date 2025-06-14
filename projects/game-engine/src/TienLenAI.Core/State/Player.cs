using TienLenAI.Core.Cards;

namespace TienLenAI.Core.State;

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
