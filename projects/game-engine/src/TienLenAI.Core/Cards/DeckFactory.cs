using System.Security.Cryptography;

namespace TienLenAI.Core.Cards;

/// <summary>
/// Factory for creating and shuffling standard 52-card decks for Tien Len games.
/// Uses cryptographically secure random number generation for fair shuffling.
/// </summary>
public static class DeckFactory
{
    /// <summary>
    /// Creates a standard 52-card deck in order (3♠ to 2♥).
    /// </summary>
    /// <returns>All 52 cards in ascending order by value</returns>
    public static List<Card> CreateStandardDeck()
    {
        var deck = new List<Card>(52);

        foreach (CardRank rank in Enum.GetValues<CardRank>())
        {
            foreach (CardSuit suit in Enum.GetValues<CardSuit>())
            {
                deck.Add(new Card(rank, suit));
            }
        }

        return deck;
    }

    /// <summary>
    /// Creates a shuffled standard 52-card deck using cryptographically secure randomization.
    /// </summary>
    /// <returns>All 52 cards in random order</returns>
    public static List<Card> CreateShuffledDeck()
    {
        var deck = CreateStandardDeck();
        Shuffle(deck);
        return deck;
    }

    /// <summary>
    /// Creates a shuffled deck using the provided random number generator.
    /// Useful for testing with seeded random generators.
    /// </summary>
    /// <param name="random">Random number generator to use for shuffling</param>
    /// <returns>All 52 cards in random order</returns>
    public static List<Card> CreateShuffledDeck(Random random)
    {
        if (random == null)
        {
            throw new ArgumentNullException(nameof(random));
        }

        var deck = CreateStandardDeck();
        Shuffle(deck, random);
        return deck;
    }

    /// <summary>
    /// Shuffles a deck in place using the Fisher-Yates algorithm with cryptographically secure randomization.
    /// </summary>
    /// <param name="deck">The deck to shuffle</param>
    public static void Shuffle(List<Card> deck)
    {
        if (deck == null)
        {
            throw new ArgumentNullException(nameof(deck));
        }

        if (deck.Count <= 1)
        {
            return;
        }

        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[4];

        for (int i = deck.Count - 1; i > 0; i--)
        {
            // Generate cryptographically secure random index
            rng.GetBytes(bytes);
            var randomValue = BitConverter.ToUInt32(bytes, 0);
            var j = (int)(randomValue % (uint)(i + 1));

            // Swap cards
            (deck[i], deck[j]) = (deck[j], deck[i]);
        }
    }

    /// <summary>
    /// Shuffles a deck in place using the Fisher-Yates algorithm with the provided random number generator.
    /// </summary>
    /// <param name="deck">The deck to shuffle</param>
    /// <param name="random">Random number generator to use</param>
    public static void Shuffle(List<Card> deck, Random random)
    {
        if (deck == null)
        {
            throw new ArgumentNullException(nameof(deck));
        }

        if (random == null)
        {
            throw new ArgumentNullException(nameof(random));
        }

        if (deck.Count <= 1)
        {
            return;
        }

        for (int i = deck.Count - 1; i > 0; i--)
        {
            var j = random.Next(i + 1);
            (deck[i], deck[j]) = (deck[j], deck[i]);
        }
    }

    /// <summary>
    /// Deals cards from a deck to the specified number of players.
    /// Each player receives the same number of cards, dealt in round-robin fashion.
    /// </summary>
    /// <param name="deck">The deck to deal from</param>
    /// <param name="numberOfPlayers">Number of players to deal to</param>
    /// <param name="cardsPerPlayer">Number of cards each player should receive</param>
    /// <returns>List of hands, one for each player</returns>
    /// <exception cref="ArgumentException">Thrown when deck doesn't have enough cards</exception>
    public static List<List<Card>> DealCards(List<Card> deck, int numberOfPlayers, int cardsPerPlayer)
    {
        if (deck == null)
        {
            throw new ArgumentNullException(nameof(deck));
        }

        if (numberOfPlayers <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(numberOfPlayers),
                "Number of players must be positive");
        }

        if (cardsPerPlayer < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(cardsPerPlayer),
                "Cards per player cannot be negative");
        }

        var totalCardsNeeded = numberOfPlayers * cardsPerPlayer;
        if (deck.Count < totalCardsNeeded)
        {
            throw new ArgumentException(
                $"Deck has {deck.Count} cards but needs {totalCardsNeeded} cards " +
                $"for {numberOfPlayers} players with {cardsPerPlayer} cards each");
        }

        var hands = new List<List<Card>>(numberOfPlayers);
        for (int i = 0; i < numberOfPlayers; i++)
        {
            hands.Add(new List<Card>(cardsPerPlayer));
        }

        // Deal cards in round-robin fashion
        var cardIndex = 0;
        for (int round = 0; round < cardsPerPlayer; round++)
        {
            for (int player = 0; player < numberOfPlayers; player++)
            {
                hands[player].Add(deck[cardIndex++]);
            }
        }

        return hands;
    }

    /// <summary>
    /// Creates a complete Tien Len game setup with 4 players, each having 13 cards.
    /// Uses a shuffled deck and returns player hands sorted by card value.
    /// </summary>
    /// <returns>List of 4 hands, each containing 13 sorted cards</returns>
    public static List<List<Card>> CreateTienLenGame()
    {
        var shuffledDeck = CreateShuffledDeck();
        var hands = DealCards(shuffledDeck, 4, 13);

        // Sort each player's hand by card value for easier gameplay
        foreach (var hand in hands)
        {
            hand.Sort((a, b) => a.Value.CompareTo(b.Value));
        }

        return hands;
    }

    /// <summary>
    /// Creates a Tien Len game setup using a seeded random generator for reproducible results.
    /// Useful for testing and debugging.
    /// </summary>
    /// <param name="seed">Seed for the random number generator</param>
    /// <returns>List of 4 hands, each containing 13 sorted cards</returns>
    public static List<List<Card>> CreateTienLenGame(int seed)
    {
        var random = new Random(seed);
        var shuffledDeck = CreateShuffledDeck(random);
        var hands = DealCards(shuffledDeck, 4, 13);

        // Sort each player's hand by card value for easier gameplay
        foreach (var hand in hands)
        {
            hand.Sort((a, b) => a.Value.CompareTo(b.Value));
        }

        return hands;
    }
}
