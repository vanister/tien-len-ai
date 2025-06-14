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
    public void IsValid_WithSameSuitTriples_ReturnsFalse()
    {
        // Arrange
        Card[] cards = [
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Five, CardSuit.Diamonds),
            new Card(CardRank.Five, CardSuit.Diamonds),
            new Card(CardRank.Five, CardSuit.Diamonds),
        ];
        var hand = new TripleStraightHand(cards);

        // Act
        var isValid = hand.IsValid();

        // Assert
        Assert.IsFalse(isValid);
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
}
