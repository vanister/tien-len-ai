using TienLenAI.Core.Cards;
using TienLenAI.Core.Hands;

namespace TienLenAI.Core.Tests.Hands;

[TestClass]
public class TripleStraightHandTests
{
    [TestMethod]
    public void Constructor_WithValidCards_CreatesHand()
    {
        // Arrange
        Card[] cards = [
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Hearts),
            new Card(CardRank.Five, CardSuit.Clubs),
            new Card(CardRank.Five, CardSuit.Diamonds),
            new Card(CardRank.Five, CardSuit.Hearts),
        ];

        // Act
        var hand = new TripleStraightHand(cards);

        // Assert
        Assert.IsNotNull(hand);
        Assert.AreEqual(6, hand.Cards.Count);
        Assert.AreEqual(HandType.TripleStraight, hand.Type);
    }

    [TestMethod]
    public void IsValid_WithValidCards_ReturnsTrue()
    {
        // Arrange
        Card[] cards = [
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Hearts),
            new Card(CardRank.Five, CardSuit.Clubs),
            new Card(CardRank.Five, CardSuit.Diamonds),
            new Card(CardRank.Five, CardSuit.Hearts),
        ];
        var hand = new TripleStraightHand(cards);

        // Act
        var isValid = hand.IsValid();

        // Assert
        Assert.IsTrue(isValid);
    }

    [TestMethod]
    public void IsValid_WithThreeConsecutiveTriples_ReturnsTrue()
    {
        // Arrange
        Card[] cards = [
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Hearts),
            new Card(CardRank.Five, CardSuit.Clubs),
            new Card(CardRank.Five, CardSuit.Diamonds),
            new Card(CardRank.Five, CardSuit.Hearts),
            new Card(CardRank.Six, CardSuit.Clubs),
            new Card(CardRank.Six, CardSuit.Diamonds),
            new Card(CardRank.Six, CardSuit.Hearts),
        ];
        var hand = new TripleStraightHand(cards);

        // Act
        var isValid = hand.IsValid();

        // Assert
        Assert.IsTrue(isValid);
    }

    [TestMethod]
    public void IsValid_WithNonConsecutiveTriples_ReturnsFalse()
    {
        // Arrange
        Card[] cards = [
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Hearts),
            new Card(CardRank.Six, CardSuit.Clubs),
            new Card(CardRank.Six, CardSuit.Diamonds),
            new Card(CardRank.Six, CardSuit.Hearts),
        ];
        var hand = new TripleStraightHand(cards);

        // Act
        var isValid = hand.IsValid();

        // Assert
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void IsValid_WithIncompleteTriples_ReturnsFalse()
    {
        // Arrange
        Card[] cards = [
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Hearts),
            new Card(CardRank.Five, CardSuit.Clubs),
            new Card(CardRank.Five, CardSuit.Diamonds),
        ];
        var hand = new TripleStraightHand(cards);

        // Act
        var isValid = hand.IsValid();

        // Assert
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void IsValid_WithInvalidNumberOfCards_ReturnsFalse()
    {
        // Arrange
        Card[] cards = [
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Hearts),
            new Card(CardRank.Five, CardSuit.Clubs),
            new Card(CardRank.Five, CardSuit.Diamonds),
            new Card(CardRank.Five, CardSuit.Hearts),
            new Card(CardRank.Six, CardSuit.Clubs),
        ];
        var hand = new TripleStraightHand(cards);

        // Act
        var isValid = hand.IsValid();

        // Assert
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void IsValid_WithSameRankCards_ReturnsFalse()
    {
        // Arrange
        Card[] cards = [
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Hearts),
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Hearts),
        ];
        var hand = new TripleStraightHand(cards);

        // Act
        var isValid = hand.IsValid();

        // Assert
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void IsValid_WithSameSuitTriples_ReturnsTrue()
    {
        // Arrange - This should now be VALID since suits don't matter
        Card[] cards = [
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Clubs),    // Same suit - now allowed
            new Card(CardRank.Four, CardSuit.Clubs),    // Same suit - now allowed
            new Card(CardRank.Five, CardSuit.Diamonds),
            new Card(CardRank.Five, CardSuit.Diamonds), // Same suit - now allowed
            new Card(CardRank.Five, CardSuit.Diamonds), // Same suit - now allowed
        ];

        var hand = new TripleStraightHand(cards);

        // Act
        var isValid = hand.IsValid();

        // Assert
        Assert.IsTrue(isValid); // CHANGED: was Assert.IsFalse
    }

    [TestMethod]
    public void CompareTo_WithSameLength_ReturnsExpectedResult()
    {
        // Arrange
        var lowerHand = new TripleStraightHand([
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Hearts),
        ]);

        var higherHand = new TripleStraightHand([
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Hearts),
            new Card(CardRank.Five, CardSuit.Clubs),
            new Card(CardRank.Five, CardSuit.Diamonds),
            new Card(CardRank.Five, CardSuit.Hearts),
        ]);

        // Act & Assert
        Assert.IsTrue(higherHand.CompareTo(lowerHand) > 0);
        Assert.IsTrue(lowerHand.CompareTo(higherHand) < 0);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CompareTo_WithDifferentLength_ThrowsException()
    {
        // Arrange
        var sixCardHand = new TripleStraightHand([
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Hearts),
            new Card(CardRank.Five, CardSuit.Clubs),
            new Card(CardRank.Five, CardSuit.Diamonds),
            new Card(CardRank.Five, CardSuit.Hearts),
        ]);

        var nineCardHand = new TripleStraightHand([
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Hearts),
            new Card(CardRank.Five, CardSuit.Clubs),
            new Card(CardRank.Five, CardSuit.Diamonds),
            new Card(CardRank.Five, CardSuit.Hearts),
            new Card(CardRank.Six, CardSuit.Clubs),
            new Card(CardRank.Six, CardSuit.Diamonds),
            new Card(CardRank.Six, CardSuit.Hearts),
        ]);

        // Act
        sixCardHand.CompareTo(nineCardHand);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CompareTo_WithDifferentHandType_ThrowsException()
    {
        // Arrange
        var tripleStraight = new TripleStraightHand([
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Hearts),
            new Card(CardRank.Five, CardSuit.Clubs),
            new Card(CardRank.Five, CardSuit.Diamonds),
            new Card(CardRank.Five, CardSuit.Hearts),
        ]);

        var straight = new StraightHand([
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Five, CardSuit.Diamonds),
            new Card(CardRank.Six, CardSuit.Hearts),
            new Card(CardRank.Seven, CardSuit.Clubs),
            new Card(CardRank.Eight, CardSuit.Diamonds),
        ]);

        // Act
        tripleStraight.CompareTo(straight);
    }

    [TestMethod]
    public void CompareTo_WithBombHand_ReturnsNegative()
    {
        // Arrange
        var tripleStraight = new TripleStraightHand([
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Hearts),
            new Card(CardRank.Five, CardSuit.Clubs),
            new Card(CardRank.Five, CardSuit.Diamonds),
            new Card(CardRank.Five, CardSuit.Hearts),
        ]);

        var bomb = new BombHand([
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Spades),
        ]);

        // Act & Assert
        Assert.IsTrue(tripleStraight.CompareTo(bomb) < 0);
    }

    [TestMethod]
    public void CompareTo_WithEqualValue_ReturnsZero()
    {
        // Arrange
        var hand1 = new TripleStraightHand([
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Hearts),
            new Card(CardRank.Five, CardSuit.Clubs),
            new Card(CardRank.Five, CardSuit.Diamonds),
            new Card(CardRank.Five, CardSuit.Hearts),
        ]);

        var hand2 = new TripleStraightHand([
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Hearts),
            new Card(CardRank.Five, CardSuit.Clubs),
            new Card(CardRank.Five, CardSuit.Diamonds),
            new Card(CardRank.Five, CardSuit.Hearts),
        ]);

        // Act
        var comparison = hand1.CompareTo(hand2);

        // Assert
        Assert.AreEqual(0, comparison);
    }

    [TestMethod]
    public void CompareTo_WithNullHand_ReturnsPositive()
    {
        // Arrange
        var hand = new TripleStraightHand([
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Hearts),
            new Card(CardRank.Five, CardSuit.Clubs),
            new Card(CardRank.Five, CardSuit.Diamonds),
            new Card(CardRank.Five, CardSuit.Hearts),
        ]);

        // Act
        var comparison = hand.CompareTo(null);

        // Assert
        Assert.IsTrue(comparison > 0);
    }

    [TestMethod]
    public void IsValid_WithMaximumStraight_ReturnsTrue()
    {
        // Arrange
        Card[] cards = [
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Hearts),
            new Card(CardRank.Five, CardSuit.Clubs),
            new Card(CardRank.Five, CardSuit.Diamonds),
            new Card(CardRank.Five, CardSuit.Hearts),
            new Card(CardRank.Six, CardSuit.Clubs),
            new Card(CardRank.Six, CardSuit.Diamonds),
            new Card(CardRank.Six, CardSuit.Hearts),
        ];
        var hand = new TripleStraightHand(cards);

        // Act
        var isValid = hand.IsValid();

        // Assert
        Assert.IsTrue(isValid);
    }

    [TestMethod]
    public void IsValid_ValidTwoConsecutiveTriples_ReturnsTrue()
    {
        // Arrange: 333-444
        var cards = new[]
        {
            new Card(CardRank.Three, CardSuit.Spades),
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Spades),
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Hearts)
        };

        // Act
        var hand = new TripleStraightHand(cards);

        // Assert
        Assert.IsTrue(hand.IsValid());
    }

    [TestMethod]
    public void IsValid_ValidThreeConsecutiveTriples_ReturnsTrue()
    {
        // Arrange: 777-888-999
        var cards = new[]
        {
            new Card(CardRank.Seven, CardSuit.Spades),
            new Card(CardRank.Seven, CardSuit.Clubs),
            new Card(CardRank.Seven, CardSuit.Diamonds),
            new Card(CardRank.Eight, CardSuit.Spades),
            new Card(CardRank.Eight, CardSuit.Clubs),
            new Card(CardRank.Eight, CardSuit.Hearts),
            new Card(CardRank.Nine, CardSuit.Spades),
            new Card(CardRank.Nine, CardSuit.Clubs),
            new Card(CardRank.Nine, CardSuit.Diamonds)
        };

        // Act
        var hand = new TripleStraightHand(cards);

        // Assert
        Assert.IsTrue(hand.IsValid());
    }

    [TestMethod]
    public void IsValid_TooFewCards_ReturnsFalse()
    {
        // Arrange: Only 5 cards (need at least 6)
        var cards = new[]
        {
            new Card(CardRank.Three, CardSuit.Spades),
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Spades),
            new Card(CardRank.Four, CardSuit.Clubs)
        };

        // Act
        var hand = new TripleStraightHand(cards);

        // Assert
        Assert.IsFalse(hand.IsValid());
    }

    [TestMethod]
    public void IsValid_CardCountNotDivisibleByThree_ReturnsFalse()
    {
        // Arrange: 7 cards (not divisible by 3)
        var cards = new[]
        {
            new Card(CardRank.Three, CardSuit.Spades),
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Spades),
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Hearts),
            new Card(CardRank.Five, CardSuit.Spades)
        };

        // Act
        var hand = new TripleStraightHand(cards);

        // Assert
        Assert.IsFalse(hand.IsValid());
    }

    [TestMethod]
    public void IsValid_RankAppearsOnlyTwice_ReturnsFalse()
    {
        // Arrange: 333-44 (four only appears twice)
        var cards = new[]
        {
            new Card(CardRank.Three, CardSuit.Spades),
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Spades),
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Five, CardSuit.Hearts)
        };

        // Act
        var hand = new TripleStraightHand(cards);

        // Assert
        Assert.IsFalse(hand.IsValid());
    }

    [TestMethod]
    public void IsValid_RankAppearsFourTimes_ReturnsFalse()
    {
        // Arrange: 3333-444 (three appears 4 times)
        var cards = new[]
        {
            new Card(CardRank.Three, CardSuit.Spades),
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Four, CardSuit.Spades),
            new Card(CardRank.Four, CardSuit.Clubs)
        };

        // Act
        var hand = new TripleStraightHand(cards);

        // Assert
        Assert.IsFalse(hand.IsValid());
    }

    [TestMethod]
    public void IsValid_NonConsecutiveRanks_ReturnsFalse()
    {
        // Arrange: 333-555 (skipping 4)
        var cards = new[]
        {
            new Card(CardRank.Three, CardSuit.Spades),
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Five, CardSuit.Spades),
            new Card(CardRank.Five, CardSuit.Clubs),
            new Card(CardRank.Five, CardSuit.Hearts)
        };

        // Act
        var hand = new TripleStraightHand(cards);

        // Assert
        Assert.IsFalse(hand.IsValid());
    }

    [TestMethod]
    public void IsValid_ContainsTwos_ReturnsFalse()
    {
        // Arrange: AAA-222 (contains 2s)
        var cards = new[]
        {
            new Card(CardRank.Ace, CardSuit.Spades),
            new Card(CardRank.Ace, CardSuit.Clubs),
            new Card(CardRank.Ace, CardSuit.Diamonds),
            new Card(CardRank.Two, CardSuit.Spades),
            new Card(CardRank.Two, CardSuit.Clubs),
            new Card(CardRank.Two, CardSuit.Hearts)
        };

        // Act
        var hand = new TripleStraightHand(cards);

        // Assert
        Assert.IsFalse(hand.IsValid());
    }

    [TestMethod]
    public void IsValid_ValidWithMixedSuits_ReturnsTrue()
    {
        // Arrange: Triple straight where triples have repeated suits
        var cards = new[]
        {
            new Card(CardRank.Five, CardSuit.Spades),
            new Card(CardRank.Five, CardSuit.Spades), // Same suit as above
            new Card(CardRank.Five, CardSuit.Clubs),
            new Card(CardRank.Six, CardSuit.Hearts),
            new Card(CardRank.Six, CardSuit.Hearts), // Same suit as above
            new Card(CardRank.Six, CardSuit.Diamonds)
        };

        // Act
        var hand = new TripleStraightHand(cards);

        // Assert
        Assert.IsTrue(hand.IsValid());
    }

    [TestMethod]
    public void IsValid_ValidLongTripleStraight_ReturnsTrue()
    {
        // Arrange: 3-4-5-6-7 triple straight (15 cards)
        var cards = new[]
        {
            // Threes
            new Card(CardRank.Three, CardSuit.Spades),
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Three, CardSuit.Diamonds),
            // Fours
            new Card(CardRank.Four, CardSuit.Spades),
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Hearts),
            // Fives
            new Card(CardRank.Five, CardSuit.Spades),
            new Card(CardRank.Five, CardSuit.Clubs),
            new Card(CardRank.Five, CardSuit.Diamonds),
            // Sixes
            new Card(CardRank.Six, CardSuit.Spades),
            new Card(CardRank.Six, CardSuit.Clubs),
            new Card(CardRank.Six, CardSuit.Hearts),
            // Sevens
            new Card(CardRank.Seven, CardSuit.Spades),
            new Card(CardRank.Seven, CardSuit.Clubs),
            new Card(CardRank.Seven, CardSuit.Diamonds)
        };

        // Act
        var hand = new TripleStraightHand(cards);

        // Assert
        Assert.IsTrue(hand.IsValid());
    }

    // NEW TEST to add - covers the 2's exclusion rule
    [TestMethod]
    public void IsValid_WithTwos_ReturnsFalse()
    {
        // Arrange - Contains 2s (not allowed in straights)
        Card[] cards = [
            new Card(CardRank.Ace, CardSuit.Spades),
            new Card(CardRank.Ace, CardSuit.Clubs),
            new Card(CardRank.Ace, CardSuit.Diamonds),
            new Card(CardRank.Two, CardSuit.Spades),
            new Card(CardRank.Two, CardSuit.Clubs),
            new Card(CardRank.Two, CardSuit.Hearts)
        ];
        var hand = new TripleStraightHand(cards);

        // Act
        var isValid = hand.IsValid();

        // Assert
        Assert.IsFalse(isValid);
    }

    // NEW TEST to add - verifies mixed suits are allowed
    [TestMethod]
    public void IsValid_WithMixedSuits_ReturnsTrue()
    {
        // Arrange - Some suits repeated, some different (all should be valid)
        Card[] cards = [
            new Card(CardRank.Five, CardSuit.Spades),
            new Card(CardRank.Five, CardSuit.Spades),    // Same suit repeated
            new Card(CardRank.Five, CardSuit.Clubs),
            new Card(CardRank.Six, CardSuit.Hearts),
            new Card(CardRank.Six, CardSuit.Hearts),     // Same suit repeated
            new Card(CardRank.Six, CardSuit.Diamonds)
        ];
        var hand = new TripleStraightHand(cards);

        // Act
        var isValid = hand.IsValid();

        // Assert
        Assert.IsTrue(isValid);
    }
}
