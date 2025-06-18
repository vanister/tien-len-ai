using TienLenAi2.Core;

namespace TienLenAi2.Core.Tests;

[TestClass]
public class CardTests
{
    [TestMethod]
    public void Card_Creation_ShouldCreateValidCard()
    {
        // Arrange & Act
        var card = new Card(Rank.Ace, Suit.Hearts);

        // Assert
        Assert.AreEqual(Rank.Ace, card.Rank);
        Assert.AreEqual(Suit.Hearts, card.Suit);
    }

    [TestMethod]
    public void Card_Value_ShouldCalculateCorrectly()
    {
        // Arrange
        var aceOfHearts = new Card(Rank.Ace, Suit.Hearts);
        var threeOfSpades = new Card(Rank.Three, Suit.Spades);
        var twoOfClubs = new Card(Rank.Two, Suit.Clubs);

        // Act & Assert
        // Value = (int)Rank * 4 + (int)Suit
        Assert.AreEqual(14 * 4 + 3, aceOfHearts.Value); // 59
        Assert.AreEqual(3 * 4 + 0, threeOfSpades.Value); // 12
        Assert.AreEqual(15 * 4 + 1, twoOfClubs.Value); // 61
    }

    [TestMethod]
    public void Card_CompareTo_ShouldCompareByRankFirst()
    {
        // Arrange
        var aceOfSpades = new Card(Rank.Ace, Suit.Spades);
        var kingOfHearts = new Card(Rank.King, Suit.Hearts);

        // Act & Assert
        Assert.IsTrue(aceOfSpades.CompareTo(kingOfHearts) > 0, "Ace should be greater than King");
        Assert.IsTrue(kingOfHearts.CompareTo(aceOfSpades) < 0, "King should be less than Ace");
    }

    [TestMethod]
    public void Card_CompareTo_ShouldCompareBySuitWhenRanksSame()
    {
        // Arrange
        var aceOfSpades = new Card(Rank.Ace, Suit.Spades);
        var aceOfClubs = new Card(Rank.Ace, Suit.Clubs);
        var aceOfDiamonds = new Card(Rank.Ace, Suit.Diamonds);
        var aceOfHearts = new Card(Rank.Ace, Suit.Hearts);

        // Act & Assert
        // Hearts > Diamonds > Clubs > Spades
        Assert.IsTrue(aceOfHearts.CompareTo(aceOfDiamonds) > 0);
        Assert.IsTrue(aceOfDiamonds.CompareTo(aceOfClubs) > 0);
        Assert.IsTrue(aceOfClubs.CompareTo(aceOfSpades) > 0);
    }

    [TestMethod]
    public void Card_CompareTo_WithNull_ShouldReturnPositive()
    {
        // Arrange
        var card = new Card(Rank.Three, Suit.Spades);

        // Act
        var result = card.CompareTo(null);

        // Assert
        Assert.AreEqual(1, result);
    }

