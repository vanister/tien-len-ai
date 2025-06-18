namespace TienLenAi2.Core;

public record Card(Rank Rank, Suit Suit) : IComparable<Card>
{
    public int Value => (int)Rank * 4 + (int)Suit;

    public int CompareTo(Card? other)
    {
        if (other is null)
        {
            return 1;
        }

        var rankComparison = Rank.CompareTo(other.Rank);
        if (rankComparison != 0)
        {
            return rankComparison;
        }

        return Suit.CompareTo(other.Suit);
    }

    public override string ToString()
    {
        var rankStr = Rank switch
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

        var suitSymbol = Suit switch
        {
            Suit.Spades => "♠",
            Suit.Clubs => "♣",
            Suit.Diamonds => "♦",
            Suit.Hearts => "♥",
            _ => "?"
        };

        var suitName = Suit switch
        {
            Suit.Spades => "Spades",
            Suit.Clubs => "Clubs",
            Suit.Diamonds => "Diamonds",
            Suit.Hearts => "Hearts",
            _ => "Unknown"
        };

        return $"{rankStr}{suitSymbol} ({rankStr} of {suitName})";
    }
}
