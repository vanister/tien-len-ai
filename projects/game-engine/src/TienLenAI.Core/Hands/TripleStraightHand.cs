using TienLenAI.Core.Cards;

namespace TienLenAI.Core.Hands;

/// <summary>
/// Represents a Triple Straight hand in Tiến Lên, consisting of consecutive triples (e.g., 777-888 or 444-555-666).
/// A Triple Straight must have at least two consecutive triples.
/// </summary>
public class TripleStraightHand : Hand
{
    /// <summary>
    /// Initializes a new instance of the TripleStraightHand class with the specified cards.
    /// </summary>
    /// <param name="cards">The cards that make up this hand. Must be at least 6 cards (2 consecutive triples).</param>
    /// <exception cref="ArgumentException">Thrown when the cards don't form a valid triple straight.</exception>
    public TripleStraightHand(IEnumerable<Card> cards) : base(cards)
    {
    }

    /// <inheritdoc/>
    public override HandType Type => HandType.TripleStraight;

    /// <inheritdoc/>
    public override bool IsValid()
    {
        // Must have at least 6 cards (2 consecutive triples) and be divisible by 3
        if (Cards.Count < 6 || Cards.Count % 3 != 0)
        {
            return false;
        }

        // Group cards by rank and verify each rank appears exactly 3 times
        var rankGroups = Cards.GroupBy(card => card.Rank).ToList();

        // Each rank must appear exactly 3 times
        if (rankGroups.Any(group => group.Count() != 3))
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

    /// <inheritdoc/>
    public override int CompareTo(Hand? other)
    {
        if (other == null)
        {
            return 1;
        }

        // Handle comparison with BombHand
        if (other is BombHand)
        {
            return -1;
        }

        if (other is not TripleStraightHand otherTripleStraight)
        {
            throw new InvalidOperationException($"Cannot compare a Triple Straight hand with a {other.GetType().Name}");
        }

        if (Cards.Count != otherTripleStraight.Cards.Count)
        {
            throw new InvalidOperationException("Cannot compare Triple Straight hands of different lengths");
        }

        // Compare based on the highest triple
        var thisHighestRank = Cards.Max(c => c.Rank);
        var otherHighestRank = otherTripleStraight.Cards.Max(c => c.Rank);

        return thisHighestRank.CompareTo(otherHighestRank);
    }
}
