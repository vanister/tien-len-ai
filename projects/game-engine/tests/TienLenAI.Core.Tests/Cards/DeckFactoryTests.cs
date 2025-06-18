using TienLenAI.Core.Cards;

namespace TienLenAI.Core.Tests.Cards;

[TestClass]
public class DeckFactoryTests
{
    [TestMethod]
    public void CreateStandardDeck_ShouldReturn52Cards()
    {
        // Act
        var deck = DeckFactory.CreateStandardDeck();

        // Assert
        Assert.AreEqual(52, deck.Count);
    }

    [TestMethod]
    public void CreateStandardDeck_ShouldContainAllCards()
    {
        // Act
        var deck = DeckFactory.CreateStandardDeck();

        // Assert
        var expectedCards = new HashSet<Card>();
        foreach (CardRank rank in Enum.GetValues<CardRank>())
        {
            foreach (CardSuit suit in Enum.GetValues<CardSuit>())
            {
                expectedCards.Add(new Card(rank, suit));
            }
        }

        var actualCards = new HashSet<Card>(deck);
        Assert.AreEqual(52, expectedCards.Count);
        Assert.AreEqual(52, actualCards.Count);
        Assert.IsTrue(expectedCards.SetEquals(actualCards));
    }

    [TestMethod]
    public void CreateStandardDeck_ShouldBeInOrder()
    {
        // Act
        var deck = DeckFactory.CreateStandardDeck();

        // Assert - Should be sorted by card value (3♠=0 to 2♥=51)
        for (int i = 0; i < deck.Count - 1; i++)
        {
            Assert.IsTrue(deck[i].Value < deck[i + 1].Value,
                $"Card at index {i} ({deck[i]}) should have lower value than card at index {i + 1} ({deck[i + 1]})");
        }
    }

    [TestMethod]
    public void CreateShuffledDeck_ShouldReturn52Cards()
    {
        // Act
        var deck = DeckFactory.CreateShuffledDeck();

        // Assert
        Assert.AreEqual(52, deck.Count);
    }

    [TestMethod]
    public void CreateShuffledDeck_ShouldContainAllCards()
    {
        // Act
        var deck = DeckFactory.CreateShuffledDeck();

        // Assert
        var expectedCards = new HashSet<Card>();
        foreach (CardRank rank in Enum.GetValues<CardRank>())
        {
            foreach (CardSuit suit in Enum.GetValues<CardSuit>())
            {
                expectedCards.Add(new Card(rank, suit));
            }
        }

        var actualCards = new HashSet<Card>(deck);
        Assert.IsTrue(expectedCards.SetEquals(actualCards));
    }

