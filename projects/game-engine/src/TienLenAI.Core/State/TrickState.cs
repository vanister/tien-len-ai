using TienLenAI.Core.Cards;
using TienLenAI.Core.Hands;

namespace TienLenAI.Core.State;

/// <summary>
/// Immutable representation of the current trick state in a Tiến Lên game.
/// Tracks what hand type is required, current cards to beat, and player pass status.
/// </summary>
public class TrickState
{
    public HandType? RequiredHandType { get; }
    public Hand? CurrentHand { get; }
    public int StartingPlayerIndex { get; }
    public int LastPlayingPlayerIndex { get; }
    public IReadOnlyList<bool> PlayersPassed { get; }
    public bool IsComplete { get; }

    /// <summary>
    /// Creates a new trick state for the start of a trick
    /// </summary>
    public TrickState(int startingPlayerIndex, int totalPlayers)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(totalPlayers);

        if (startingPlayerIndex < 0 || startingPlayerIndex >= totalPlayers)
        {
            throw new ArgumentOutOfRangeException(nameof(startingPlayerIndex));
        }

        RequiredHandType = null;
        CurrentHand = null;
        StartingPlayerIndex = startingPlayerIndex;
        LastPlayingPlayerIndex = startingPlayerIndex;
        PlayersPassed = new bool[totalPlayers];
        IsComplete = false;
    }

    /// <summary>
    /// Private constructor for creating new instances with updated state
    /// </summary>
    private TrickState(
        HandType? requiredHandType,
        Hand? currentHand,
        int startingPlayerIndex,
        int lastPlayingPlayerIndex,
        IReadOnlyList<bool> playersPassed,
        bool isComplete)
    {
        RequiredHandType = requiredHandType;
        CurrentHand = currentHand;
        StartingPlayerIndex = startingPlayerIndex;
        LastPlayingPlayerIndex = lastPlayingPlayerIndex;
        PlayersPassed = playersPassed;
        IsComplete = isComplete;
    }

    /// <summary>
    /// Creates a new TrickState with a player's hand played
    /// </summary>
    public TrickState WithPlayerPlay(Hand hand, int playerIndex)
    {
        ArgumentNullException.ThrowIfNull(hand);

        if (playerIndex < 0 || playerIndex >= PlayersPassed.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(playerIndex));
        }

        if (IsComplete)
        {
            throw new InvalidOperationException("Cannot play on a completed trick");
        }

        if (PlayersPassed[playerIndex])
        {
            throw new InvalidOperationException("Player has already passed this trick");
        }

        // Validate the play is legal
        if (!IsLegalPlay(hand))
        {
            throw new InvalidOperationException(
                $"Illegal play: {hand.Type} does not match required type or beat current hand");
        }

        // Reset all other players' pass status (they get a chance to play again)
        var newPlayersPassed = new bool[PlayersPassed.Count];
        newPlayersPassed[playerIndex] = false; // This player just played

        // Check if trick is now complete (all other players would need to pass)
        var isNowComplete = CheckTrickComplete(newPlayersPassed, playerIndex);

        return new TrickState(
            hand.Type,
            hand,
            StartingPlayerIndex,
            playerIndex,
            newPlayersPassed,
            isNowComplete);
    }

    /// <summary>
    /// Creates a new TrickState with a player passing
    /// </summary>
    public TrickState WithPlayerPass(int playerIndex)
    {
        if (playerIndex < 0 || playerIndex >= PlayersPassed.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(playerIndex));
        }

        if (IsComplete)
        {
            throw new InvalidOperationException("Cannot pass on a completed trick");
        }

        if (PlayersPassed[playerIndex])
        {
            throw new InvalidOperationException("Player has already passed this trick");
        }

        // Update pass status
        var newPlayersPassed = PlayersPassed.ToArray();
        newPlayersPassed[playerIndex] = true;

        // Check if trick is now complete
        var isNowComplete = CheckTrickComplete(newPlayersPassed, LastPlayingPlayerIndex);

        return new TrickState(
            RequiredHandType,
            CurrentHand,
            StartingPlayerIndex,
            LastPlayingPlayerIndex,
            newPlayersPassed,
            isNowComplete);
    }

    /// <summary>
    /// Validates if a hand can be legally played in this trick state
    /// </summary>
    public bool IsLegalPlay(Hand hand)
    {
        if (hand == null)
        {
            return false;
        }

        // First play of the trick - any valid hand is allowed
        if (CurrentHand == null)
        {
            return hand.IsValid();
        }

        // Must match the required hand type
        if (hand.Type != RequiredHandType)
        {
            // Exception: Bombs can beat any non-bomb hand
            if (hand.Type == HandType.Bomb && CurrentHand.Type != HandType.Bomb)
            {
                return hand.IsValid();
            }
            return false;
        }

        // Must beat the current hand
        return hand.IsValid() && hand.CompareTo(CurrentHand) > 0;
    }

    /// <summary>
    /// Gets all players who can still play (haven't passed)
    /// </summary>
    public IEnumerable<int> GetActivePlayers()
    {
        if (IsComplete)
        {
            return Enumerable.Empty<int>();
        }

        return PlayersPassed
            .Select((hasPassed, index) => new { HasPassed = hasPassed, Index = index })
            .Where(x => !x.HasPassed)
            .Select(x => x.Index);
    }

    /// <summary>
    /// Gets the player who should play next in turn order
    /// </summary>
    public int? GetNextPlayer(int currentPlayerIndex)
    {
        if (IsComplete)
        {
            return null;
        }

        var totalPlayers = PlayersPassed.Count;

        for (int i = 1; i < totalPlayers; i++)
        {
            var nextPlayerIndex = (currentPlayerIndex + i) % totalPlayers;

            if (!PlayersPassed[nextPlayerIndex])
            {
                return nextPlayerIndex;
            }
        }

        return null; // No active players found
    }

    /// <summary>
    /// Checks if the trick is complete (all players except the last playing player have passed)
    /// </summary>
    private bool CheckTrickComplete(bool[] playersPassed, int lastPlayingPlayerIndex)
    {
        // If no one has played yet, trick can't be complete
        if (CurrentHand == null && RequiredHandType == null)
        {
            return false;
        }

        // Count how many players have passed
        var passedCount = playersPassed.Count(passed => passed);
        var totalPlayers = playersPassed.Length;

        // Trick is complete when all players except the last playing player have passed
        // This means (totalPlayers - 1) players have passed, leaving only the last player active
        return passedCount >= totalPlayers - 1;
    }

    /// <summary>
    /// Creates a new trick state for the winner to start the next trick
    /// </summary>
    public TrickState StartNewTrick(int winningPlayerIndex)
    {
        if (!IsComplete)
        {
            throw new InvalidOperationException("Cannot start new trick before current trick is complete");
        }

        return new TrickState(winningPlayerIndex, PlayersPassed.Count);
    }
}
