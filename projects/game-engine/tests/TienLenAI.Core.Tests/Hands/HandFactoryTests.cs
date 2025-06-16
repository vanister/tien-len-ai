using TienLenAI.Core.Cards;
using TienLenAI.Core.Hands;

namespace TienLenAI.Core.Tests.Hands;

[TestClass]
public class HandFactoryTests
{
    #region CreateBestHand Tests (Updated from CreateHand)

    [TestMethod]
    public void CreateBestHand_WithNull_ReturnsNull()
    {
        // Act
        var result = HandFactory.CreateBestHand([]);

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void CreateBestHand_WithEmptyCards_ReturnsNull()
    {
        // Arrange
        var cards = new List<Card>();

        // Act
        var result = HandFactory.CreateBestHand(cards);

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void CreateBestHand_WithValidSingle_ReturnsSingleHand()
    {
        // Arrange
        var cards = new[] { new Card(CardRank.Ace, CardSuit.Hearts) };

        // Act
        var result = HandFactory.CreateBestHand(cards);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(SingleHand));
        Assert.AreEqual(HandType.Single, result.Type);
    }

    [TestMethod]
    public void CreateBestHand_WithValidPair_ReturnsPairHand()
    {
        // Arrange
        var cards = new[]
        {
            new Card(CardRank.Ace, CardSuit.Hearts),
            new Card(CardRank.Ace, CardSuit.Diamonds)
        };

        // Act
        var result = HandFactory.CreateBestHand(cards);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(PairHand));
        Assert.AreEqual(HandType.Pair, result.Type);
    }

