using TienLenAI.Core.Cards;
using TienLenAI.Core.Hands;

namespace TienLenAI.Core.Tests.Hands;

[TestClass]
public class TripleHandTests
{
    [TestMethod]
    public void IsValid_WithValidTriple_ReturnsTrue()
    {
        // Arrange
        Card[] cards = [
            new Card(CardRank.Ace, CardSuit.Hearts),
            new Card(CardRank.Ace, CardSuit.Diamonds),
            new Card(CardRank.Ace, CardSuit.Clubs)
        ];
        var hand = new TripleHand(cards);

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
            new Card(CardRank.Ace, CardSuit.Diamonds),
            new Card(CardRank.King, CardSuit.Clubs)
        ];
        var hand = new TripleHand(cards);

        // Act
        var isValid = hand.IsValid();

        // Assert
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void IsValid_WithTwoCards_ReturnsFalse()
    {
        // Arrange
        Card[] cards = [
            new Card(CardRank.Ace, CardSuit.Hearts),
            new Card(CardRank.Ace, CardSuit.Diamonds)
        ];
        var hand = new TripleHand(cards);

        // Act
        var isValid = hand.IsValid();

        // Assert
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void CompareTo_HigherTripleWins()
    {
        // Arrange
        var lowerTriple = new TripleHand([
            new Card(CardRank.King, CardSuit.Hearts),
            new Card(CardRank.King, CardSuit.Diamonds),
            new Card(CardRank.King, CardSuit.Clubs)
        ]);
        var higherTriple = new TripleHand([
            new Card(CardRank.Ace, CardSuit.Clubs),
            new Card(CardRank.Ace, CardSuit.Spades),
            new Card(CardRank.Ace, CardSuit.Hearts)
        ]);

        // Act & Assert
        Assert.IsTrue(higherTriple.CompareTo(lowerTriple) > 0);
        Assert.IsTrue(lowerTriple.CompareTo(higherTriple) < 0);
    }

    [TestMethod]
    public void CompareTo_SameRankHigherSuitWins()
    {
        // Arrange
        var lowerTriple = new TripleHand([
            new Card(CardRank.Ace, CardSuit.Clubs),
            new Card(CardRank.Ace, CardSuit.Spades),
            new Card(CardRank.Ace, CardSuit.Diamonds)
        ]);
        var higherTriple = new TripleHand([
            new Card(CardRank.Ace, CardSuit.Clubs),
            new Card(CardRank.Ace, CardSuit.Diamonds),
            new Card(CardRank.Ace, CardSuit.Hearts)
        ]);

        // Act & Assert
        Assert.IsTrue(higherTriple.CompareTo(lowerTriple) > 0);
        Assert.IsTrue(lowerTriple.CompareTo(higherTriple) < 0);
    }

    [TestMethod]
    public void CompareTo_WithBombHand_ReturnsNegative()
    {
        // Arrange
        var triple = new TripleHand([
            new Card(CardRank.Two, CardSuit.Hearts),
            new Card(CardRank.Two, CardSuit.Diamonds),
            new Card(CardRank.Two, CardSuit.Clubs)
        ]);
        var bomb = new BombHand([
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Three, CardSuit.Spades)
        ]);

        // Act & Assert
        Assert.IsTrue(triple.CompareTo(bomb) < 0);
    }

    [TestMethod]
    public void CompareTo_WithNull_ReturnsPositive()
    {
        // Arrange
        var triple = new TripleHand([
            new Card(CardRank.Ace, CardSuit.Hearts),
            new Card(CardRank.Ace, CardSuit.Diamonds),
            new Card(CardRank.Ace, CardSuit.Clubs)
        ]);

        // Act & Assert
        Assert.IsTrue(triple.CompareTo(null) > 0);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CompareTo_WithDifferentHandType_ThrowsException()
    {
        // Arrange
        var triple = new TripleHand([
            new Card(CardRank.Ace, CardSuit.Hearts),
            new Card(CardRank.Ace, CardSuit.Diamonds),
            new Card(CardRank.Ace, CardSuit.Clubs)
        ]);
        var single = new SingleHand(new Card(CardRank.Ace, CardSuit.Hearts));

        // Act - should throw
        triple.CompareTo(single);
    }
}
