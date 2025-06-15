using TienLenAI.Core.Cards;
using TienLenAI.Core.Hands;

namespace TienLenAI.Core.Tests.Hands;

[TestClass]
public class HandFactoryTests
{
    [TestMethod]
    public void CreateHand_WithNull_ReturnsNull()
    {
        // Act
        var result = HandFactory.CreateHand(null);

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void CreateHand_WithEmptyCards_ReturnsNull()
    {
        // Arrange
        var cards = new List<Card>();

        // Act
        var result = HandFactory.CreateHand(cards);

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void CreateHand_WithValidSingle_ReturnsSingleHand()
    {
        // Arrange
        var cards = new[] { new Card(CardRank.Ace, CardSuit.Hearts) };

        // Act
        var result = HandFactory.CreateHand(cards);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(SingleHand));
        Assert.AreEqual(HandType.Single, result.Type);
    }

    [TestMethod]
    public void CreateHand_WithValidPair_ReturnsPairHand()
    {
        // Arrange
        var cards = new[]
        {
            new Card(CardRank.Ace, CardSuit.Hearts),
            new Card(CardRank.Ace, CardSuit.Diamonds)
        };

        // Act
        var result = HandFactory.CreateHand(cards);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(PairHand));
        Assert.AreEqual(HandType.Pair, result.Type);
    }

    [TestMethod]
    public void CreateHand_WithValidTriple_ReturnsTripleHand()
    {
        // Arrange
        var cards = new[]
        {
            new Card(CardRank.King, CardSuit.Hearts),
            new Card(CardRank.King, CardSuit.Diamonds),
            new Card(CardRank.King, CardSuit.Clubs)
        };

        // Act
        var result = HandFactory.CreateHand(cards);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(TripleHand));
        Assert.AreEqual(HandType.Triple, result.Type);
    }

    [TestMethod]
    public void CreateHand_WithValidStraight_ReturnsStraightHand()
    {
        // Arrange
        var cards = new[]
        {
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Five, CardSuit.Clubs)
        };

        // Act
        var result = HandFactory.CreateHand(cards);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(StraightHand));
        Assert.AreEqual(HandType.Straight, result.Type);
    }

    [TestMethod]
    public void CreateHand_WithValidDoubleStraight_ReturnsDoubleStraightHand()
    {
        // Arrange
        var cards = new[]
        {
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Spades),
            new Card(CardRank.Five, CardSuit.Hearts),
            new Card(CardRank.Five, CardSuit.Diamonds)
        };

        // Act
        var result = HandFactory.CreateHand(cards);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(DoubleStraightHand));
        Assert.AreEqual(HandType.DoubleStraight, result.Type);
    }

    [TestMethod]
    public void CreateHand_WithValidTripleStraight_ReturnsTripleStraightHand()
    {
        // Arrange
        var cards = new[]
        {
            new Card(CardRank.Four, CardSuit.Hearts),
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Five, CardSuit.Hearts),
            new Card(CardRank.Five, CardSuit.Diamonds),
            new Card(CardRank.Five, CardSuit.Clubs)
        };

        // Act
        var result = HandFactory.CreateHand(cards);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(TripleStraightHand));
        Assert.AreEqual(HandType.TripleStraight, result.Type);
    }

    [TestMethod]
    public void CreateHand_WithValidBomb_ReturnsBombHand()
    {
        // Arrange
        var cards = new[]
        {
            new Card(CardRank.Seven, CardSuit.Hearts),
            new Card(CardRank.Seven, CardSuit.Diamonds),
            new Card(CardRank.Seven, CardSuit.Clubs),
            new Card(CardRank.Seven, CardSuit.Spades)
        };

        // Act
        var result = HandFactory.CreateHand(cards);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(BombHand));
        Assert.AreEqual(HandType.Bomb, result.Type);
    }

    [TestMethod]
    public void CreateHand_BombTakesPriorityOverTripleStraight()
    {
        // Arrange - 4 cards that could be a bomb (if same rank) but not a triple straight
        var cards = new[]
        {
            new Card(CardRank.Five, CardSuit.Hearts),
            new Card(CardRank.Five, CardSuit.Diamonds),
            new Card(CardRank.Five, CardSuit.Clubs),
            new Card(CardRank.Five, CardSuit.Spades)
        };

        // Act
        var result = HandFactory.CreateHand(cards);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(BombHand));
        Assert.AreEqual(HandType.Bomb, result.Type);
    }

    [TestMethod]
    public void CreateHand_TripleStraightTakesPriorityOverDoubleStraight()
    {
        // Arrange - 6 cards that form both triple straight and double straight
        // This is a tricky case - these cards could theoretically be both
        var cards = new[]
        {
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Hearts),
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Clubs)
        };

        // Act
        var result = HandFactory.CreateHand(cards);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(TripleStraightHand));
        Assert.AreEqual(HandType.TripleStraight, result.Type);
    }

    [TestMethod]
    public void CreateHand_StraightTakesPriorityOverTriple()
    {
        // Arrange - 3 consecutive cards (straight) vs 3 same rank (impossible with consecutive)
        // This tests that straight detection works when we have 3 cards
        var cards = new[]
        {
            new Card(CardRank.Seven, CardSuit.Hearts),
            new Card(CardRank.Eight, CardSuit.Diamonds),
            new Card(CardRank.Nine, CardSuit.Clubs)
        };

        // Act
        var result = HandFactory.CreateHand(cards);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(StraightHand));
        Assert.AreEqual(HandType.Straight, result.Type);
    }

    [TestMethod]
    public void CreateHand_WithInvalidCards_ReturnsNull()
    {
        // Arrange - Non-consecutive, non-matching cards that don't form any valid hand
        var cards = new[]
        {
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Seven, CardSuit.Diamonds)
        };

        // Act
        var result = HandFactory.CreateHand(cards);

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void CreateHand_WithTooManyCards_ReturnsNull()
    {
        // Arrange - More cards than any valid hand type allows
        var cards = new[]
        {
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
            new Card(CardRank.Two, CardSuit.Hearts) // 13 cards - too many for any hand
        };

        // Act
        var result = HandFactory.CreateHand(cards);

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void CreateHand_WithStraightContainingTwo_ReturnsNull()
    {
        // Arrange - Invalid straight containing 2 (not allowed in straights)
        var cards = new[]
        {
            new Card(CardRank.King, CardSuit.Hearts),
            new Card(CardRank.Ace, CardSuit.Diamonds),
            new Card(CardRank.Two, CardSuit.Clubs)
        };

        // Act
        var result = HandFactory.CreateHand(cards);

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void CreateHand_WithInvalidBomb_ReturnsNull()
    {
        // Arrange - 4 cards but different ranks
        var cards = new[]
        {
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Spades)
        };

        // Act
        var result = HandFactory.CreateHand(cards);

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void CreateHand_PreservesCardOrder()
    {
        // Arrange
        var cards = new[]
        {
            new Card(CardRank.Five, CardSuit.Diamonds),
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Four, CardSuit.Clubs)
        };

        // Act
        var result = HandFactory.CreateHand(cards);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(StraightHand));
        // The Hand base class should sort cards by value, so we should have ordered cards
        Assert.AreEqual(3, result.Cards.Count);
        Assert.IsTrue(result.Cards[0].Value < result.Cards[1].Value);
        Assert.IsTrue(result.Cards[1].Value < result.Cards[2].Value);
    }
}
