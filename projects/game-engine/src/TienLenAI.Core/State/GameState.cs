using TienLenAI.Core.Cards;
using TienLenAI.Core.Hands;

namespace TienLenAI.Core.State;

public class GameState
{
    public IReadOnlyList<Player> Players { get; private set; }
    public int CurrentPlayerIndex { get; private set; }
    public TrickState CurrentTrick { get; private set; }
    public IReadOnlyList<TrickState> TrickHistory { get; private set; }
    public bool IsGameComplete { get; private set; }
    public List<int> FinishOrder { get; private set; }
    public bool IsFirstGame { get; private set; }
    public bool HasFirstPlayBeenMade { get; private set; }
    public int TrickNumber { get; private set; }

    public GameState(List<Player> players, bool isFirstGame = true)
    {
        if (players == null || players.Count != 4)
        {
            throw new ArgumentException("Game requires exactly 4 players", nameof(players));
        }

        Players = players.AsReadOnly();
        IsFirstGame = isFirstGame;
        FinishOrder = [];
        TrickHistory = [];
        IsGameComplete = false;
        HasFirstPlayBeenMade = false;
        TrickNumber = 1;

        // Set starting player based on game type
        CurrentPlayerIndex = DetermineStartingPlayer();
        CurrentTrick = new TrickState(CurrentPlayerIndex, Players.Count);
    }

    /// <summary>
    /// Gets the current player whose turn it is
    /// </summary>
    public Player GetCurrentPlayer()
    {
        return Players[CurrentPlayerIndex];
    }

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
        var updatedPlayers = UpdatePlayerHand(CurrentPlayerIndex, newHand);

        // Update trick state
        var newTrickState = CurrentTrick.WithPlayerPlay(hand, CurrentPlayerIndex);

        // Check if player finished
        var newFinishOrder = new List<int>(FinishOrder);
        if (newHand.Count == 0)
        {
            newFinishOrder.Add(CurrentPlayerIndex);
        }

        // Determine next player and update history
        var (nextPlayerIndex, newTrickHistory, newTrickNumber, updatedTrickState) = DetermineNextPlayerAndUpdateHistory(newTrickState, newFinishOrder);

        return new GameState(
            updatedPlayers,
            nextPlayerIndex,
            updatedTrickState,
            newTrickHistory,
            newFinishOrder,
            IsFirstGame,
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

        if (CurrentTrick.CurrentHand == null)
        {
            throw new InvalidOperationException("Cannot pass when no hand has been played");
        }

        // Update trick state with pass
        var newTrickState = CurrentTrick.WithPlayerPass(CurrentPlayerIndex);

        // Determine next player and update history
        var (nextPlayerIndex, newTrickHistory, newTrickNumber, updatedTrickState) = DetermineNextPlayerAndUpdateHistory(newTrickState, FinishOrder);

        return new GameState(
            Players.ToList(),
            nextPlayerIndex,
            updatedTrickState,
            newTrickHistory,
            FinishOrder,
            IsFirstGame,
            HasFirstPlayBeenMade,
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
        if (IsFirstGame && !HasFirstPlayBeenMade)
        {
            var threeOfSpades = new Card(CardRank.Three, CardSuit.Spades);
            if (!hand.Cards.Contains(threeOfSpades))
            {
                return false;
            }
        }

        // Check trick-level validity
        return CurrentTrick.IsLegalPlay(hand);
    }

    /// <summary>
    /// Gets all players who can still play (haven't finished and haven't passed)
    /// </summary>
    public IEnumerable<int> GetActivePlayers()
    {
        return CurrentTrick.GetActivePlayers()
            .Where(playerIndex => !FinishOrder.Contains(playerIndex));
    }

    /// <summary>
    /// Gets all completed tricks in chronological order
    /// </summary>
    public IEnumerable<TrickState> GetCompletedTricks()
    {
        return TrickHistory;
    }

    /// <summary>
    /// Gets the last completed trick, or null if no tricks have been completed
    /// </summary>
    public TrickState? GetLastCompletedTrick()
    {
        return TrickHistory.LastOrDefault();
    }

    /// <summary>
    /// Gets all hands played by a specific player across all completed tricks
    /// </summary>
    public IEnumerable<Hand> GetPlayerHandHistory(int playerIndex)
    {
        if (playerIndex < 0 || playerIndex >= Players.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(playerIndex));
        }

        var hands = new List<Hand>();
        
        foreach (var trick in TrickHistory)
        {
            if (trick.LastPlayingPlayerIndex == playerIndex && trick.CurrentHand != null)
            {
                hands.Add(trick.CurrentHand);
            }
        }

        // Add current trick if this player has played
        if (CurrentTrick.LastPlayingPlayerIndex == playerIndex && CurrentTrick.CurrentHand != null)
        {
            hands.Add(CurrentTrick.CurrentHand);
        }

        return hands;
    }

