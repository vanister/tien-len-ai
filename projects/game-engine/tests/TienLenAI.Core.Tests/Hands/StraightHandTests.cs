using TienLenAI.Core.Cards;
using TienLenAI.Core.Hands;

namespace TienLenAI.Core.Tests.Hands;

[TestClass]
public class StraightHandTests
{
    [TestMethod]
    public void IsValid_WithValidThreeCardStraight_ReturnsTrue()
    {
        // Arrange
        Card[] cards = [
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Five, CardSuit.Clubs)
        ];
        var hand = new StraightHand(cards);

        // Act
        var isValid = hand.IsValid();

        // Assert
        Assert.IsTrue(isValid);
    }

    [TestMethod]
    public void IsValid_WithValidMaxStraight_ReturnsTrue()
    {
        // Arrange - 3 through Ace (12 cards)
        Card[] cards = [
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
            new Card(CardRank.Ace, CardSuit.Spades)
        ];
        var hand = new StraightHand(cards);

        // Act
        var isValid = hand.IsValid();

        // Assert
        Assert.IsTrue(isValid);
    }

    [TestMethod]
    public void IsValid_WithTwoCards_ReturnsFalse()
    {
        // Arrange
        Card[] cards = [
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Four, CardSuit.Diamonds)
        ];
        var hand = new StraightHand(cards);

        // Act
        var isValid = hand.IsValid();

        // Assert
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void IsValid_WithThirteenCards_ReturnsFalse()
    {
        // Arrange - 3 through 2 (13 cards)
        Card[] cards = [
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
            new Card(CardRank.Two, CardSuit.Hearts)
        ];
        var hand = new StraightHand(cards);

        // Act
        var isValid = hand.IsValid();

        // Assert
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void IsValid_WithTwo_ReturnsFalse()
    {
        // Arrange
        Card[] cards = [
            new Card(CardRank.King, CardSuit.Hearts),
            new Card(CardRank.Ace, CardSuit.Diamonds),
            new Card(CardRank.Two, CardSuit.Clubs)
        ];
        var hand = new StraightHand(cards);

        // Act
        var isValid = hand.IsValid();

        // Assert
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void IsValid_WithNonConsecutiveCards_ReturnsFalse()
    {
        // Arrange
        Card[] cards = [
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Six, CardSuit.Clubs)  // Skip Five
        ];
        var hand = new StraightHand(cards);

        // Act
        var isValid = hand.IsValid();

        // Assert
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void CompareTo_HigherStraightWins()
    {
        // Arrange
        var lowerStraight = new StraightHand([
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Five, CardSuit.Clubs)
        ]);
        var higherStraight = new StraightHand([
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Five, CardSuit.Spades),
            new Card(CardRank.Six, CardSuit.Hearts)
        ]);

        // Act & Assert
        Assert.IsTrue(higherStraight.CompareTo(lowerStraight) > 0);
        Assert.IsTrue(lowerStraight.CompareTo(higherStraight) < 0);
    }

    [TestMethod]
    public void CompareTo_SameRankHigherSuitWins()
    {
        // Arrange
        var lowerStraight = new StraightHand([
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Five, CardSuit.Clubs)
        ]);
        var higherStraight = new StraightHand([
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Spades),
            new Card(CardRank.Five, CardSuit.Hearts)
        ]);

        // Act & Assert
        Assert.IsTrue(higherStraight.CompareTo(lowerStraight) > 0);
        Assert.IsTrue(lowerStraight.CompareTo(higherStraight) < 0);
    }

    [TestMethod]
    public void CompareTo_WithBombHand_ReturnsNegative()
    {
        // Arrange
        var straight = new StraightHand([
            new Card(CardRank.King, CardSuit.Hearts),
            new Card(CardRank.Queen, CardSuit.Diamonds),
            new Card(CardRank.Jack, CardSuit.Clubs)
        ]);
        var bomb = new BombHand([
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Three, CardSuit.Spades)
        ]);

        // Act & Assert
        Assert.IsTrue(straight.CompareTo(bomb) < 0);
    }

    [TestMethod]
    public void CompareTo_WithNull_ReturnsPositive()
    {
        // Arrange
        var straight = new StraightHand([
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Five, CardSuit.Clubs)
        ]);

        // Act & Assert
        Assert.IsTrue(straight.CompareTo(null) > 0);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CompareTo_WithDifferentHandType_ThrowsException()
    {
        // Arrange
        var straight = new StraightHand([
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Five, CardSuit.Clubs)
        ]);
        var single = new SingleHand(new Card(CardRank.Ace, CardSuit.Hearts));

        // Act - should throw
        straight.CompareTo(single);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CompareTo_WithDifferentLengthStraight_ThrowsException()
    {
        // Arrange
        var threeCardStraight = new StraightHand([
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Five, CardSuit.Clubs)
        ]);
        var fourCardStraight = new StraightHand([
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Spades),
            new Card(CardRank.Five, CardSuit.Hearts),
            new Card(CardRank.Six, CardSuit.Diamonds)
        ]);

        // Act - should throw
        threeCardStraight.CompareTo(fourCardStraight);
    }
}
