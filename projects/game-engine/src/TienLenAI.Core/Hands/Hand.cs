using TienLenAI.Core.Cards;

namespace TienLenAI.Core.Hands;

public abstract class Hand : IComparable<Hand>
{
    public IReadOnlyList<Card> Cards { get; }
    public abstract HandType Type { get; }

    protected Hand(IEnumerable<Card> cards)
    {
        Cards = cards.OrderBy(c => c.Value).ToList();
    }

    /// <summary>
    /// Validates if the cards form a valid hand of this type
    /// </summary>
    public abstract bool IsValid();

    /// <summary>
    /// Compares this hand with another hand of the same type.
    /// </summary>
    /// <returns>
    /// Returns positive if this hand is stronger, negative if weaker, 0 if equal.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if hands are of different types (except for Bombs).
    /// </exception>
    public abstract int CompareTo(Hand? other);
}