    [TestMethod]
    public void CreateShuffledDeck_WithSeed_ShouldBeReproducible()
    {
        // Arrange
        const int seed = 12345;

        // Act
        var deck1 = DeckFactory.CreateShuffledDeck(new Random(seed));
        var deck2 = DeckFactory.CreateShuffledDeck(new Random(seed));

        // Assert
        CollectionAssert.AreEqual(deck1, deck2);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CreateShuffledDeck_WithNullRandom_ShouldThrow()
    {
        // Act & Assert
        DeckFactory.CreateShuffledDeck(null!);
    }

    [TestMethod]
    public void Shuffle_ShouldPreserveAllCards()
    {
        // Arrange
        var originalDeck = DeckFactory.CreateStandardDeck();
        var deckToShuffle = new List<Card>(originalDeck);

        // Act
        DeckFactory.Shuffle(deckToShuffle);

        // Assert
        Assert.AreEqual(originalDeck.Count, deckToShuffle.Count);
        var originalSet = new HashSet<Card>(originalDeck);
        var shuffledSet = new HashSet<Card>(deckToShuffle);
        Assert.IsTrue(originalSet.SetEquals(shuffledSet));
    }

    [TestMethod]
    public void Shuffle_WithSeed_ShouldBeReproducible()
    {
        // Arrange
        const int seed = 54321;
        var deck1 = DeckFactory.CreateStandardDeck();
        var deck2 = DeckFactory.CreateStandardDeck();

        // Act
        DeckFactory.Shuffle(deck1, new Random(seed));
        DeckFactory.Shuffle(deck2, new Random(seed));

        // Assert
        CollectionAssert.AreEqual(deck1, deck2);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Shuffle_WithNullDeck_ShouldThrow()
    {
        // Act & Assert
        DeckFactory.Shuffle(null!);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Shuffle_WithNullRandom_ShouldThrow()
    {
        // Arrange
        var deck = DeckFactory.CreateStandardDeck();

        // Act & Assert
        DeckFactory.Shuffle(deck, null!);
    }

    [TestMethod]
    public void Shuffle_WithEmptyDeck_ShouldNotThrow()
    {
        // Arrange
        var emptyDeck = new List<Card>();

        // Act & Assert
        DeckFactory.Shuffle(emptyDeck); // Should not throw
        Assert.AreEqual(0, emptyDeck.Count);
    }

    [TestMethod]
    public void Shuffle_WithSingleCard_ShouldNotChange()
    {
        // Arrange
        var singleCardDeck = new List<Card> { new Card(CardRank.Ace, CardSuit.Spades) };
        var original = singleCardDeck[0];

        // Act
        DeckFactory.Shuffle(singleCardDeck);

        // Assert
        Assert.AreEqual(1, singleCardDeck.Count);
        Assert.AreEqual(original, singleCardDeck[0]);
    }

    [TestMethod]
    public void DealCards_ShouldReturnCorrectNumberOfHands()
    {
        // Arrange
        var deck = DeckFactory.CreateStandardDeck();

        // Act
        var hands = DeckFactory.DealCards(deck, 4, 13);

        // Assert
        Assert.AreEqual(4, hands.Count);
    }

    [TestMethod]
    public void DealCards_ShouldGiveEachPlayerCorrectNumberOfCards()
    {
        // Arrange
        var deck = DeckFactory.CreateStandardDeck();

        // Act
        var hands = DeckFactory.DealCards(deck, 4, 13);

        // Assert
        foreach (var hand in hands)
        {
            Assert.AreEqual(13, hand.Count);
        }
    }

    [TestMethod]
    public void DealCards_ShouldDistributeAllRequestedCards()
    {
        // Arrange
        var deck = DeckFactory.CreateStandardDeck();

        // Act
        var hands = DeckFactory.DealCards(deck, 4, 13);

        // Assert
        var totalCardsDealt = hands.SelectMany(h => h).Count();
        Assert.AreEqual(52, totalCardsDealt);
    }

    [TestMethod]
    public void DealCards_ShouldNotDuplicateCards()
    {
        // Arrange
        var deck = DeckFactory.CreateStandardDeck();

        // Act
        var hands = DeckFactory.DealCards(deck, 4, 13);

        // Assert
        var allDealtCards = hands.SelectMany(h => h).ToList();
        var uniqueCards = new HashSet<Card>(allDealtCards);
        Assert.AreEqual(allDealtCards.Count, uniqueCards.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void DealCards_WithNullDeck_ShouldThrow()
    {
        // Act & Assert
        DeckFactory.DealCards(null!, 4, 13);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void DealCards_WithZeroPlayers_ShouldThrow()
    {
        // Arrange
        var deck = DeckFactory.CreateStandardDeck();

        // Act & Assert
        DeckFactory.DealCards(deck, 0, 13);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void DealCards_WithNegativeCardsPerPlayer_ShouldThrow()
    {
        // Arrange
        var deck = DeckFactory.CreateStandardDeck();

        // Act & Assert
        DeckFactory.DealCards(deck, 4, -1);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void DealCards_WithInsufficientCards_ShouldThrow()
    {
        // Arrange
        var smallDeck = new List<Card> { new Card(CardRank.Three, CardSuit.Spades) };

        // Act & Assert
        DeckFactory.DealCards(smallDeck, 4, 13); // Needs 52 cards but only has 1
    }

    [TestMethod]
    public void CreateTienLenGame_ShouldReturn4Hands()
    {
        // Act
        var hands = DeckFactory.CreateTienLenGame();

        // Assert
        Assert.AreEqual(4, hands.Count);
    }

    [TestMethod]
    public void CreateTienLenGame_ShouldGiveEachPlayer13Cards()
    {
        // Act
        var hands = DeckFactory.CreateTienLenGame();

        // Assert
        foreach (var hand in hands)
        {
            Assert.AreEqual(13, hand.Count);
        }
    }

    [TestMethod]
    public void CreateTienLenGame_ShouldSortHandsByValue()
    {
        // Act
        var hands = DeckFactory.CreateTienLenGame();

        // Assert
        foreach (var hand in hands)
        {
            for (int i = 0; i < hand.Count - 1; i++)
            {
                Assert.IsTrue(hand[i].Value <= hand[i + 1].Value,
                    $"Hand should be sorted: {hand[i]} (value {hand[i].Value}) should come before {hand[i + 1]} (value {hand[i + 1].Value})");
            }
        }
    }

    [TestMethod]
    public void CreateTienLenGame_ShouldDistributeAll52Cards()
    {
        // Act
        var hands = DeckFactory.CreateTienLenGame();

        // Assert
        var allCards = hands.SelectMany(h => h).ToHashSet();
        Assert.AreEqual(52, allCards.Count);
    }

    [TestMethod]
    public void CreateTienLenGame_WithSeed_ShouldBeReproducible()
    {
        // Arrange
        const int seed = 98765;

        // Act
        var game1 = DeckFactory.CreateTienLenGame(seed);
        var game2 = DeckFactory.CreateTienLenGame(seed);

        // Assert
        Assert.AreEqual(game1.Count, game2.Count);
        for (int i = 0; i < game1.Count; i++)
        {
            CollectionAssert.AreEqual(game1[i], game2[i]);
        }
    }

    [TestMethod]
    public void CreateTienLenGame_OnePlayerShouldHave3Spades()
    {
        // Act
        var hands = DeckFactory.CreateTienLenGame();

        // Assert
        var threeOfSpades = new Card(CardRank.Three, CardSuit.Spades);
        var playersWithThreeSpades = hands.Count(hand => hand.Contains(threeOfSpades));
        Assert.AreEqual(1, playersWithThreeSpades, "Exactly one player should have the 3♠");
    }
}