    /// <summary>
    /// Gets statistics about trick winners
    /// </summary>
    public Dictionary<int, int> GetTrickWinCounts()
    {
        var winCounts = new Dictionary<int, int>();
        
        for (int i = 0; i < Players.Count; i++)
        {
            winCounts[i] = 0;
        }

        foreach (var trick in TrickHistory.Where(t => t.IsComplete))
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
        
        for (int i = 0; i < Players.Count; i++)
        {
            cardCounts[i] = 0;
        }

        foreach (var trick in TrickHistory)
        {
            if (trick.CurrentHand != null)
            {
                cardCounts[trick.LastPlayingPlayerIndex] += trick.CurrentHand.Cards.Count;
            }
        }

        // Add current trick if someone has played
        if (CurrentTrick.CurrentHand != null)
        {
            cardCounts[CurrentTrick.LastPlayingPlayerIndex] += CurrentTrick.CurrentHand.Cards.Count;
        }

        return cardCounts;
    }

    /// <summary>
    /// Gets all hand types that have been played in the game
    /// </summary>
    public IEnumerable<HandType> GetPlayedHandTypes()
    {
        var handTypes = new HashSet<HandType>();

        foreach (var trick in TrickHistory)
        {
            if (trick.CurrentHand != null)
            {
                handTypes.Add(trick.CurrentHand.Type);
            }
        }

        if (CurrentTrick.CurrentHand != null)
        {
            handTypes.Add(CurrentTrick.CurrentHand.Type);
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
        IReadOnlyList<TrickState> trickHistory,
        List<int> finishOrder,
        bool isFirstGame,
        bool hasFirstPlayBeenMade,
        int trickNumber)
    {
        Players = players.AsReadOnly();
        CurrentPlayerIndex = currentPlayerIndex;
        CurrentTrick = currentTrick;
        TrickHistory = trickHistory;
        FinishOrder = finishOrder;
        IsFirstGame = isFirstGame;
        HasFirstPlayBeenMade = hasFirstPlayBeenMade;
        TrickNumber = trickNumber;
        IsGameComplete = finishOrder.Count >= 3; // Game ends when 3 players finish
    }

    private int DetermineStartingPlayer()
    {
        if (!IsFirstGame)
        {
            // Winner of previous game starts (this would be passed in constructor in real implementation)
            return 0; // Placeholder - should be winner from previous game
        }

        // First game: find player with 3♠
        var threeOfSpades = new Card(CardRank.Three, CardSuit.Spades);
        for (int i = 0; i < Players.Count; i++)
        {
            if (Players[i].Hand.Contains(threeOfSpades))
            {
                return i;
            }
        }

        throw new InvalidOperationException("No player has 3♠ - invalid game setup");
    }

    private (int nextPlayerIndex, IReadOnlyList<TrickState> trickHistory, int trickNumber, TrickState currentTrick) DetermineNextPlayerAndUpdateHistory(TrickState trickState, List<int> finishOrder)
    {
        var newTrickHistory = TrickHistory.ToList();
        var newTrickNumber = TrickNumber;

        if (trickState.IsComplete)
        {
            // Trick completed - add to history and start new trick
            newTrickHistory.Add(trickState);
            var winner = trickState.LastPlayingPlayerIndex;
            var newTrick = trickState.StartNewTrick(winner);
            newTrickNumber++;
            return (winner, newTrickHistory.AsReadOnly(), newTrickNumber, newTrick);
        }

        // Find next active player who hasn't finished
        var nextPlayer = trickState.GetNextPlayer(CurrentPlayerIndex);
        while (nextPlayer.HasValue && finishOrder.Contains(nextPlayer.Value))
        {
            nextPlayer = trickState.GetNextPlayer(nextPlayer.Value);
        }

        return (nextPlayer ?? CurrentPlayerIndex, newTrickHistory.AsReadOnly(), newTrickNumber, trickState);
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
        
        for (int i = 0; i < Players.Count; i++)
        {
            if (i == playerIndex)
            {
                updatedPlayers.Add(new Player(Players[i].Name, newHand));
            }
            else
            {
                updatedPlayers.Add(Players[i]);
            }
        }
        
        return updatedPlayers;
    }
}
