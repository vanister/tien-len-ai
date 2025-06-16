using TienLenAI.Core.Cards;
using TienLenAI.Core.Hands;
using TienLenAI.Core.State;

namespace TienLenAI.Core.Tests.State;

[TestClass]
public class TrickStateTests
{
    [TestMethod]
    public void Constructor_WithValidParameters_InitializesCorrectly()
    {
        // Arrange & Act
        var trickState = new TrickState(0, 4);

        // Assert
        Assert.IsNull(trickState.RequiredHandType);
        Assert.IsNull(trickState.CurrentHand);
        Assert.AreEqual(0, trickState.StartingPlayerIndex);
        Assert.AreEqual(0, trickState.LastPlayingPlayerIndex);
        Assert.AreEqual(4, trickState.PlayersPassed.Count);
        Assert.IsTrue(trickState.PlayersPassed.All(passed => !passed));
        Assert.IsFalse(trickState.IsComplete);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Constructor_WithNegativeStartingPlayer_ThrowsException()
    {
        // Act
        _ = new TrickState(-1, 4);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Constructor_WithStartingPlayerTooHigh_ThrowsException()
    {
        // Act
        _ = new TrickState(4, 4);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Constructor_WithZeroPlayers_ThrowsException()
    {
        // Act - This will throw for totalPlayers parameter
        _ = new TrickState(0, 0);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Constructor_WithNegativePlayers_ThrowsException()
    {
        // Act - This will throw for totalPlayers parameter
        _ = new TrickState(0, -1);
    }

    [TestMethod]
    public void WithPlayerPlay_FirstPlay_SetsHandTypeAndHand()
    {
        // Arrange
        var trickState = new TrickState(0, 4);
        var hand = new SingleHand(new Card(CardRank.Ace, CardSuit.Hearts));

        // Act
        var newState = trickState.WithPlayerPlay(hand, 0);

        // Assert
        Assert.AreEqual(HandType.Single, newState.RequiredHandType);
        Assert.AreEqual(hand, newState.CurrentHand);
        Assert.AreEqual(0, newState.LastPlayingPlayerIndex);
        Assert.IsFalse(newState.IsComplete);
    }

    [TestMethod]
    public void WithPlayerPlay_ValidPlay_BeatsCurrentHand()
    {
        // Arrange
        var trickState = new TrickState(0, 4);
        var firstHand = new SingleHand(new Card(CardRank.Three, CardSuit.Spades));
        var secondHand = new SingleHand(new Card(CardRank.Four, CardSuit.Hearts));

        var stateAfterFirst = trickState.WithPlayerPlay(firstHand, 0);

        // Act
        var stateAfterSecond = stateAfterFirst.WithPlayerPlay(secondHand, 1);

        // Assert
        Assert.AreEqual(HandType.Single, stateAfterSecond.RequiredHandType);
        Assert.AreEqual(secondHand, stateAfterSecond.CurrentHand);
        Assert.AreEqual(1, stateAfterSecond.LastPlayingPlayerIndex);
        Assert.IsFalse(stateAfterSecond.IsComplete);
    }

    [TestMethod]
    public void WithPlayerPlay_BombBeatsNonBomb_IsLegal()
    {
        // Arrange
        var trickState = new TrickState(0, 4);
        var straight = new StraightHand([
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Five, CardSuit.Clubs)
        ]);
        var bomb = new BombHand([
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Three, CardSuit.Spades)
        ]);

        var stateAfterStraight = trickState.WithPlayerPlay(straight, 0);

        // Act
        var stateAfterBomb = stateAfterStraight.WithPlayerPlay(bomb, 1);

        // Assert
        Assert.AreEqual(HandType.Bomb, stateAfterBomb.RequiredHandType);
        Assert.AreEqual(bomb, stateAfterBomb.CurrentHand);
        Assert.AreEqual(1, stateAfterBomb.LastPlayingPlayerIndex);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void WithPlayerPlay_WithNullHand_ThrowsException()
    {
        // Arrange
        var trickState = new TrickState(0, 4);

        // Act
        trickState.WithPlayerPlay(null, 0);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void WithPlayerPlay_WithInvalidPlayerIndex_ThrowsException()
    {
        // Arrange
        var trickState = new TrickState(0, 4);
        var hand = new SingleHand(new Card(CardRank.Ace, CardSuit.Hearts));

        // Act
        trickState.WithPlayerPlay(hand, 5);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void WithPlayerPlay_OnCompletedTrick_ThrowsException()
    {
        // Arrange
        var trickState = new TrickState(0, 2);
        var hand = new SingleHand(new Card(CardRank.Ace, CardSuit.Hearts));

        var stateAfterPlay = trickState.WithPlayerPlay(hand, 0);
        var stateAfterPass = stateAfterPlay.WithPlayerPass(1);

        // Act - should throw since trick is complete
        stateAfterPass.WithPlayerPlay(hand, 0);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void WithPlayerPlay_WhenPlayerAlreadyPassed_ThrowsException()
    {
        // Arrange
        var trickState = new TrickState(0, 4);
        var hand = new SingleHand(new Card(CardRank.Ace, CardSuit.Hearts));

        var stateAfterPlay = trickState.WithPlayerPlay(hand, 0);
        var stateAfterPass = stateAfterPlay.WithPlayerPass(1);

        // Act - player 1 already passed
        stateAfterPass.WithPlayerPlay(hand, 1);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void WithPlayerPlay_WithWeakerHand_ThrowsException()
    {
        // Arrange
        var trickState = new TrickState(0, 4);
        var strongHand = new SingleHand(new Card(CardRank.Ace, CardSuit.Hearts));
        var weakHand = new SingleHand(new Card(CardRank.Three, CardSuit.Spades));

        var stateAfterStrong = trickState.WithPlayerPlay(strongHand, 0);

        // Act - weak hand can't beat strong hand
        stateAfterStrong.WithPlayerPlay(weakHand, 1);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void WithPlayerPlay_WithWrongHandType_ThrowsException()
    {
        // Arrange
        var trickState = new TrickState(0, 4);
        var single = new SingleHand(new Card(CardRank.Three, CardSuit.Hearts));
        var pair = new PairHand([
            new Card(CardRank.Ace, CardSuit.Hearts),
            new Card(CardRank.Ace, CardSuit.Diamonds)
        ]);

        var stateAfterSingle = trickState.WithPlayerPlay(single, 0);

        // Act - pair doesn't match single hand type
        stateAfterSingle.WithPlayerPlay(pair, 1);
    }

    [TestMethod]
    public void WithPlayerPass_UpdatesPassStatus()
    {
        // Arrange
        var trickState = new TrickState(0, 4);
        var hand = new SingleHand(new Card(CardRank.Ace, CardSuit.Hearts));
        var stateAfterPlay = trickState.WithPlayerPlay(hand, 0);

        // Act
        var stateAfterPass = stateAfterPlay.WithPlayerPass(1);

        // Assert
        Assert.IsTrue(stateAfterPass.PlayersPassed[1]);
        Assert.IsFalse(stateAfterPass.PlayersPassed[0]);
        Assert.IsFalse(stateAfterPass.PlayersPassed[2]);
        Assert.IsFalse(stateAfterPass.PlayersPassed[3]);
    }

    [TestMethod]
    public void WithPlayerPass_CompletesTorick_WhenAllOthersPass()
    {
        // Arrange - 2 player game for simple completion test
        var trickState = new TrickState(0, 2);
        var hand = new SingleHand(new Card(CardRank.Ace, CardSuit.Hearts));
        var stateAfterPlay = trickState.WithPlayerPlay(hand, 0);

        // Act
        var stateAfterPass = stateAfterPlay.WithPlayerPass(1);

        // Assert
        Assert.IsTrue(stateAfterPass.IsComplete);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void WithPlayerPass_WithInvalidPlayerIndex_ThrowsException()
    {
        // Arrange
        var trickState = new TrickState(0, 4);

        // Act
        trickState.WithPlayerPass(5);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void WithPlayerPass_OnCompletedTrick_ThrowsException()
    {
        // Arrange
        var trickState = new TrickState(0, 2);
        var hand = new SingleHand(new Card(CardRank.Ace, CardSuit.Hearts));

        var stateAfterPlay = trickState.WithPlayerPlay(hand, 0);
        var completedState = stateAfterPlay.WithPlayerPass(1);

        // Act - should throw since trick is complete
        completedState.WithPlayerPass(0);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void WithPlayerPass_WhenPlayerAlreadyPassed_ThrowsException()
    {
        // Arrange
        var trickState = new TrickState(0, 4);
        var hand = new SingleHand(new Card(CardRank.Ace, CardSuit.Hearts));

        var stateAfterPlay = trickState.WithPlayerPlay(hand, 0);
        var stateAfterPass = stateAfterPlay.WithPlayerPass(1);

        // Act - player 1 already passed
        stateAfterPass.WithPlayerPass(1);
    }

    [TestMethod]
    public void IsLegalPlay_FirstPlay_AcceptsAnyValidHand()
    {
        // Arrange
        var trickState = new TrickState(0, 4);
        var single = new SingleHand(new Card(CardRank.Ace, CardSuit.Hearts));
        var pair = new PairHand([
            new Card(CardRank.King, CardSuit.Hearts),
            new Card(CardRank.King, CardSuit.Diamonds)
        ]);

        // Act & Assert
        Assert.IsTrue(trickState.IsLegalPlay(single));
        Assert.IsTrue(trickState.IsLegalPlay(pair));
    }

    [TestMethod]
    public void IsLegalPlay_WithInvalidHand_ReturnsFalse()
    {
        // Arrange
        var trickState = new TrickState(0, 4);
        var invalidHand = new PairHand([
            new Card(CardRank.Ace, CardSuit.Hearts),
            new Card(CardRank.King, CardSuit.Diamonds) // Different ranks - invalid pair
        ]);

        // Act & Assert
        Assert.IsFalse(trickState.IsLegalPlay(invalidHand));
    }

    [TestMethod]
    public void GetNextPlayer_WhenAllOthersHavePassed_ReturnsNull()
    {
        // Arrange - 3 player game where 2 players pass
        var trickState = new TrickState(0, 3);
        var hand = new SingleHand(new Card(CardRank.Ace, CardSuit.Hearts));

        var stateAfterPlay = trickState.WithPlayerPlay(hand, 0);
        var stateAfterPass1 = stateAfterPlay.WithPlayerPass(1);
        var stateAfterPass2 = stateAfterPass1.WithPlayerPass(2);

        // Act - Only player 0 is active, no next player
        var nextPlayer = stateAfterPass2.GetNextPlayer(0);

        // Assert
        Assert.IsNull(nextPlayer);
    }

    [TestMethod]
    public void CheckTrickComplete_BeforeAnyPlays_ReturnsFalse()
    {
        // Arrange - Fresh trick state
        var trickState = new TrickState(0, 4);

        // Act & Assert
        Assert.IsFalse(trickState.IsComplete);
    }

    [TestMethod]
    public void CheckTrickComplete_FourPlayerGame_RequiresThreePasses()
    {
        // Arrange
        var trickState = new TrickState(0, 4);
        var hand = new SingleHand(new Card(CardRank.Ace, CardSuit.Hearts));

        var stateAfterPlay = trickState.WithPlayerPlay(hand, 0);
        var stateAfterPass1 = stateAfterPlay.WithPlayerPass(1);
        var stateAfterPass2 = stateAfterPass1.WithPlayerPass(2);

        // Act & Assert
        Assert.IsFalse(stateAfterPass2.IsComplete); // Only 2 passes, need 3

        var stateAfterPass3 = stateAfterPass2.WithPlayerPass(3);
        Assert.IsTrue(stateAfterPass3.IsComplete); // Now 3 passes, complete
    }

    [TestMethod]
    public void IsLegalPlay_MatchingTypeStrongerHand_ReturnsTrue()
    {
        // Arrange
        var trickState = new TrickState(0, 4);
        var weakSingle = new SingleHand(new Card(CardRank.Three, CardSuit.Spades));
        var strongSingle = new SingleHand(new Card(CardRank.Ace, CardSuit.Hearts));

        var stateAfterWeak = trickState.WithPlayerPlay(weakSingle, 0);

        // Act & Assert
        Assert.IsTrue(stateAfterWeak.IsLegalPlay(strongSingle));
    }

    [TestMethod]
    public void IsLegalPlay_MatchingTypeWeakerHand_ReturnsFalse()
    {
        // Arrange
        var trickState = new TrickState(0, 4);
        var strongSingle = new SingleHand(new Card(CardRank.Ace, CardSuit.Hearts));
        var weakSingle = new SingleHand(new Card(CardRank.Three, CardSuit.Spades));

        var stateAfterStrong = trickState.WithPlayerPlay(strongSingle, 0);

        // Act & Assert
        Assert.IsFalse(stateAfterStrong.IsLegalPlay(weakSingle));
    }

    [TestMethod]
    public void IsLegalPlay_BombOnBomb_RequiresStrongerBomb()
    {
        // Arrange
        var trickState = new TrickState(0, 4);
        var weakBomb = new BombHand([
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Three, CardSuit.Spades)
        ]);
        var strongBomb = new BombHand([
            new Card(CardRank.Four, CardSuit.Hearts),
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Four, CardSuit.Spades)
        ]);
        var weakerBomb = new BombHand([
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Three, CardSuit.Spades)
        ]);

        var stateAfterWeakBomb = trickState.WithPlayerPlay(weakBomb, 0);

        // Act & Assert
        Assert.IsTrue(stateAfterWeakBomb.IsLegalPlay(strongBomb));
        Assert.IsFalse(stateAfterWeakBomb.IsLegalPlay(weakerBomb)); // Same strength, not stronger
    }

    [TestMethod]
    public void GetActivePlayers_InitialState_ReturnsAllPlayers()
    {
        // Arrange
        var trickState = new TrickState(0, 4);

        // Act
        var activePlayers = trickState.GetActivePlayers().ToList();

        // Assert
        CollectionAssert.AreEqual(new[] { 0, 1, 2, 3 }, activePlayers);
    }

    [TestMethod]
    public void GetActivePlayers_AfterSomePasses_ReturnsOnlyActivePlayers()
    {
        // Arrange
        var trickState = new TrickState(0, 4);
        var hand = new SingleHand(new Card(CardRank.Ace, CardSuit.Hearts));

        var stateAfterPlay = trickState.WithPlayerPlay(hand, 0);
        var stateAfterPass1 = stateAfterPlay.WithPlayerPass(1);
        var stateAfterPass2 = stateAfterPass1.WithPlayerPass(2);

        // Act
        var activePlayers = stateAfterPass2.GetActivePlayers().ToList();

        // Assert
        CollectionAssert.AreEqual(new[] { 0, 3 }, activePlayers);
    }

    [TestMethod]
    public void GetActivePlayers_CompletedTrick_ReturnsEmpty()
    {
        // Arrange
        var trickState = new TrickState(0, 2);
        var hand = new SingleHand(new Card(CardRank.Ace, CardSuit.Hearts));

        var stateAfterPlay = trickState.WithPlayerPlay(hand, 0);
        var completedState = stateAfterPlay.WithPlayerPass(1);

        // Act
        var activePlayers = completedState.GetActivePlayers().ToList();

        // Assert
        Assert.AreEqual(0, activePlayers.Count);
    }

    [TestMethod]
    public void GetNextPlayer_InTurnOrder_ReturnsCorrectPlayer()
    {
        // Arrange
        var trickState = new TrickState(0, 4);
        var hand = new SingleHand(new Card(CardRank.Ace, CardSuit.Hearts));
        var stateAfterPlay = trickState.WithPlayerPlay(hand, 0);

        // Act & Assert
        Assert.AreEqual(1, stateAfterPlay.GetNextPlayer(0));
        Assert.AreEqual(2, stateAfterPlay.GetNextPlayer(1));
        Assert.AreEqual(3, stateAfterPlay.GetNextPlayer(2));
        Assert.AreEqual(0, stateAfterPlay.GetNextPlayer(3));
    }

    [TestMethod]
    public void GetNextPlayer_SkipsPassedPlayers_ReturnsNextActivePlayer()
    {
        // Arrange
        var trickState = new TrickState(0, 4);
        var hand = new SingleHand(new Card(CardRank.Ace, CardSuit.Hearts));

        var stateAfterPlay = trickState.WithPlayerPlay(hand, 0);
        var stateAfterPass = stateAfterPlay.WithPlayerPass(1);

        // Act
        var nextPlayer = stateAfterPass.GetNextPlayer(0);

        // Assert
        Assert.AreEqual(2, nextPlayer); // Skips player 1 who passed
    }

    [TestMethod]
    public void GetNextPlayer_CompletedTrick_ReturnsNull()
    {
        // Arrange
        var trickState = new TrickState(0, 2);
        var hand = new SingleHand(new Card(CardRank.Ace, CardSuit.Hearts));

        var stateAfterPlay = trickState.WithPlayerPlay(hand, 0);
        var completedState = stateAfterPlay.WithPlayerPass(1);

        // Act
        var nextPlayer = completedState.GetNextPlayer(0);

        // Assert
        Assert.IsNull(nextPlayer);
    }

    [TestMethod]
    public void StartNewTrick_FromCompletedTrick_CreatesNewTrickState()
    {
        // Arrange
        var trickState = new TrickState(0, 4);
        var hand = new SingleHand(new Card(CardRank.Ace, CardSuit.Hearts));

        var stateAfterPlay = trickState.WithPlayerPlay(hand, 0);
        var stateAfterPass1 = stateAfterPlay.WithPlayerPass(1);
        var stateAfterPass2 = stateAfterPass1.WithPlayerPass(2);
        var completedState = stateAfterPass2.WithPlayerPass(3);

        // Act
        var newTrickState = completedState.StartNewTrick(2);

        // Assert
        Assert.IsNull(newTrickState.RequiredHandType);
        Assert.IsNull(newTrickState.CurrentHand);
        Assert.AreEqual(2, newTrickState.StartingPlayerIndex);
        Assert.AreEqual(2, newTrickState.LastPlayingPlayerIndex);
        Assert.IsTrue(newTrickState.PlayersPassed.All(passed => !passed));
        Assert.IsFalse(newTrickState.IsComplete);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void StartNewTrick_OnIncompleteTrick_ThrowsException()
    {
        // Arrange
        var trickState = new TrickState(0, 4);
        var hand = new SingleHand(new Card(CardRank.Ace, CardSuit.Hearts));
        var stateAfterPlay = trickState.WithPlayerPlay(hand, 0);

        // Act - trick is not complete
        stateAfterPlay.StartNewTrick(1);
    }

    [TestMethod]
    public void WithPlayerPlay_ResetsOtherPlayersPassStatus()
    {
        // Arrange
        var trickState = new TrickState(0, 4);
        var firstHand = new SingleHand(new Card(CardRank.Three, CardSuit.Hearts));
        var secondHand = new SingleHand(new Card(CardRank.Four, CardSuit.Hearts));

        var stateAfterFirstPlay = trickState.WithPlayerPlay(firstHand, 0);
        var stateAfterPass1 = stateAfterFirstPlay.WithPlayerPass(1);
        var stateAfterPass2 = stateAfterPass1.WithPlayerPass(2);

        // Act - Player 3 plays, should reset pass status for others
        var stateAfterSecondPlay = stateAfterPass2.WithPlayerPlay(secondHand, 3);

        // Assert
        Assert.IsFalse(stateAfterSecondPlay.PlayersPassed[0]); // Reset
        Assert.IsFalse(stateAfterSecondPlay.PlayersPassed[1]); // Reset
        Assert.IsFalse(stateAfterSecondPlay.PlayersPassed[2]); // Reset
        Assert.IsFalse(stateAfterSecondPlay.PlayersPassed[3]); // Current player
    }
}
