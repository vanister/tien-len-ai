using TienLenAI.Core.Cards;

namespace TienLenAI.Core.Hands;

public class DoubleStraightHand : Hand
{
    public override HandType Type => HandType.DoubleStraight;

    public DoubleStraightHand(IEnumerable<Card> cards) : base(cards) { }

    public override bool IsValid()
    {
        // Check if total number of cards is valid (must be even and between 6-20)
        if (Cards.Count < 6 || Cards.Count > 20 || Cards.Count % 2 != 0)
        {
            return false;
        }

        // Group cards by rank and verify each rank appears exactly twice
        var rankGroups = Cards.GroupBy(card => card.Rank).ToList();

        // Each rank must appear exactly twice
        if (rankGroups.Any(group => group.Count() != 2))
        {
            return false;
        }

        // Get the sequence of ranks (should be one entry per rank)
        var ranks = rankGroups.Select(g => g.Key).OrderBy(r => r).ToList();

        // 2's cannot be used in straights
        if (ranks.Contains(CardRank.Two))
        {
            return false;
        }

        // Check if ranks are consecutive
        for (int i = 0; i < ranks.Count - 1; i++)
        {
            if ((int)ranks[i + 1] - (int)ranks[i] != 1)
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

        var otherDoubleStraight = (DoubleStraightHand)other;

        // Must be same length to compare
        if (Cards.Count != otherDoubleStraight.Cards.Count)
        {
            throw new InvalidOperationException(
                $"Cannot compare double straights of different lengths: {Cards.Count} vs {otherDoubleStraight.Cards.Count}");
        }

        // Compare the highest cards in each straight
        // Since cards are ordered by value in the base class constructor,
        // the highest card will be the last one
        return Cards[^1].CompareTo(otherDoubleStraight.Cards[^1]);
    }
}
