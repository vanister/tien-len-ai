using TienLenAI.Core.Cards;
using TienLenAI.Core.Hands;

namespace TienLenAI.Core.Tests.Hands;

[TestClass]
public class PairHandTests
{
    [TestMethod]
    public void IsValid_WithValidPair_ReturnsTrue()
    {
        // Arrange
        Card[] cards = [
            new Card(CardRank.Ace, CardSuit.Hearts),
            new Card(CardRank.Ace, CardSuit.Diamonds)
        ];
        var hand = new PairHand(cards);

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
            new Card(CardRank.Ace, CardSuit.Hearts),
            new Card(CardRank.King, CardSuit.Diamonds)
        ];
        var hand = new PairHand(cards);

        // Act
        var isValid = hand.IsValid();

        // Assert
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void IsValid_WithSingleCard_ReturnsFalse()
    {
        // Arrange
        Card[] cards = [
            new Card(CardRank.Ace, CardSuit.Hearts)
        ];
        var hand = new PairHand(cards);

        // Act
        var isValid = hand.IsValid();

        // Assert
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void CompareTo_HigherPairWins()
    {
        // Arrange
        var lowerPair = new PairHand([
            new Card(CardRank.King, CardSuit.Hearts),
            new Card(CardRank.King, CardSuit.Diamonds)
        ]);
        var higherPair = new PairHand([
            new Card(CardRank.Ace, CardSuit.Clubs),
            new Card(CardRank.Ace, CardSuit.Spades)
        ]);

        // Act & Assert
        Assert.IsTrue(higherPair.CompareTo(lowerPair) > 0);
        Assert.IsTrue(lowerPair.CompareTo(higherPair) < 0);
    }

    [TestMethod]
    public void CompareTo_SameRankHigherSuitWins()
    {
        // Arrange
        var lowerPair = new PairHand([
            new Card(CardRank.Ace, CardSuit.Clubs),
            new Card(CardRank.Ace, CardSuit.Spades)
        ]);
        var higherPair = new PairHand([
            new Card(CardRank.Ace, CardSuit.Diamonds),
            new Card(CardRank.Ace, CardSuit.Hearts)
        ]);

        // Act & Assert
        Assert.IsTrue(higherPair.CompareTo(lowerPair) > 0);
        Assert.IsTrue(lowerPair.CompareTo(higherPair) < 0);
    }

    [TestMethod]
    public void CompareTo_WithBombHand_ReturnsNegative()
    {
        // Arrange
        var pair = new PairHand([
            new Card(CardRank.Two, CardSuit.Hearts),
            new Card(CardRank.Two, CardSuit.Diamonds)
        ]);
        var bomb = new BombHand([
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Three, CardSuit.Spades)
        ]);

        // Act & Assert
        Assert.IsTrue(pair.CompareTo(bomb) < 0);
    }

    // Note: Full Bomb comparison test will be added when BombHand is implemented
    [TestMethod]
    public void CompareTo_WithNull_ReturnsPositive()
    {
        // Arrange
        var pair = new PairHand([
            new Card(CardRank.Ace, CardSuit.Hearts),
            new Card(CardRank.Ace, CardSuit.Diamonds)
        ]);

        // Act & Assert
        Assert.IsTrue(pair.CompareTo(null) > 0);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CompareTo_WithDifferentHandType_ThrowsException()
    {
        // Arrange
        var pair = new PairHand([
            new Card(CardRank.Ace, CardSuit.Hearts),
            new Card(CardRank.Ace, CardSuit.Diamonds)
        ]);
        var single = new SingleHand(new Card(CardRank.Ace, CardSuit.Hearts));

        // Act - should throw
        pair.CompareTo(single);
    }
}
