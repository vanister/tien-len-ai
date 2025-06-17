using TienLenAI.Core.Cards;
using TienLenAI.Core.Hands;

namespace TienLenAI.Core.State;

public class GameState
{
    public IReadOnlyList<Player> Players { get; private set; }
    public int CurrentPlayerIndex { get; private set; }
    public TrickState CurrentTrick { get; private set; }
    public bool IsGameComplete { get; private set; }
    public List<int> FinishOrder { get; private set; }
    public bool IsFirstGame { get; private set; }
    public bool HasFirstPlayBeenMade { get; private set; }

    public GameState(List<Player> players, bool isFirstGame = true)
    {
        if (players == null || players.Count != 4)
        {
            throw new ArgumentException("Game requires exactly 4 players", nameof(players));
        }

        Players = players.AsReadOnly();
        IsFirstGame = isFirstGame;
        FinishOrder = [];
        IsGameComplete = false;
        HasFirstPlayBeenMade = false;

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

        // Determine next player
        var nextPlayerIndex = DetermineNextPlayer(newTrickState, newFinishOrder);

        return new GameState(
            updatedPlayers,
            nextPlayerIndex,
            newTrickState,
            newFinishOrder,
            IsFirstGame,
            true);
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

        // Determine next player
        var nextPlayerIndex = DetermineNextPlayer(newTrickState, FinishOrder);

        return new GameState(
            Players.ToList(),
            nextPlayerIndex,
            newTrickState,
            FinishOrder,
            IsFirstGame,
            HasFirstPlayBeenMade);
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
    /// Private constructor for creating new state instances
    /// </summary>
    private GameState(
        List<Player> players,
        int currentPlayerIndex,
        TrickState currentTrick,
        List<int> finishOrder,
        bool isFirstGame,
        bool hasFirstPlayBeenMade)
    {
        Players = players.AsReadOnly();
        CurrentPlayerIndex = currentPlayerIndex;
        CurrentTrick = currentTrick;
        FinishOrder = finishOrder;
        IsFirstGame = isFirstGame;
        HasFirstPlayBeenMade = hasFirstPlayBeenMade;
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

    private int DetermineNextPlayer(TrickState trickState, List<int> finishOrder)
    {
        if (trickState.IsComplete)
        {
            // Trick completed - winner starts new trick
            var winner = trickState.LastPlayingPlayerIndex;
            var newTrick = trickState.StartNewTrick(winner);
            CurrentTrick = newTrick;
            return winner;
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
