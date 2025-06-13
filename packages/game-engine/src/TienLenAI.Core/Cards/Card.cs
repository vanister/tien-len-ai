namespace TienLenAI.Core.Cards;

/// <summary>
/// Immutable card representation optimized for AI training and fast comparisons.
/// Uses ranks 3-2 (3,4,5,6,7,8,9,10,J,Q,K,A,2) with suits ♠,♣,♦,♥.
/// Each card has a unique value from 0-51 for efficient processing.
/// </summary>
public readonly struct Card : IEquatable<Card>, IComparable<Card>
{
    public CardRank Rank { get; }
    public CardSuit Suit { get; }

    /// <summary>
    /// Unique value from 0-51 representing card's absolute ranking.<br/><br/>
    /// Value = ((rank-3) * 4) + suit, where:<br/>
    /// - rank is the numerical value (3=3, 4=4, ..., J=11, Q=12, K=13, A=14, 2=15)<br/>
    /// - suit is the enum value (Spades=0, Clubs=1, Diamonds=2, Hearts=3)<br/><br/>
    /// Examples: 3♠=0, 3♣=1, 3♦=2, 3♥=3, 4♠=4, ..., A♥=47, 2♠=48, 2♣=49, 2♦=50, 2♥=51
    /// </summary>
    public int Value { get; }

    public Card(CardRank rank, CardSuit suit)
    {
        Rank = rank;
        Suit = suit;
        Value = ((int)rank - 3) * 4 + (int)suit;
    }

    /// <summary>
    /// Create card from unique value (0-51)
    /// </summary>
    public Card(int value)
    {
        if (value < 0 || value > 51)
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                "Card value must be between 0 and 51");
        }

        Value = value;
        Rank = (CardRank)(value / 4 + 3);
        Suit = (CardSuit)(value % 4);
    }

    public bool Equals(Card other) => Value == other.Value;

    public override bool Equals(object? obj) => obj is Card other && Equals(other);

    public override int GetHashCode() => Value.GetHashCode();

    public int CompareTo(Card other) => Value.CompareTo(other.Value);

    public static bool operator ==(Card left, Card right) => left.Equals(right);
    public static bool operator !=(Card left, Card right) => !left.Equals(right);
    public static bool operator <(Card left, Card right) => left.Value < right.Value;
    public static bool operator >(Card left, Card right) => left.Value > right.Value;
    public static bool operator <=(Card left, Card right) => left.Value <= right.Value;
    public static bool operator >=(Card left, Card right) => left.Value >= right.Value;

    public override string ToString()
    {
        var rankStr = Rank switch
        {
            CardRank.Jack => "J",
            CardRank.Queen => "Q",
            CardRank.King => "K",
            CardRank.Ace => "A",
            CardRank.Two => "2",
            _ => Rank.ToString()
        };

        var suitStr = Suit switch
        {
            CardSuit.Spades => "♠",
            CardSuit.Clubs => "♣",
            CardSuit.Diamonds => "♦",
            CardSuit.Hearts => "♥",
            _ => throw new InvalidOperationException("Invalid suit")
        };

        return $"{rankStr}{suitStr}";
    }
}