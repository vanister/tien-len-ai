using TienLenAI.Core.Cards;
using TienLenAI.Core.Hands;

namespace TienLenAI.Core.Tests.Hands;

[TestClass]
public class SingleHandTests
{
    [TestMethod]
    public void Constructor_WithValidCard_ShouldInitializeCorrectly()
    {
        // Arrange
        var card = new Card(CardRank.Three, CardSuit.Spades);

        // Act
        var hand = new SingleHand(card);

        // Assert
        Assert.AreEqual(1, hand.Cards.Count);
        Assert.AreEqual(card, hand.Cards[0]);
        Assert.AreEqual(HandType.Single, hand.Type);
    }

    [TestMethod]
    public void IsValid_WithSingleCard_ShouldReturnTrue()
    {
        // Arrange
        var hand = new SingleHand(new Card(CardRank.Three, CardSuit.Spades));

        // Act & Assert
        Assert.IsTrue(hand.IsValid());
    }

    [TestMethod]
    public void CompareTo_WithNullHand_ShouldReturnPositive()
    {
        // Arrange
        var hand = new SingleHand(new Card(CardRank.Three, CardSuit.Spades));

        // Act
        var result = hand.CompareTo(null);

        // Assert
        Assert.IsTrue(result > 0);
    }

    [TestMethod]
    public void CompareTo_WithBombHand_ShouldReturnNegative()
    {
        // Arrange
        var singleHand = new SingleHand(new Card(CardRank.Three, CardSuit.Spades));
        var bombHand = new BombHand([
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Three, CardSuit.Spades)
        ]);

        // Act
        var result = singleHand.CompareTo(bombHand);

        // Assert
        Assert.IsTrue(result < 0);
    }

    [TestMethod]
    public void CompareTo_WithHigherCard_ShouldReturnNegative()
    {
        // Arrange
        var lowerHand = new SingleHand(new Card(CardRank.Three, CardSuit.Spades));
        var higherHand = new SingleHand(new Card(CardRank.Four, CardSuit.Spades));

        // Act
        var result = lowerHand.CompareTo(higherHand);

        // Assert
        Assert.IsTrue(result < 0);
    }

    [TestMethod]
    public void CompareTo_WithLowerCard_ShouldReturnPositive()
    {
        // Arrange
        var higherHand = new SingleHand(new Card(CardRank.Four, CardSuit.Spades));
        var lowerHand = new SingleHand(new Card(CardRank.Three, CardSuit.Spades));

        // Act
        var result = higherHand.CompareTo(lowerHand);

        // Assert
        Assert.IsTrue(result > 0);
    }

    [TestMethod]
    public void CompareTo_WithSameSuitDifferentRank_ShouldCompareByRank()
    {
        // Arrange
        var lowerHand = new SingleHand(new Card(CardRank.Three, CardSuit.Spades));
        var higherHand = new SingleHand(new Card(CardRank.Four, CardSuit.Spades));

        // Act
        var result = lowerHand.CompareTo(higherHand);

        // Assert
        Assert.IsTrue(result < 0);
    }

    [TestMethod]
    public void CompareTo_WithSameRankDifferentSuit_ShouldCompareBySuit()
    {
        // Arrange
        var lowerHand = new SingleHand(new Card(CardRank.Three, CardSuit.Spades));
        var higherHand = new SingleHand(new Card(CardRank.Three, CardSuit.Hearts));

        // Act
        var result = lowerHand.CompareTo(higherHand);

        // Assert
        Assert.IsTrue(result < 0);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CompareTo_WithDifferentHandType_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var singleHand = new SingleHand(new Card(CardRank.Three, CardSuit.Spades));
        var pairHand = new PairHand([
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds)
        ]);

        // Act - should throw
        singleHand.CompareTo(pairHand);
    }
}
