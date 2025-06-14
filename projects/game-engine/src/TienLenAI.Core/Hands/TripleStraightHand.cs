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
        if (Cards.Count < 6 || Cards.Count % 3 != 0)
            return false;

        // Group cards by rank to ensure we have triples
        var rankGroups = Cards.GroupBy(c => c.Rank).OrderBy(g => g.Key).ToList();

        // Check if we have the right number of consecutive triples
        if (rankGroups.Count != Cards.Count / 3)
            return false;

        // Ensure each group has exactly 3 cards and all cards have different suits
        foreach (var group in rankGroups)
        {
            if (group.Count() != 3)
                return false;

            // Check that all suits in this triple are different
            var suits = group.Select(c => c.Suit).ToList();
            if (suits.Distinct().Count() != 3)
                return false;
        }

        // Check if ranks are consecutive
        for (int i = 1; i < rankGroups.Count; i++)
        {
            if (rankGroups[i].Key != rankGroups[i - 1].Key + 1)
                return false;
        }

        return true;
    }

    /// <inheritdoc/>
    public override int CompareTo(Hand? other)
    {
        if (other == null) return 1;

        // Handle comparison with BombHand
        if (other is BombHand) return -1;

        if (other is not TripleStraightHand otherTripleStraight)
            throw new InvalidOperationException($"Cannot compare a Triple Straight hand with a {other.GetType().Name}");

        if (Cards.Count != otherTripleStraight.Cards.Count)
            throw new InvalidOperationException("Cannot compare Triple Straight hands of different lengths");

        // Compare based on the highest triple
        var thisHighestRank = Cards.Max(c => c.Rank);
        var otherHighestRank = otherTripleStraight.Cards.Max(c => c.Rank);

        return thisHighestRank.CompareTo(otherHighestRank);
    }
}
