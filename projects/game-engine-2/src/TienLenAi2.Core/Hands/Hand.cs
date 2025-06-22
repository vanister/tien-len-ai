using System.Collections.Immutable;
using TienLenAi2.Core.Cards;

namespace TienLenAi2.Core.Hands;

public record Hand : IComparable<Hand>
{
    public HandType Type { get; init; }
    public ImmutableList<Card> Cards { get; init; } = [];
    public Rank Rank => HighestCard.Rank;
    public Card HighestCard { get; init; }
    public bool ContainsThreeOfSpades { get; init; }
    public int Value { get; init; }

    public Hand(HandType type, IEnumerable<Card> cards)
    {
        ArgumentNullException.ThrowIfNull(cards);

        Type = type;
        Cards = [.. cards];
        HighestCard = Cards.Max();
        Value = HighestCard.Value;
        ContainsThreeOfSpades = Cards.Contains(Card.ThreeOfSpades);

        if (Cards.Count == 0)
        {
            throw new ArgumentException("Hand must contain at least one card", nameof(cards));
        }
    }

    public int CompareTo(Hand? other)
    {
        if (other is null)
        {
            return 1; // This hand is greater than null
        }

        // // different hand types (both non-bombs) can't be compared
        // if (Type != other.Type && Type != HandType.Bomb && other.Type != HandType.Bomb)
        // throw new InvalidOperationException($"Cannot compare {Type} with {other.Type}");

        return Value.CompareTo(other.Value);
    }

    public bool CanBeat(Hand? other)
    {
        if (other is null)
        {
            return false;
        }

        try
        {
            return CompareTo(other) > 0;
        }
        catch (InvalidOperationException)
        {
            return false;
        }
    }

    public override string ToString()
    {
        return $"{Type}: {string.Join(" ", Cards.Select(c => c.ToString()))}";
    }
}