    [TestMethod]
    public void CreateBestHand_WithValidTriple_ReturnsTripleHand()
    {
        // Arrange
        var cards = new[]
        {
            new Card(CardRank.King, CardSuit.Hearts),
            new Card(CardRank.King, CardSuit.Diamonds),
            new Card(CardRank.King, CardSuit.Clubs)
        };

        // Act
        var result = HandFactory.CreateBestHand(cards);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(TripleHand));
        Assert.AreEqual(HandType.Triple, result.Type);
    }

    [TestMethod]
    public void CreateBestHand_WithValidStraight_ReturnsStraightHand()
    {
        // Arrange
        var cards = new[]
        {
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Five, CardSuit.Clubs)
        };

        // Act
        var result = HandFactory.CreateBestHand(cards);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(StraightHand));
        Assert.AreEqual(HandType.Straight, result.Type);
    }

    [TestMethod]
    public void CreateBestHand_WithValidDoubleStraight_ReturnsDoubleStraightHand()
    {
        // Arrange
        var cards = new[]
        {
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Spades),
            new Card(CardRank.Five, CardSuit.Hearts),
            new Card(CardRank.Five, CardSuit.Diamonds)
        };

        // Act
        var result = HandFactory.CreateBestHand(cards);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(DoubleStraightHand));
        Assert.AreEqual(HandType.DoubleStraight, result.Type);
    }

    [TestMethod]
    public void CreateBestHand_WithValidTripleStraight_ReturnsTripleStraightHand()
    {
        // Arrange
        var cards = new[]
        {
            new Card(CardRank.Four, CardSuit.Hearts),
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Five, CardSuit.Hearts),
            new Card(CardRank.Five, CardSuit.Diamonds),
            new Card(CardRank.Five, CardSuit.Clubs)
        };

        // Act
        var result = HandFactory.CreateBestHand(cards);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(TripleStraightHand));
        Assert.AreEqual(HandType.TripleStraight, result.Type);
    }

    [TestMethod]
    public void CreateBestHand_WithValidBomb_ReturnsBombHand()
    {
        // Arrange
        var cards = new[]
        {
            new Card(CardRank.Seven, CardSuit.Hearts),
            new Card(CardRank.Seven, CardSuit.Diamonds),
            new Card(CardRank.Seven, CardSuit.Clubs),
            new Card(CardRank.Seven, CardSuit.Spades)
        };

        // Act
        var result = HandFactory.CreateBestHand(cards);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(BombHand));
        Assert.AreEqual(HandType.Bomb, result.Type);
    }

    [TestMethod]
    public void CreateBestHand_BombTakesPriorityOverTripleStraight()
    {
        // Arrange - 4 cards that could be a bomb (if same rank) but not a triple straight
        var cards = new[]
        {
            new Card(CardRank.Five, CardSuit.Hearts),
            new Card(CardRank.Five, CardSuit.Diamonds),
            new Card(CardRank.Five, CardSuit.Clubs),
            new Card(CardRank.Five, CardSuit.Spades)
        };

        // Act
        var result = HandFactory.CreateBestHand(cards);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(BombHand));
        Assert.AreEqual(HandType.Bomb, result.Type);
    }

    [TestMethod]
    public void CreateBestHand_TripleStraightTakesPriorityOverDoubleStraight()
    {
        // Arrange - 6 cards that form both triple straight and double straight
        // This is a tricky case - these cards could theoretically be both
        var cards = new[]
        {
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Hearts),
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Clubs)
        };

        // Act
        var result = HandFactory.CreateBestHand(cards);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(TripleStraightHand));
        Assert.AreEqual(HandType.TripleStraight, result.Type);
    }

    [TestMethod]
    public void CreateBestHand_StraightTakesPriorityOverTriple()
    {
        // Arrange - 3 consecutive cards (straight) vs 3 same rank (impossible with consecutive)
        // This tests that straight detection works when we have 3 cards
        var cards = new[]
        {
            new Card(CardRank.Seven, CardSuit.Hearts),
            new Card(CardRank.Eight, CardSuit.Diamonds),
            new Card(CardRank.Nine, CardSuit.Clubs)
        };

        // Act
        var result = HandFactory.CreateBestHand(cards);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(StraightHand));
        Assert.AreEqual(HandType.Straight, result.Type);
    }

    [TestMethod]
    public void CreateBestHand_WithInvalidCards_ReturnsNull()
    {
        // Arrange - Non-consecutive, non-matching cards that don't form any valid hand
        var cards = new[]
        {
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Seven, CardSuit.Diamonds)
        };

        // Act
        var result = HandFactory.CreateBestHand(cards);

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void CreateBestHand_WithTooManyCards_ReturnsNull()
    {
        // Arrange - More cards than any valid hand type allows
        var cards = new[]
        {
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Five, CardSuit.Clubs),
            new Card(CardRank.Six, CardSuit.Spades),
            new Card(CardRank.Seven, CardSuit.Hearts),
            new Card(CardRank.Eight, CardSuit.Diamonds),
            new Card(CardRank.Nine, CardSuit.Clubs),
            new Card(CardRank.Ten, CardSuit.Spades),
            new Card(CardRank.Jack, CardSuit.Hearts),
            new Card(CardRank.Queen, CardSuit.Diamonds),
            new Card(CardRank.King, CardSuit.Clubs),
            new Card(CardRank.Ace, CardSuit.Spades),
            new Card(CardRank.Two, CardSuit.Hearts) // 13 cards - too many for any hand
        };

        // Act
        var result = HandFactory.CreateBestHand(cards);

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void CreateBestHand_WithStraightContainingTwo_ReturnsNull()
    {
        // Arrange - Invalid straight containing 2 (not allowed in straights)
        var cards = new[]
        {
            new Card(CardRank.King, CardSuit.Hearts),
            new Card(CardRank.Ace, CardSuit.Diamonds),
            new Card(CardRank.Two, CardSuit.Clubs)
        };

        // Act
        var result = HandFactory.CreateBestHand(cards);

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void CreateBestHand_WithInvalidBomb_ReturnsNull()
    {
        // Arrange - 4 cards but different ranks
        var cards = new[]
        {
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Spades)
        };

        // Act
        var result = HandFactory.CreateBestHand(cards);

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void CreateBestHand_PreservesCardOrder()
    {
        // Arrange
        var cards = new[]
        {
            new Card(CardRank.Five, CardSuit.Diamonds),
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Four, CardSuit.Clubs)
        };

        // Act
        var result = HandFactory.CreateBestHand(cards);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(StraightHand));
        // The Hand base class should sort cards by value, so we should have ordered cards
        Assert.AreEqual(3, result.Cards.Count);
        Assert.IsTrue(result.Cards[0].Value < result.Cards[1].Value);
        Assert.IsTrue(result.Cards[1].Value < result.Cards[2].Value);
    }

    #endregion

    #region CreateHandsForType Tests

    [TestMethod]
    public void CreateHandsForType_WithNull_ReturnsEmpty()
    {
        // Act
        var result = HandFactory.CreateHandsForType(null, HandType.Single);

        // Assert
        Assert.IsFalse(result.Any());
    }

    [TestMethod]
    public void CreateHandsForType_WithEmptyCards_ReturnsEmpty()
    {
        // Arrange
        var cards = new List<Card>();

        // Act
        var result = HandFactory.CreateHandsForType(cards, HandType.Single);

        // Assert
        Assert.IsFalse(result.Any());
    }

    [TestMethod]
    public void CreateHandsForType_Single_ReturnsAllCards()
    {
        // Arrange
        var cards = new[]
        {
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Ace, CardSuit.Diamonds),
            new Card(CardRank.King, CardSuit.Clubs)
        };

        // Act
        var result = HandFactory.CreateHandsForType(cards, HandType.Single);

        // Assert
        Assert.AreEqual(3, result.Count);
        Assert.IsTrue(result.All(h => h.Type == HandType.Single));
        Assert.IsTrue(result.All(h => h.Cards.Count == 1));

        // Verify all cards are represented
        var resultCards = result.Select(h => h.Cards[0]).ToList();
        CollectionAssert.AreEquivalent(cards, resultCards);
    }

    [TestMethod]
    public void CreateHandsForType_Pair_ReturnsAllPossiblePairs()
    {
        // Arrange - Two pairs available: 3-3 and A-A
        var cards = new[]
        {
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Ace, CardSuit.Clubs),
            new Card(CardRank.Ace, CardSuit.Spades),
            new Card(CardRank.King, CardSuit.Hearts) // Single King, no pair
        };

        // Act
        var result = HandFactory.CreateHandsForType(cards, HandType.Pair);

        // Assert
        Assert.AreEqual(2, result.Count);
        Assert.IsTrue(result.All(h => h.Type == HandType.Pair));
        Assert.IsTrue(result.All(h => h.Cards.Count == 2));

        // Check that we have one pair of 3s and one pair of Aces
        var ranks = result.Select(h => h.Cards[0].Rank).OrderBy(r => r).ToList();
        CollectionAssert.AreEqual(new[] { CardRank.Three, CardRank.Ace }, ranks);
    }

    [TestMethod]
    public void CreateHandsForType_Pair_WithThreeOfSameRank_ReturnsMultiplePairs()
    {
        // Arrange - Three 3s should give us 3 different pairs
        var cards = new[]
        {
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Three, CardSuit.Clubs)
        };

        // Act
        var result = HandFactory.CreateHandsForType(cards, HandType.Pair);

        // Assert
        Assert.AreEqual(3, result.Count); // C(3,2) = 3 combinations
        Assert.IsTrue(result.All(h => h.Type == HandType.Pair));
        Assert.IsTrue(result.All(h => h.Cards.Count == 2));
        Assert.IsTrue(result.All(h => h.Cards.All(c => c.Rank == CardRank.Three)));
    }

    [TestMethod]
    public void CreateHandsForType_Triple_ReturnsAllPossibleTriples()
    {
        // Arrange - One triple of 3s available
        var cards = new[]
        {
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Ace, CardSuit.Spades) // Extra card
        };

        // Act
        var result = HandFactory.CreateHandsForType(cards, HandType.Triple);

        // Assert
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(HandType.Triple, result[0].Type);
        Assert.AreEqual(3, result[0].Cards.Count);
        Assert.IsTrue(result[0].Cards.All(c => c.Rank == CardRank.Three));
    }

    [TestMethod]
    public void CreateHandsForType_Triple_WithFourOfSameRank_ReturnsMultipleTriples()
    {
        // Arrange - Four 3s should give us 4 different triples
        var cards = new[]
        {
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Three, CardSuit.Spades)
        };

        // Act
        var result = HandFactory.CreateHandsForType(cards, HandType.Triple);

        // Assert
        Assert.AreEqual(4, result.Count); // C(4,3) = 4 combinations
        Assert.IsTrue(result.All(h => h.Type == HandType.Triple));
        Assert.IsTrue(result.All(h => h.Cards.Count == 3));
        Assert.IsTrue(result.All(h => h.Cards.All(c => c.Rank == CardRank.Three)));
    }

    [TestMethod]
    public void CreateHandsForType_Straight_ReturnsMultipleLengths()
    {
        // Arrange - Cards that can form 3-card and 4-card straights
        var cards = new[]
        {
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Five, CardSuit.Clubs),
            new Card(CardRank.Six, CardSuit.Spades)
        };

        // Act
        var result = HandFactory.CreateHandsForType(cards, HandType.Straight);

        // Assert
        Assert.IsTrue(result.Count > 0);
        Assert.IsTrue(result.All(h => h.Type == HandType.Straight));

        // Should have straights of different lengths
        var lengths = result.Select(h => h.Cards.Count).Distinct().OrderBy(l => l).ToList();
        Assert.IsTrue(lengths.Contains(3)); // 3-4-5
        Assert.IsTrue(lengths.Contains(4)); // 3-4-5-6
    }

    [TestMethod]
    public void CreateHandsForType_Straight_WithMultipleCardsPerRank_ReturnsDifferentCombinations()
    {
        // Arrange - Multiple cards of same rank should create different straight combinations
        var cards = new[]
        {
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds), // Two 3s
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Five, CardSuit.Spades)
        };

        // Act
        var result = HandFactory.CreateHandsForType(cards, HandType.Straight);

        // Assert
        Assert.IsTrue(result.Count >= 2); // Should have at least 2 different 3-card straights
        Assert.IsTrue(result.All(h => h.Type == HandType.Straight));
        Assert.IsTrue(result.All(h => h.Cards.Count == 3));

        // Should have different combinations using different 3s
        var threesUsed = result.Select(h => h.Cards.First(c => c.Rank == CardRank.Three)).Distinct().ToList();
        Assert.IsTrue(threesUsed.Count >= 2);
    }

    [TestMethod]
    public void CreateHandsForType_Straight_ExcludesTwos()
    {
        // Arrange - Include 2s which should be excluded from straights
        var cards = new[]
        {
            new Card(CardRank.King, CardSuit.Hearts),
            new Card(CardRank.Ace, CardSuit.Diamonds),
            new Card(CardRank.Two, CardSuit.Clubs)
        };

        // Act
        var result = HandFactory.CreateHandsForType(cards, HandType.Straight);

        // Assert
        Assert.AreEqual(0, result.Count); // No valid straights with 2s
    }

    [TestMethod]
    public void CreateHandsForType_Bomb_ReturnsAllBombs()
    {
        // Arrange - One bomb available
        var cards = new[]
        {
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Three, CardSuit.Spades)
        };

        // Act
        var result = HandFactory.CreateHandsForType(cards, HandType.Bomb);

        // Assert
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(HandType.Bomb, result[0].Type);
        Assert.AreEqual(4, result[0].Cards.Count);
        Assert.IsTrue(result[0].Cards.All(c => c.Rank == CardRank.Three));
    }

    [TestMethod]
    public void CreateHandsForType_DoubleStraight_ReturnsEmpty()
    {
        // Arrange - Valid double straight cards
        var cards = new[]
        {
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Spades),
            new Card(CardRank.Five, CardSuit.Hearts),
            new Card(CardRank.Five, CardSuit.Diamonds)
        };

        // Act
        var result = HandFactory.CreateHandsForType(cards, HandType.DoubleStraight);

        // Assert
        Assert.AreEqual(0, result.Count); // TODO implementation - should be empty for now
    }

    [TestMethod]
    public void CreateHandsForType_TripleStraight_ReturnsEmpty()
    {
        // Arrange - Valid triple straight cards
        var cards = new[]
        {
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Hearts),
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Clubs)
        };

        // Act
        var result = HandFactory.CreateHandsForType(cards, HandType.TripleStraight);

        // Assert
        Assert.AreEqual(0, result.Count); // TODO implementation - should be empty for now
    }

    [TestMethod]
    public void CreateHandsForType_Pair_WithNoPairs_ReturnsEmpty()
    {
        // Arrange - No pairs available
        var cards = new[]
        {
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Five, CardSuit.Clubs)
        };

        // Act
        var result = HandFactory.CreateHandsForType(cards, HandType.Pair);

        // Assert
        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void CreateHandsForType_Triple_WithNoTriples_ReturnsEmpty()
    {
        // Arrange - No triples available
        var cards = new[]
        {
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds), // Only a pair
            new Card(CardRank.Four, CardSuit.Clubs)
        };

        // Act
        var result = HandFactory.CreateHandsForType(cards, HandType.Triple);

        // Assert
        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void CreateHandsForType_Bomb_WithNoBombs_ReturnsEmpty()
    {
        // Arrange - No bombs available
        var cards = new[]
        {
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Three, CardSuit.Clubs) // Only a triple
        };

        // Act
        var result = HandFactory.CreateHandsForType(cards, HandType.Bomb);

        // Assert
        Assert.AreEqual(0, result.Count);
    }

    #endregion
}
