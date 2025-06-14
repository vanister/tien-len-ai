using TienLenAI.Core.Cards;

namespace TienLenAI.Core.Hands;

public class StraightHand : Hand
{
    public override HandType Type => HandType.Straight;

    public StraightHand(IEnumerable<Card> cards) : base(cards) { }

    public override bool IsValid()
    {
        // Check length constraints (3-12 cards)
        if (Cards.Count < 3 || Cards.Count > 12)
        {
            return false;
        }

        // 2's cannot be used in straights
        if (Cards.Any(card => card.Rank == CardRank.Two))
        {
            return false;
        }

        // Check if ranks are consecutive
        for (int i = 0; i < Cards.Count - 1; i++)
        {
            // Since cards are ordered by value in the base class,
            // we can just check if each consecutive pair differs by 1
            if ((int)Cards[i + 1].Rank - (int)Cards[i].Rank != 1)
            {
                return false;
            }
        }

        return true;
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

        var otherStraight = (StraightHand)other;

        // Must be same length to compare
        if (Cards.Count != otherStraight.Cards.Count)
        {
            throw new InvalidOperationException(
                $"Cannot compare straights of different lengths: {Cards.Count} vs {otherStraight.Cards.Count}");
        }

        // Compare the highest cards in each straight
        // Since cards are ordered by value in the base class constructor,
        // the highest card will be the last one
        return Cards[^1].CompareTo(otherStraight.Cards[^1]);
    }
}
