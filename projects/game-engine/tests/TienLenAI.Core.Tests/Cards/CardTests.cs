using Microsoft.VisualStudio.TestTools.UnitTesting;
using TienLenAI.Core.Cards;

namespace TienLenAI.Core.Tests.Cards
{
    [TestClass]
    public class CardTests
    {
        [TestMethod]
        public void Card_ShouldHaveCorrectValue()
        {
            // Arrange
            var card = new Card(CardRank.Three, CardSuit.Spades);

            // Act
            var value = card.Value;

            // Assert
            Assert.AreEqual(0, value); // 3♠ should have value 0
        }

        [TestMethod]
        public void Card_ShouldCompareCorrectly()
        {
            // Arrange
            var lowerCard = new Card(CardRank.Three, CardSuit.Spades);
            var higherCard = new Card(CardRank.Four, CardSuit.Hearts);

            // Act
            var comparison = lowerCard.CompareTo(higherCard);

            // Assert
            Assert.IsTrue(comparison < 0); // lowerCard should be less than higherCard
        }

        [TestMethod]
        public void Card_ShouldBeEqual_WhenSameRankAndSuit()
        {
            // Arrange
            var card1 = new Card(CardRank.Three, CardSuit.Spades);
            var card2 = new Card(CardRank.Three, CardSuit.Spades);

            // Act
            var isEqual = card1.Equals(card2);

            // Assert
            Assert.IsTrue(isEqual); // Cards with same rank and suit should be equal
        }

        [TestMethod]
        public void Card_ShouldNotBeEqual_WhenDifferentRankOrSuit()
        {
            // Arrange
            var card1 = new Card(CardRank.Three, CardSuit.Spades);
            var card2 = new Card(CardRank.Four, CardSuit.Spades);

            // Act
            var isEqual = card1.Equals(card2);

            // Assert
            Assert.IsFalse(isEqual); // Cards with different rank or suit should not be equal
        }
    }
}
