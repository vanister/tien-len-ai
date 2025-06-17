using TienLenAI.Core.Cards;
using TienLenAI.Core.Hands;

namespace TienLenAI.Core.State;

public class GameState
{
    // Private implementation details
    private List<Player> players;
    private int currentPlayerIndex;
    private TrickState currentTrick;
    private List<int> finishOrder;
    private bool isFirstGame;
    private bool hasFirstPlayBeenMade;
    private List<TrickState> trickHistory;

    // Essential public state
    public bool IsGameComplete { get; private set; }
    public int TrickNumber { get; private set; }

    public GameState(List<Player> players, bool isFirstGame = true)
    {
        if (players == null || players.Count != 4)
        {
            throw new ArgumentException("Game requires exactly 4 players", nameof(players));
        }

        this.players = players;
        this.isFirstGame = isFirstGame;
        this.finishOrder = [];
        this.trickHistory = [];
        IsGameComplete = false;
        hasFirstPlayBeenMade = false;
        TrickNumber = 1;

        // Set starting player based on game type
        currentPlayerIndex = DetermineStartingPlayer();
        currentTrick = new TrickState(currentPlayerIndex, this.players.Count);
    }

    /// <summary>
    /// Gets the current player whose turn it is
    /// </summary>
    public Player GetCurrentPlayer()
    {
        return players[currentPlayerIndex];
    }

    /// <summary>
    /// Gets a specific player by index
    /// </summary>
    public Player GetPlayer(int index)
    {
        if (index < 0 || index >= players.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }
        return players[index];
    }

    /// <summary>
    /// Gets the total number of players
    /// </summary>
    public int PlayerCount => players.Count;

    /// <summary>
    /// Gets the current player's index
    /// </summary>
    public int CurrentPlayerIndex => currentPlayerIndex;

    /// <summary>
    /// Gets the current trick state
    /// </summary>
    public TrickState CurrentTrick => currentTrick;

    /// <summary>
    /// Gets the finish order (readonly)
    /// </summary>
    public IReadOnlyList<int> FinishOrder => finishOrder.AsReadOnly();

    /// <summary>
    /// Attempts to play a hand for the current player
    /// </summary>
    public GameState PlayHand(Hand hand)
    {
        if (IsGameComplete)
        {
            throw new InvalidOperationException("Cannot play on a completed game");
        }

        // Validate the play
        if (!IsValidPlay(hand))
        {
            throw new InvalidOperationException("Invalid play for current game state");
        }

        // Remove cards from player's hand
        var currentPlayer = GetCurrentPlayer();
        var newHand = RemoveCardsFromHand(currentPlayer.Hand, hand.Cards);
        var updatedPlayers = UpdatePlayerHand(currentPlayerIndex, newHand);

        // Update trick state
        var newTrickState = currentTrick.WithPlayerPlay(hand, currentPlayerIndex);

        // Check if player finished
        var newFinishOrder = new List<int>(finishOrder);
        if (newHand.Count == 0)
        {
            newFinishOrder.Add(currentPlayerIndex);
        }

        // Update trick history if needed
        var (updatedTrickState, newTrickHistory, newTrickNumber) = UpdateTrickHistory(newTrickState);

        // Determine next player
        var nextPlayerIndex = DetermineNextPlayer(updatedTrickState, newFinishOrder);

        return new GameState(
            updatedPlayers,
            nextPlayerIndex,
            updatedTrickState,
            newTrickHistory,
            newFinishOrder,
            isFirstGame,
            true,
            newTrickNumber);
    }

    /// <summary>
    /// Current player passes their turn
    /// </summary>
    public GameState PassTurn()
    {
        if (IsGameComplete)
        {
            throw new InvalidOperationException("Cannot pass on a completed game");
        }
        if (currentTrick.CurrentHand == null)
        {
            throw new InvalidOperationException("Cannot pass when no hand has been played");
        }

        // Update trick state with pass
        var newTrickState = currentTrick.WithPlayerPass(currentPlayerIndex);

        // Update trick history if needed
        var (updatedTrickState, newTrickHistory, newTrickNumber) = UpdateTrickHistory(newTrickState);

        // Determine next player
        var nextPlayerIndex = DetermineNextPlayer(updatedTrickState, finishOrder);

        return new GameState(
            players,
            nextPlayerIndex,
            updatedTrickState,
            newTrickHistory,
            finishOrder,
            isFirstGame,
            hasFirstPlayBeenMade,
            newTrickNumber);
    }

