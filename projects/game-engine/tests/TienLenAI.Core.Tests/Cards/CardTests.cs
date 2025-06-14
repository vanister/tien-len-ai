using TienLenAI.Core.Cards;

namespace TienLenAI.Core.Tests.Cards;

[TestClass]
public class CardTests
{
    [TestMethod]
    public void Constructor_WithRankAndSuit_SetsProperties()
    {
        // Arrange & Act
        var card = new Card(CardRank.Three, CardSuit.Spades);

        // Assert
        Assert.AreEqual(CardRank.Three, card.Rank);
        Assert.AreEqual(CardSuit.Spades, card.Suit);
        Assert.AreEqual(0, card.Value); // 3♠ = 0
    }

    [TestMethod]
    public void Constructor_WithValue_SetsProperties()
    {
        // Arrange & Act
        var card = new Card(0); // 3♠

        // Assert
        Assert.AreEqual(CardRank.Three, card.Rank);
        Assert.AreEqual(CardSuit.Spades, card.Suit);
        Assert.AreEqual(0, card.Value);
    }

    [TestMethod]
    [DataRow(-1)]
    [DataRow(52)]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Constructor_WithInvalidValue_ThrowsException(int value)
    {
        // Act - should throw
        _ = new Card(value);
    }

    [TestMethod]
    public void Value_CalculatesCorrectly_ForNumericalRanks()
    {
        // Test boundary cases and representative values
        AssertCardValue(new Card(CardRank.Three, CardSuit.Spades), 0);    // Lowest card (3♠)
        AssertCardValue(new Card(CardRank.Three, CardSuit.Hearts), 3);    // 3♥
        AssertCardValue(new Card(CardRank.Four, CardSuit.Spades), 4);     // 4♠
        AssertCardValue(new Card(CardRank.Ten, CardSuit.Diamonds), 30);   // 10♦
    }

    [TestMethod]
    public void Value_CalculatesCorrectly_ForFaceCards()
    {
        AssertCardValue(new Card(CardRank.Jack, CardSuit.Spades), 32);    // J♠
        AssertCardValue(new Card(CardRank.Queen, CardSuit.Clubs), 37);    // Q♣
        AssertCardValue(new Card(CardRank.King, CardSuit.Diamonds), 42);  // K♦
        AssertCardValue(new Card(CardRank.Ace, CardSuit.Hearts), 47);     // A♥
    }

    [TestMethod]
    public void Value_CalculatesCorrectly_ForHighestRank()
    {
        // Two should be valued highest in Tiến Lên
        AssertCardValue(new Card(CardRank.Two, CardSuit.Spades), 48);     // 2♠
        AssertCardValue(new Card(CardRank.Two, CardSuit.Clubs), 49);      // 2♣
        AssertCardValue(new Card(CardRank.Two, CardSuit.Diamonds), 50);   // 2♦
        AssertCardValue(new Card(CardRank.Two, CardSuit.Hearts), 51);     // 2♥ (highest card)
    }

    [TestMethod]
    public void Equals_WithSameCard_ReturnsTrue()
    {
        // Arrange
        var card1 = new Card(CardRank.Ace, CardSuit.Hearts);
        var card2 = new Card(CardRank.Ace, CardSuit.Hearts);

        // Act & Assert
        Assert.IsTrue(card1.Equals(card2));
        Assert.IsTrue(card1.Equals((object)card2));
        Assert.IsTrue(card1 == card2);
        Assert.IsFalse(card1 != card2);
    }

    [TestMethod]
    public void Equals_WithDifferentCards_ReturnsFalse()
    {
        // Arrange
        var card1 = new Card(CardRank.Ace, CardSuit.Hearts);
        var card2 = new Card(CardRank.Ace, CardSuit.Diamonds);

        // Act & Assert
        Assert.IsFalse(card1.Equals(card2));
        Assert.IsFalse(card1.Equals((object)card2));
        Assert.IsFalse(card1 == card2);
        Assert.IsTrue(card1 != card2);
    }

    [TestMethod]
    public void Equals_WithObject_WorksCorrectly()
    {
        // Arrange
        var card = new Card(CardRank.Ace, CardSuit.Hearts);
        object cardAsObject = new Card(CardRank.Ace, CardSuit.Hearts);
        object differentObject = "not a card";

        // Act & Assert
        Assert.IsTrue(card.Equals(cardAsObject));
        Assert.IsFalse(card.Equals(differentObject));
        Assert.IsFalse(card.Equals((object?)null));
    }

