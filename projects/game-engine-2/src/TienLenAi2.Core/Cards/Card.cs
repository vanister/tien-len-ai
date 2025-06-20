using System.Collections.Immutable;

namespace TienLenAi2.Core.Cards;

public readonly struct Card : IComparable<Card>, IEquatable<Card>
{
    public readonly Suit Suit { get; }
    public readonly Rank Rank { get; }

    // Value combines rank and suit into a single unique value.
    // Each rank gets 4 positions (one for each suit).
    // This gives a unique value from 12-63 for each card in a standard deck.
    public readonly int Value => ((int)Rank * 4) + (int)Suit;

    public Card(Rank rank, Suit suit)
    {
        Rank = rank;
        Suit = suit;
    }

    public override string ToString()
    {
        string rankSymbol = Rank switch
        {
            Rank.Three => "3",
            Rank.Four => "4",
            Rank.Five => "5",
            Rank.Six => "6",
            Rank.Seven => "7",
            Rank.Eight => "8",
            Rank.Nine => "9",
            Rank.Ten => "10",
            Rank.Jack => "J",
            Rank.Queen => "Q",
            Rank.King => "K",
            Rank.Ace => "A",
            Rank.Two => "2",
            _ => "?"
        };

        string suitSymbol = Suit switch
        {
            Suit.Spades => "♠",
            Suit.Clubs => "♣",
            Suit.Diamonds => "♦",
            Suit.Hearts => "♥",
            _ => "?"
        };

        return $"{rankSymbol}{suitSymbol}";
    }

    public int CompareTo(Card other)
    {
        // Using Value for comparison is more efficient
        // since it already encapsulates rank and suit ordering
        return Value.CompareTo(other.Value);
    }

    public bool Equals(Card other)
    {
        return Rank == other.Rank && Suit == other.Suit;
    }

    public override bool Equals(object? obj)
    {
        return obj is Card card && Equals(card);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Rank, Suit);
    }

    // Operator overloads for comparison
    public static bool operator <(Card left, Card right) => left.CompareTo(right) < 0;
    public static bool operator >(Card left, Card right) => left.CompareTo(right) > 0;
    public static bool operator <=(Card left, Card right) => left.CompareTo(right) <= 0;
    public static bool operator >=(Card left, Card right) => left.CompareTo(right) >= 0;
    public static bool operator ==(Card left, Card right) => left.Equals(right);
    public static bool operator !=(Card left, Card right) => !left.Equals(right);
    
    public static Card ThreeOfSpades => new Card(Rank.Three, Suit.Spades);
    
    public static ImmutableList<Card> CreateStandardDeck()
    {
        var cards = new List<Card>(52);
        
        foreach (Rank rank in Enum.GetValues<Rank>())
        {
            foreach (Suit suit in Enum.GetValues<Suit>())
            {
                cards.Add(new Card(rank, suit));
            }
        }
        
        return [.. cards];
    }
}