    /// <summary>
    /// Validates if a hand can be played in the current game state
    /// </summary>
    public bool IsValidPlay(Hand hand)
    {
        if (hand == null || !hand.IsValid())
        {
            return false;
        }

        // Check if player has all the cards
        var currentPlayer = GetCurrentPlayer();
        if (!HasAllCards(currentPlayer.Hand, hand.Cards))
        {
            return false;
        }

        // First play must include 3♠ if it's the first game and first play
        if (isFirstGame && !hasFirstPlayBeenMade)
        {
            var threeOfSpades = new Card(CardRank.Three, CardSuit.Spades);
            if (!hand.Cards.Contains(threeOfSpades))
            {
                return false;
            }
        }

        // Check trick-level validity
        return currentTrick.IsLegalPlay(hand);
    }

    /// <summary>
    /// Gets all players who can still play (haven't finished and haven't passed)
    /// </summary>
    public IEnumerable<int> GetActivePlayers()
    {
        return currentTrick.GetActivePlayers()
            .Where(playerIndex => !finishOrder.Contains(playerIndex));
    }

    /// <summary>
    /// Gets all completed tricks in chronological order
    /// </summary>
    public IEnumerable<TrickState> GetCompletedTricks()
    {
        return trickHistory;
    }

    /// <summary>
    /// Gets the last completed trick, or null if no tricks have been completed
    /// </summary>
    public TrickState? GetLastCompletedTrick()
    {
        return trickHistory.LastOrDefault();
    }

    /// <summary>
    /// Gets all hands played by a specific player across all completed tricks
    /// </summary>
    public IEnumerable<Hand> GetPlayerHandHistory(int playerIndex)
    {
        if (playerIndex < 0 || playerIndex >= players.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(playerIndex));
        }

        var hands = new List<Hand>();

        foreach (var trick in trickHistory)
        {
            if (trick.LastPlayingPlayerIndex == playerIndex && trick.CurrentHand != null)
            {
                hands.Add(trick.CurrentHand);
            }
        }

        // Add current trick if this player has played
        if (currentTrick.LastPlayingPlayerIndex == playerIndex && currentTrick.CurrentHand != null)
        {
            hands.Add(currentTrick.CurrentHand);
        }

