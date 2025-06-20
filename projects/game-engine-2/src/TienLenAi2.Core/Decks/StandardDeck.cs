using System.Collections.Immutable;
using TienLenAi2.Core.Cards;

namespace TienLenAi2.Core.Decks;

// todo - consider making this an interface so we can have different deck types
// like AiStandardDeck, SecureDeck (for multiplayer), etc.
public class StandardDeck
{
    private static readonly ImmutableList<Card> _standardDeck = CreateStandardDeck();

    public static ImmutableList<Card> Create() => _standardDeck;

    public static ImmutableList<Card> CreateShuffled(int seed = 0)
    {
        var rand = new Random(seed);
        var shuffledDeck = CreateStandardDeck().OrderBy(_ => rand.Next());

        return [.. shuffledDeck];
    }

    private static ImmutableList<Card> CreateStandardDeck()
    {
        var cards = new List<Card>();

        foreach (Suit suit in Enum.GetValues(typeof(Suit)))
        {
            foreach (Rank rank in Enum.GetValues(typeof(Rank)))
            {
                cards.Add(new Card(rank, suit));
            }
        }

        return [.. cards];
    }
}