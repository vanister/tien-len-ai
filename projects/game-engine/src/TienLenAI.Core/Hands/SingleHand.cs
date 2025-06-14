using TienLenAI.Core.Cards;

namespace TienLenAI.Core.Hands;

public class SingleHand : Hand
{
    public override HandType Type => HandType.Single;

    public SingleHand(Card card) : base([card]) { }

    public override bool IsValid()
    {
        return Cards.Count == 1;
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

        // Compare the single cards
        return Cards[0].CompareTo(other.Cards[0]);
    }
}