    [TestMethod]
    public void Compare_WithDifferentRanks_ComparesCorrectly()
    {
        // Arrange
        var lowerCard = new Card(CardRank.Three, CardSuit.Hearts);
        var higherCard = new Card(CardRank.Four, CardSuit.Spades);

        // Act & Assert
        Assert.IsTrue(lowerCard.CompareTo(higherCard) < 0);
        Assert.IsTrue(higherCard.CompareTo(lowerCard) > 0);
        Assert.IsTrue(lowerCard < higherCard);
        Assert.IsTrue(higherCard > lowerCard);
        Assert.IsTrue(lowerCard <= higherCard);
        Assert.IsTrue(higherCard >= lowerCard);
    }

    [TestMethod]
    public void Compare_WithSameRankDifferentSuits_ComparesCorrectly()
    {
        // Arrange
        var lowerCard = new Card(CardRank.Three, CardSuit.Spades);
        var higherCard = new Card(CardRank.Three, CardSuit.Hearts);

        // Act & Assert
        Assert.IsTrue(lowerCard.CompareTo(higherCard) < 0);
        Assert.IsTrue(higherCard.CompareTo(lowerCard) > 0);
        Assert.IsTrue(lowerCard < higherCard);
        Assert.IsTrue(higherCard > lowerCard);
        Assert.IsTrue(lowerCard <= higherCard);
        Assert.IsTrue(higherCard >= lowerCard);
    }

    [TestMethod]
    public void Compare_WithSameCard_ReturnsZero()
    {
        // Arrange
        var card1 = new Card(CardRank.Three, CardSuit.Spades);
        var card2 = new Card(CardRank.Three, CardSuit.Spades);

        // Act & Assert
        Assert.AreEqual(0, card1.CompareTo(card2));
        Assert.IsTrue(card1 <= card2);
        Assert.IsTrue(card1 >= card2);
    }



    [TestMethod]
    public void ToString_WithNumberCard_ReturnsCorrectFormat()
    {
        // Test numerical ranks
        var testCases = new Dictionary<Card, string>
        {
            { new Card(CardRank.Three, CardSuit.Spades), "Three♠" },
            { new Card(CardRank.Four, CardSuit.Hearts), "Four♥" },
            { new Card(CardRank.Ten, CardSuit.Diamonds), "Ten♦" }
        };

        foreach (var (card, expected) in testCases)
        {
            Assert.AreEqual(expected, card.ToString(), $"Failed for card: {card.Rank} of {card.Suit}");
        }
    }

    [TestMethod]
    public void ToString_WithFaceCard_ReturnsCorrectFormat()
    {
        // Test all face cards and special ranks
        var testCases = new Dictionary<Card, string>
        {
            { new Card(CardRank.Jack, CardSuit.Spades), "J♠" },
            { new Card(CardRank.Queen, CardSuit.Clubs), "Q♣" },
            { new Card(CardRank.King, CardSuit.Diamonds), "K♦" },
            { new Card(CardRank.Ace, CardSuit.Hearts), "A♥" },
            { new Card(CardRank.Two, CardSuit.Spades), "2♠" }
        };

        foreach (var (card, expected) in testCases)
        {
            Assert.AreEqual(expected, card.ToString(), $"Failed for card: {card.Rank} of {card.Suit}");
        }
    }

    [TestMethod]
    public void ToString_WithAllSuits_ReturnsCorrectSymbols()
    {
        // Test all suits with same rank
        var testCases = new Dictionary<CardSuit, string>
        {
            { CardSuit.Spades, "♠" },
            { CardSuit.Clubs, "♣" },
            { CardSuit.Diamonds, "♦" },
            { CardSuit.Hearts, "♥" }
        };

        foreach (var (suit, symbol) in testCases)
        {
            var card = new Card(CardRank.Three, suit);
            Assert.IsTrue(card.ToString().EndsWith(symbol),
                $"Card string should end with {symbol} for suit {suit}, but was: {card}");
        }
    }

    [TestMethod]
    public void GetHashCode_ReturnsSameValueForEqualCards()
    {
        // Arrange
        var card1 = new Card(CardRank.Ace, CardSuit.Hearts);
        var card2 = new Card(CardRank.Ace, CardSuit.Hearts);

        // Act & Assert
        Assert.AreEqual(card1.GetHashCode(), card2.GetHashCode());
    }

    private static void AssertCardValue(Card card, int expectedValue)
    {
        Assert.AreEqual(expectedValue, card.Value,
            $"Card {card} should have value {expectedValue} but got {card.Value}");

        // Also verify that constructing with this value creates the same card
        var reconstructed = new Card(expectedValue);
        Assert.AreEqual(card.Rank, reconstructed.Rank,
            $"Reconstructing from value {expectedValue} gave wrong rank");
        Assert.AreEqual(card.Suit, reconstructed.Suit,
            $"Reconstructing from value {expectedValue} gave wrong suit");
    }
}
