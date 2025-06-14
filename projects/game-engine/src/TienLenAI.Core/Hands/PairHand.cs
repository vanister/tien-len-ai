using TienLenAI.Core.Cards;

namespace TienLenAI.Core.Hands;

public class PairHand : Hand
{
    public override HandType Type => HandType.Pair;

    public PairHand(IEnumerable<Card> cards) : base(cards) { }

    public override bool IsValid()
    {
        // A pair must have exactly 2 cards
        if (Cards.Count != 2)
        {
            return false;
        }

        // Both cards must have the same rank
        return Cards[0].Rank == Cards[1].Rank;
    }

    public override int CompareTo(Hand? other)
    {
        if (other == null)
        {
            return 1;
        }

        // Handle comparison with Bomb
        if (other.Type == HandType.Bomb)
        {
            return -1;
        }

        // Validate same hand type
        if (other.Type != Type)
        {
            throw new InvalidOperationException(
                $"Cannot compare {Type} with {other.Type}");
        }

        var otherPair = (PairHand)other;

        // Compare the highest cards in each pair
        // Since cards are ordered by value in the base class constructor,
        // the highest card will be at index 1
        return Cards[1].CompareTo(otherPair.Cards[1]);
    }
}
