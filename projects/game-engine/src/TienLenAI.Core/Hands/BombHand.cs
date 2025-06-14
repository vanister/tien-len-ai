using TienLenAI.Core.Cards;

namespace TienLenAI.Core.Hands;

public class BombHand : Hand
{
    public override HandType Type => HandType.Bomb;

    public BombHand(IEnumerable<Card> cards) : base(cards) { }

    public override bool IsValid()
    {
        // A bomb must have exactly 4 cards
        if (Cards.Count != 4)
        {
            return false;
        }

        // All cards must have the same rank
        var rank = Cards[0].Rank;
        return Cards.All(card => card.Rank == rank);
    }

    public override int CompareTo(Hand? other)
    {
        if (other == null)
        {
            return 1;
        }

        // For non-bomb hands, bomb always wins
        if (other.Type != HandType.Bomb)
        {
            return 1;
        }

        var otherBomb = (BombHand)other;

        // Compare the highest cards in each bomb
        // Since cards are ordered by value in the base class constructor,
        // and all cards in a bomb have the same rank,
        // we can compare any card (we'll use the last one for consistency)
        return Cards[^1].CompareTo(otherBomb.Cards[^1]);
    }
}