    [TestMethod]
    public void Card_CompareTo_WithSameCard_ShouldReturnZero()
    {
        // Arrange
        var card1 = new Card(Rank.Five, Suit.Diamonds);
        var card2 = new Card(Rank.Five, Suit.Diamonds);

        // Act
        var result = card1.CompareTo(card2);

        // Assert
        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Card_ToString_ShouldReturnCorrectFormat()
    {
        // Arrange & Act
        var aceOfHearts = new Card(Rank.Ace, Suit.Hearts);
        var threeOfSpades = new Card(Rank.Three, Suit.Spades);
        var tenOfDiamonds = new Card(Rank.Ten, Suit.Diamonds);
        var twoOfClubs = new Card(Rank.Two, Suit.Clubs);

        // Assert
        Assert.AreEqual("A♥ (A of Hearts)", aceOfHearts.ToString());
        Assert.AreEqual("3♠ (3 of Spades)", threeOfSpades.ToString());
        Assert.AreEqual("10♦ (10 of Diamonds)", tenOfDiamonds.ToString());
        Assert.AreEqual("2♣ (2 of Clubs)", twoOfClubs.ToString());
    }

    [TestMethod]
    public void Card_ToString_AllRanks_ShouldReturnCorrectFormat()
    {
        // Test all ranks have correct string representation
        var testCases = new Dictionary<Rank, string>
        {
            { Rank.Three, "3" },
            { Rank.Four, "4" },
            { Rank.Five, "5" },
            { Rank.Six, "6" },
            { Rank.Seven, "7" },
            { Rank.Eight, "8" },
            { Rank.Nine, "9" },
            { Rank.Ten, "10" },
            { Rank.Jack, "J" },
            { Rank.Queen, "Q" },
            { Rank.King, "K" },
            { Rank.Ace, "A" },
            { Rank.Two, "2" }
        };

        foreach (var testCase in testCases)
        {
            // Arrange
            var card = new Card(testCase.Key, Suit.Hearts);

            // Act
            var result = card.ToString();

            // Assert
            Assert.IsTrue(result.StartsWith(testCase.Value), 
                $"Card {testCase.Key} should start with '{testCase.Value}', but got '{result}'");
        }
    }

    [TestMethod]
    public void Card_ToString_AllSuits_ShouldReturnCorrectSymbols()
    {
        // Test all suits have correct symbols
        var testCases = new Dictionary<Suit, (string symbol, string name)>
        {
            { Suit.Spades, ("♠", "Spades") },
            { Suit.Clubs, ("♣", "Clubs") },
            { Suit.Diamonds, ("♦", "Diamonds") },
            { Suit.Hearts, ("♥", "Hearts") }
        };

        foreach (var testCase in testCases)
        {
            // Arrange
            var card = new Card(Rank.Ace, testCase.Key);

            // Act
            var result = card.ToString();

            // Assert
            Assert.IsTrue(result.Contains(testCase.Value.symbol), 
                $"Card should contain symbol '{testCase.Value.symbol}', but got '{result}'");
            Assert.IsTrue(result.Contains(testCase.Value.name), 
                $"Card should contain name '{testCase.Value.name}', but got '{result}'");
        }
    }

    [TestMethod]
    public void Card_Equality_ShouldWorkCorrectly()
    {
        // Arrange
        var card1 = new Card(Rank.King, Suit.Hearts);
        var card2 = new Card(Rank.King, Suit.Hearts);
        var card3 = new Card(Rank.King, Suit.Spades);

        // Act & Assert
        Assert.AreEqual(card1, card2, "Same rank and suit should be equal");
        Assert.AreNotEqual(card1, card3, "Different suits should not be equal");
        Assert.IsTrue(card1 == card2, "== operator should work");
        Assert.IsTrue(card1 != card3, "!= operator should work");
    }

    [TestMethod]
    public void Card_GetHashCode_ShouldBeConsistent()
    {
        // Arrange
        var card1 = new Card(Rank.Queen, Suit.Diamonds);
        var card2 = new Card(Rank.Queen, Suit.Diamonds);

        // Act & Assert
        Assert.AreEqual(card1.GetHashCode(), card2.GetHashCode(), 
            "Equal cards should have equal hash codes");
    }

    [TestMethod]
    public void Card_TienLenRanking_ShouldRespectGameRules()
    {
        // In Tiến Lên, Two is the highest rank
        var twoOfSpades = new Card(Rank.Two, Suit.Spades);
        var aceOfHearts = new Card(Rank.Ace, Suit.Hearts);
        var threeOfClubs = new Card(Rank.Three, Suit.Clubs);

        // Act & Assert
        Assert.IsTrue(twoOfSpades.CompareTo(aceOfHearts) > 0, "Two should be higher than Ace in Tiến Lên");
        Assert.IsTrue(aceOfHearts.CompareTo(threeOfClubs) > 0, "Ace should be higher than Three");
        Assert.IsTrue(twoOfSpades.CompareTo(threeOfClubs) > 0, "Two should be higher than Three");
    }
}