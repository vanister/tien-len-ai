using TienLenAI.Core.Cards;
using TienLenAI.Core.Hands;

namespace TienLenAI.Core.Tests.Hands;

[TestClass]
public class BombHandTests
{
    [TestMethod]
    public void IsValid_WithValidBomb_ReturnsTrue()
    {
        // Arrange
        Card[] cards = [
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Three, CardSuit.Spades)
        ];
        var hand = new BombHand(cards);

        // Act
        var isValid = hand.IsValid();

        // Assert
        Assert.IsTrue(isValid);
    }

    [TestMethod]
    public void IsValid_WithDifferentRanks_ReturnsFalse()
    {
        // Arrange
        Card[] cards = [
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Spades)
        ];
        var hand = new BombHand(cards);

        // Act
        var isValid = hand.IsValid();

        // Assert
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void IsValid_WithThreeCards_ReturnsFalse()
    {
        // Arrange
        Card[] cards = [
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Three, CardSuit.Clubs)
        ];
        var hand = new BombHand(cards);

        // Act
        var isValid = hand.IsValid();

        // Assert
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void CompareTo_HigherBombWins()
    {
        // Arrange
        var lowerBomb = new BombHand([
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Three, CardSuit.Spades)
        ]);
        var higherBomb = new BombHand([
            new Card(CardRank.Four, CardSuit.Hearts),
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Spades)
        ]);

        // Act & Assert
        Assert.IsTrue(higherBomb.CompareTo(lowerBomb) > 0);
        Assert.IsTrue(lowerBomb.CompareTo(higherBomb) < 0);
    }

    [TestMethod]
    public void CompareTo_WithSingleHand_ReturnsPositive()
    {
        // Arrange
        var bomb = new BombHand([
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Three, CardSuit.Spades)
        ]);
        var single = new SingleHand(new Card(CardRank.Two, CardSuit.Hearts));

        // Act & Assert
        Assert.IsTrue(bomb.CompareTo(single) > 0);
    }

    [TestMethod]
    public void CompareTo_WithPairHand_ReturnsPositive()
    {
        // Arrange
        var bomb = new BombHand([
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Three, CardSuit.Spades)
        ]);
        var pair = new PairHand([
            new Card(CardRank.Two, CardSuit.Hearts),
            new Card(CardRank.Two, CardSuit.Diamonds)
        ]);

        // Act & Assert
        Assert.IsTrue(bomb.CompareTo(pair) > 0);
    }

    [TestMethod]
    public void CompareTo_WithTripleHand_ReturnsPositive()
    {
        // Arrange
        var bomb = new BombHand([
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Three, CardSuit.Spades)
        ]);
        var triple = new TripleHand([
            new Card(CardRank.Two, CardSuit.Hearts),
            new Card(CardRank.Two, CardSuit.Diamonds),
            new Card(CardRank.Two, CardSuit.Clubs)
        ]);

        // Act & Assert
        Assert.IsTrue(bomb.CompareTo(triple) > 0);
    }

    [TestMethod]
    public void CompareTo_WithStraightHand_ReturnsPositive()
    {
        // Arrange
        var bomb = new BombHand([
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Three, CardSuit.Spades)
        ]);
        var straight = new StraightHand([
            new Card(CardRank.King, CardSuit.Hearts),
            new Card(CardRank.Ace, CardSuit.Diamonds),
            new Card(CardRank.Queen, CardSuit.Clubs)
        ]);

        // Act & Assert
        Assert.IsTrue(bomb.CompareTo(straight) > 0);
    }

    [TestMethod]
    public void CompareTo_WithDoubleStraightHand_ReturnsPositive()
    {
        // Arrange
        var bomb = new BombHand([
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Three, CardSuit.Spades)
        ]);
        var doubleStraight = new DoubleStraightHand([
            new Card(CardRank.King, CardSuit.Hearts),
            new Card(CardRank.King, CardSuit.Diamonds),
            new Card(CardRank.Queen, CardSuit.Clubs),
            new Card(CardRank.Queen, CardSuit.Spades),
            new Card(CardRank.Jack, CardSuit.Hearts),
            new Card(CardRank.Jack, CardSuit.Diamonds)
        ]);

        // Act & Assert
        Assert.IsTrue(bomb.CompareTo(doubleStraight) > 0);
    }

    [TestMethod]
    public void CompareTo_WithNull_ReturnsPositive()
    {
        // Arrange
        var bomb = new BombHand([
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Three, CardSuit.Spades)
        ]);

        // Act & Assert
        Assert.IsTrue(bomb.CompareTo(null) > 0);
    }
}