        return hands;
    }

    /// <summary>
    /// Gets statistics about trick winners
    /// </summary>
    public Dictionary<int, int> GetTrickWinCounts()
    {
        var winCounts = new Dictionary<int, int>();

        for (int i = 0; i < players.Count; i++)
        {
            winCounts[i] = 0;
        }

        foreach (var trick in trickHistory.Where(t => t.IsComplete))
        {
            winCounts[trick.LastPlayingPlayerIndex]++;
        }

        return winCounts;
    }

    /// <summary>
    /// Gets the total number of cards played by each player
    /// </summary>
    public Dictionary<int, int> GetCardsPlayedCounts()
    {
        var cardCounts = new Dictionary<int, int>();

        for (int i = 0; i < players.Count; i++)
        {
            cardCounts[i] = 0;
        }

        foreach (var trick in trickHistory)
        {
            if (trick.CurrentHand != null)
            {
                cardCounts[trick.LastPlayingPlayerIndex] += trick.CurrentHand.Cards.Count;
            }
        }

        // Add current trick if someone has played
        if (currentTrick.CurrentHand != null)
        {
            cardCounts[currentTrick.LastPlayingPlayerIndex] += currentTrick.CurrentHand.Cards.Count;
        }

        return cardCounts;
    }

    /// <summary>
    /// Gets all hand types that have been played in the game
    /// </summary>
    public IEnumerable<HandType> GetPlayedHandTypes()
    {
        var handTypes = new HashSet<HandType>();

        foreach (var trick in trickHistory)
        {
            if (trick.CurrentHand != null)
            {
                handTypes.Add(trick.CurrentHand.Type);
            }
        }

        if (currentTrick.CurrentHand != null)
        {
            handTypes.Add(currentTrick.CurrentHand.Type);
        }

        return handTypes;
    }

    /// <summary>
    /// Private constructor for creating new state instances
    /// </summary>
    private GameState(
        List<Player> players,
        int currentPlayerIndex,
        TrickState currentTrick,
        List<TrickState> trickHistory,
        List<int> finishOrder,
        bool isFirstGame,
        bool hasFirstPlayBeenMade,
        int trickNumber)
    {
        this.players = players;
        this.currentPlayerIndex = currentPlayerIndex;
        this.currentTrick = currentTrick;
        this.trickHistory = trickHistory;
        this.finishOrder = finishOrder;
        this.isFirstGame = isFirstGame;
        this.hasFirstPlayBeenMade = hasFirstPlayBeenMade;
        TrickNumber = trickNumber;
        IsGameComplete = finishOrder.Count >= 3; // Game ends when 3 players finish
    }

    private int DetermineStartingPlayer()
    {
        if (!isFirstGame)
        {
            // Winner of previous game starts (this would be passed in constructor in real implementation)
            return 0; // Placeholder - should be winner from previous game
        }

        // First game: find player with 3♠
        var threeOfSpades = new Card(CardRank.Three, CardSuit.Spades);
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].Hand.Contains(threeOfSpades))
            {
                return i;
            }
        }

        throw new InvalidOperationException("No player has 3♠ - invalid game setup");
    }

    /// <summary>
    /// Updates trick history when a trick is completed
    /// </summary>
    private (TrickState trickState, List<TrickState> trickHistory, int trickNumber) UpdateTrickHistory(TrickState trickState)
    {
        if (!trickState.IsComplete)
        {
            // Trick not complete, no history update needed
            return (trickState, trickHistory, TrickNumber);
        }

        // Trick completed - add to history and start new trick
        var newTrickHistory = trickHistory.ToList();
        newTrickHistory.Add(trickState);

        var winner = trickState.LastPlayingPlayerIndex;
        var newTrick = trickState.StartNewTrick(winner);

        return (newTrick, newTrickHistory, TrickNumber + 1);
    }

    /// <summary>
    /// Determines the next player to play
    /// </summary>
    private int DetermineNextPlayer(TrickState trickState, IReadOnlyList<int> finishOrder)
    {
        if (trickState.IsComplete)
        {
            // Trick just completed, winner starts the new trick
            return trickState.LastPlayingPlayerIndex;
        }

        // Find next active player who hasn't finished
        var nextPlayer = trickState.GetNextPlayer(CurrentPlayerIndex);
        while (nextPlayer.HasValue && finishOrder.Contains(nextPlayer.Value))
        {
            nextPlayer = trickState.GetNextPlayer(nextPlayer.Value);
        }

        return nextPlayer ?? CurrentPlayerIndex;
    }

    private bool HasAllCards(List<Card> playerHand, IReadOnlyList<Card> requiredCards)
    {
        var handCopy = new List<Card>(playerHand);

        foreach (var card in requiredCards)
        {
            if (!handCopy.Remove(card))
            {
                return false;
            }
        }

        return true;
    }

    private List<Card> RemoveCardsFromHand(List<Card> hand, IReadOnlyList<Card> cardsToRemove)
    {
        var newHand = new List<Card>(hand);

        foreach (var card in cardsToRemove)
        {
            newHand.Remove(card);
        }

        return newHand;
    }
    private List<Player> UpdatePlayerHand(int playerIndex, List<Card> newHand)
    {
        var updatedPlayers = new List<Player>();

        for (int i = 0; i < players.Count; i++)
        {
            if (i == playerIndex)
            {
                updatedPlayers.Add(new Player(players[i].Name, newHand));
            }
            else
            {
                updatedPlayers.Add(players[i]);
            }
        }

        return updatedPlayers;
    }
}
