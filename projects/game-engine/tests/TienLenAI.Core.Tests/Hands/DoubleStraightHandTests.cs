using TienLenAI.Core.Cards;
using TienLenAI.Core.Hands;

namespace TienLenAI.Core.Tests.Hands;

[TestClass]
public class DoubleStraightHandTests
{
    [TestMethod]
    public void IsValid_WithValidMinimumDoubleStraight_ReturnsTrue()
    {
        // Arrange - Three pairs: 3-4-5
        Card[] cards = [
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Spades),
            new Card(CardRank.Five, CardSuit.Hearts),
            new Card(CardRank.Five, CardSuit.Diamonds)
        ];
        var hand = new DoubleStraightHand(cards);

        // Act
        var isValid = hand.IsValid();

        // Assert
        Assert.IsTrue(isValid);
    }

    [TestMethod]
    public void IsValid_WithValidLargeDoubleStraight_ReturnsTrue()
    {
        // Arrange - Five pairs: 3-4-5-6-7
        Card[] cards = [
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Spades),
            new Card(CardRank.Five, CardSuit.Hearts),
            new Card(CardRank.Five, CardSuit.Diamonds),
            new Card(CardRank.Six, CardSuit.Clubs),
            new Card(CardRank.Six, CardSuit.Spades),
            new Card(CardRank.Seven, CardSuit.Hearts),
            new Card(CardRank.Seven, CardSuit.Diamonds)
        ];
        var hand = new DoubleStraightHand(cards);

        // Act
        var isValid = hand.IsValid();

        // Assert
        Assert.IsTrue(isValid);
    }

    [TestMethod]
    public void IsValid_WithFourCards_ReturnsFalse()
    {
        // Arrange - Two pairs (too few)
        Card[] cards = [
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Spades)
        ];
        var hand = new DoubleStraightHand(cards);

        // Act
        var isValid = hand.IsValid();

        // Assert
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void IsValid_WithOddNumberOfCards_ReturnsFalse()
    {
        // Arrange - 7 cards (odd number)
        Card[] cards = [
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Spades),
            new Card(CardRank.Five, CardSuit.Hearts),
            new Card(CardRank.Five, CardSuit.Diamonds),
            new Card(CardRank.Six, CardSuit.Clubs)
        ];
        var hand = new DoubleStraightHand(cards);

        // Act
        var isValid = hand.IsValid();

        // Assert
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void IsValid_WithInvalidCardCount_ReturnsFalse()
    {
        // Arrange - Three of one rank (invalid)
        Card[] cards = [
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Spades),
            new Card(CardRank.Four, CardSuit.Hearts),
            new Card(CardRank.Five, CardSuit.Diamonds)
        ];
        var hand = new DoubleStraightHand(cards);

        // Act
        var isValid = hand.IsValid();

        // Assert
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void IsValid_WithNonConsecutiveRanks_ReturnsFalse()
    {
        // Arrange - Missing Four
        Card[] cards = [
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Five, CardSuit.Clubs),
            new Card(CardRank.Five, CardSuit.Spades),
            new Card(CardRank.Six, CardSuit.Hearts),
            new Card(CardRank.Six, CardSuit.Diamonds)
        ];
        var hand = new DoubleStraightHand(cards);

        // Act
        var isValid = hand.IsValid();

        // Assert
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void IsValid_WithTwo_ReturnsFalse()
    {
        // Arrange - Contains Two
        Card[] cards = [
            new Card(CardRank.King, CardSuit.Hearts),
            new Card(CardRank.King, CardSuit.Diamonds),
            new Card(CardRank.Ace, CardSuit.Clubs),
            new Card(CardRank.Ace, CardSuit.Spades),
            new Card(CardRank.Two, CardSuit.Hearts),
            new Card(CardRank.Two, CardSuit.Diamonds)
        ];
        var hand = new DoubleStraightHand(cards);

        // Act
        var isValid = hand.IsValid();

        // Assert
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void CompareTo_HigherDoubleStraightWins()
    {
        // Arrange
        var lowerHand = new DoubleStraightHand([
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Spades),
            new Card(CardRank.Five, CardSuit.Hearts),
            new Card(CardRank.Five, CardSuit.Diamonds)
        ]);
        var higherHand = new DoubleStraightHand([
            new Card(CardRank.Four, CardSuit.Hearts),
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Five, CardSuit.Clubs),
            new Card(CardRank.Five, CardSuit.Spades),
            new Card(CardRank.Six, CardSuit.Hearts),
            new Card(CardRank.Six, CardSuit.Diamonds)
        ]);

        // Act & Assert
        Assert.IsTrue(higherHand.CompareTo(lowerHand) > 0);
        Assert.IsTrue(lowerHand.CompareTo(higherHand) < 0);
    }

    [TestMethod]
    public void CompareTo_SameRankHigherSuitWins()
    {
        // Arrange
        var lowerHand = new DoubleStraightHand([
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Spades),
            new Card(CardRank.Five, CardSuit.Clubs),
            new Card(CardRank.Five, CardSuit.Diamonds)
        ]);
        var higherHand = new DoubleStraightHand([
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Spades),
            new Card(CardRank.Five, CardSuit.Diamonds),
            new Card(CardRank.Five, CardSuit.Hearts)
        ]);

        // Act & Assert
        Assert.IsTrue(higherHand.CompareTo(lowerHand) > 0);
        Assert.IsTrue(lowerHand.CompareTo(higherHand) < 0);
    }

    [TestMethod]
    public void CompareTo_WithNull_ReturnsPositive()
    {
        // Arrange
        var hand = new DoubleStraightHand([
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Spades),
            new Card(CardRank.Five, CardSuit.Hearts),
            new Card(CardRank.Five, CardSuit.Diamonds)
        ]);

        // Act & Assert
        Assert.IsTrue(hand.CompareTo(null) > 0);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CompareTo_WithDifferentHandType_ThrowsException()
    {
        // Arrange
        var doubleStraight = new DoubleStraightHand([
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Spades),
            new Card(CardRank.Five, CardSuit.Hearts),
            new Card(CardRank.Five, CardSuit.Diamonds)
        ]);
        var single = new SingleHand(new Card(CardRank.Ace, CardSuit.Hearts));

        // Act - should throw
        doubleStraight.CompareTo(single);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CompareTo_WithDifferentLength_ThrowsException()
    {
        // Arrange - 3 pairs vs 4 pairs
        var shorter = new DoubleStraightHand([
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Spades),
            new Card(CardRank.Five, CardSuit.Hearts),
            new Card(CardRank.Five, CardSuit.Diamonds)
        ]);
        var longer = new DoubleStraightHand([
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Spades),
            new Card(CardRank.Five, CardSuit.Hearts),
            new Card(CardRank.Five, CardSuit.Diamonds),
            new Card(CardRank.Six, CardSuit.Hearts),
            new Card(CardRank.Six, CardSuit.Diamonds)
        ]);

        // Act - should throw
        shorter.CompareTo(longer);
    }

    [TestMethod]
    public void CompareTo_WithBombHand_ReturnsNegative()
    {
        // Arrange
        var doubleStraight = new DoubleStraightHand([
            new Card(CardRank.King, CardSuit.Hearts),
            new Card(CardRank.King, CardSuit.Diamonds),
            new Card(CardRank.Queen, CardSuit.Clubs),
            new Card(CardRank.Queen, CardSuit.Spades),
            new Card(CardRank.Jack, CardSuit.Hearts),
            new Card(CardRank.Jack, CardSuit.Diamonds)
        ]);
        var bomb = new BombHand([
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Three, CardSuit.Spades)
        ]);

        // Act & Assert
        Assert.IsTrue(doubleStraight.CompareTo(bomb) < 0);
    }
}
