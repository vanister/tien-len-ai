using TienLenAI.Core.Cards;

namespace TienLenAI.Core.Hands;

public class TripleHand : Hand
{
    public override HandType Type => HandType.Triple;

    public TripleHand(IEnumerable<Card> cards) : base(cards) { }

    public override bool IsValid()
    {
        // A triple must have exactly 3 cards
        if (Cards.Count != 3)
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

        var otherTriple = (TripleHand)other;

        // Compare the highest cards in each triple
        // Since cards are ordered by value in the base class constructor,
        // the highest card will be at index 2
        return Cards[2].CompareTo(otherTriple.Cards[2]);
    }
}
