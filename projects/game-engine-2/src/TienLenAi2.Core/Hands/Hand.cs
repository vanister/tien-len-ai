using System.Collections.Immutable;
using TienLenAi2.Core.Cards;

namespace TienLenAi2.Core.Hands;

public record Hand : IComparable<Hand>
{
    public HandType Type { get; init; }
    public ImmutableList<Card> Cards { get; init; } = [];
    
    // The effective rank value of this hand for comparison purposes.
    // Used to determine which hand is stronger when comparing hands of the same type.
    // For single cards, pairs, triples and bombs, this is the int value of the card's rank.
    // For straights and other combinations, it's based on the highest card.
    public int Rank { get; init; }
    
    public Card HighestCard { get; init; }
    public bool ContainsThreeOfSpades { get; init; }

    public Hand(HandType type, IEnumerable<Card> cards, int rank, Card highestCard)
    {
        ArgumentNullException.ThrowIfNull(cards);
        
        Type = type;
        Cards = [.. cards];
        Rank = rank;
        HighestCard = highestCard;
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
            return 1;
        }
        
        if (Type != other.Type)
        {
            if (Type == HandType.Bomb && other.Type != HandType.Bomb)
            {
                return 1;
            }
            if (other.Type == HandType.Bomb && Type != HandType.Bomb)
            {
                return -1;
            }
            
            throw new InvalidOperationException($"Cannot compare {Type} hand to {other.Type} hand");
        }
        
        return Rank.CompareTo(other.Rank);
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
