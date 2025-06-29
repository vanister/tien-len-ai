using System.Collections.Immutable;
using TienLenAi2.Core.Cards;
using TienLenAi2.Core.Decks;
using TienLenAi2.Core.Hands;
using TienLenAi2.Core.States;
using TienLenAi2.Core.States.Game;
using TienLenAi2.Core.States.Players;

namespace TienLenAi2.Core.Engine;

public class GameEngine
{
    private readonly Store _store;

    public GameEngine(Store store)
    {
        ArgumentNullException.ThrowIfNull(store);
        _store = store;
    }

    public RootState CurrentState => _store.CurrentState;

    #region Game Commands

    public void AddPlayers(int playerCount = 4)
    {
        if (CurrentState.Game.Phase > GamePhase.AddPlayer)
        {
            throw new InvalidOperationException("Cannot add players after the game has started");
        }
        // Validate player count
        if (playerCount < 2 || playerCount > 4)
        {
            throw new ArgumentException("Player count must be between 2 and 4", nameof(playerCount));
        }

        // check to make sure we're not adding more than 4 total players if players already exist
        if (CurrentState.Players.TotalPlayers + playerCount > 4)
        {
            throw new InvalidOperationException("Cannot add more than 4 players in total");
        }

        var action = AddPlayersAction.Add(playerCount);
        _store.Dispatch(action);
    }

    public void DealCards(int cardsToDeal = 13, ImmutableList<Card>? shuffledDeck = null, int seed = 0)
    {
        if (CurrentState.Game.Phase > GamePhase.Dealing)
        {
            throw new InvalidOperationException("Cannot deal cards after the game has started");
        }

        // Validate we have players
        if (CurrentState.Players.TotalPlayers == 0)
        {
            throw new InvalidOperationException("Cannot deal cards when no players exist");
        }

        var deck = shuffledDeck ?? StandardDeck.CreateShuffled(seed);
        var playerIds = CurrentState.Players.Ids.OrderBy(id => id).ToImmutableList();
        var totalCardsNeeded = playerIds.Count * cardsToDeal;

        if (deck.Count < totalCardsNeeded)
        {
            throw new ArgumentException($"Not enough cards in deck. Need {totalCardsNeeded}, but deck has {deck.Count}");
        }

        // 2. Deal cards to each player
        var playerCardActions = deck
            .Take(playerIds.Count * cardsToDeal)
            .Select((card, index) => new
            {
                // Assign card to player in round-robin fashion
                PlayerId = playerIds[index % playerIds.Count],
                Card = card
            })
            .GroupBy(p => p.PlayerId)
            .Select(group => new UpdatePlayerCardsAction(
                PlayerActionTypes.UpdatePlayerCards,
                group.Key,
                [.. group.Select(x => x.Card)]
            ));

        // Dispatch each player's card update
        foreach (var action in playerCardActions)
        {
            _store.Dispatch(action);
        }
    }

    public void StartGame()
    {
        // check if the game is already started
        if (CurrentState.Game.Phase > GamePhase.Starting)
        {
            throw new InvalidOperationException("Game has already started");
        }

        // Validate we have players
        if (CurrentState.Players.TotalPlayers == 0)
        {
            throw new InvalidOperationException("Cannot start game when no players exist");
        }

        // todo -  check that all players have cards

        var startingPlayerId = PlayerSelectors.FindPlayerWith3OfSpades(CurrentState)
            ?? throw new InvalidOperationException("Cannot setup game without a player having 3 of Spades");

        var startAction = new StartGameAction(GameActionTypes.StartGame, startingPlayerId);
        _store.Dispatch(startAction);
    }

    public void PlayHand(int playerId, Hand hand)
    {
        var cards = hand.Cards;
        var handType = hand.Type;

        if (CurrentState.Game.CurrentPlayerId != playerId)
        {
            throw new InvalidOperationException($"It's not player {playerId}'s turn");
        }

        if (CurrentState.Game.Phase != GamePhase.Playing)
        {
            throw new InvalidOperationException($"Cannot play hand when not in 'Playing' phase. Current phase: {CurrentState.Game.Phase}");
        }

        var player = PlayerSelectors.FindPlayerById(CurrentState, playerId)
            ?? throw new ArgumentException($"Player with ID {playerId} does not exist", nameof(playerId));

        if (!cards.All(player.Cards.Contains))
        {
            var missingCards = cards.Except(player.Cards);
            throw new ArgumentException($"Player does not have all the cards in the provided hand. Missing {string.Join(", ", missingCards)}", nameof(hand));
        }

        HandFactory.TryCreateHand(cards, handType, out var validHand);

        if (validHand == null)
        {
            throw new ArgumentException($"Invalid hand. Cannot create hand from provided cards. Cards: {string.Join(",", hand.Cards)}", nameof(hand));
        }

        var IsFirstGame = GameSelectors.IsFirstGame(CurrentState);

        if (IsFirstGame && !validHand.ContainsThreeOfSpades)
        {
            throw new InvalidOperationException("In the first game, the hand must contain the 3 of Spades");
        }

        // make sure hand can beat the current hand on the table
        if (CurrentState.Game.CurrentTrick != null && !validHand.CanBeat(CurrentState.Game.CurrentTrick.Hand))
        {
            throw new InvalidOperationException("The hand played cannot beat the current hand on the table");
        }

        // dispatch the action to play the hand on the game state
        var gameAction = new PlayHandAction(GameActionTypes.PlayHand, playerId, validHand);
        _store.Dispatch(gameAction);

        // dispatch an action to remove the cards from the player's dealt hand
        var playerAction = new RemovePlayerCardsAction(PlayerActionTypes.RemovePlayerCards, playerId, validHand.Cards);
        _store.Dispatch(playerAction);
    }

    public void Pass(int playerId)
    {
        if (CurrentState.Game.CurrentPlayerId != playerId)
        {
            throw new InvalidOperationException($"It's not player {playerId}'s turn");
        }

        if (CurrentState.Game.Phase != GamePhase.Playing)
        {
            throw new InvalidOperationException($"Cannot pass when not in 'Playing' phase. Current phase: {CurrentState.Game.Phase}");
        }

        var totalPlayers = CurrentState.Players.TotalPlayers;
        var action = new PassAction(GameActionTypes.Pass, playerId, totalPlayers);

        _store.Dispatch(action);
    }

    public void UpdateGamePhase(GamePhase phase)
    {
        _store.Dispatch(new UpdateGamePhaseAction(GameActionTypes.UpdateGamePhase, phase));
    }

    public void NextTurn()
    {
        if (CurrentState.Game.Phase != GamePhase.Playing)
        {
            throw new InvalidOperationException("Cannot proceed to next turn when the game is not in progress");
        }

        var totalPlayers = CurrentState.Players.TotalPlayers;
        var action = new NextTurnAction(GameActionTypes.NextTurn, totalPlayers);

        _store.Dispatch(action);
    }

    public void EndGame(int winningPlayerId)
    {
        if (CurrentState.Game.Phase != GamePhase.Playing)
        {
            throw new InvalidOperationException("Cannot end game when the game is not in progress");
        }

        var winnerAction = new UpdateWinnerAction(GameActionTypes.UpdateWinner, winningPlayerId);
        _store.Dispatch(winnerAction);
    }

    public void NewGame(int? winningPlayerId = null)
    {
        if (CurrentState.Game.Phase == GamePhase.NotStarted)
        {
            throw new InvalidOperationException("Cannot start a new game when the current game has not started");
        }

        // Reset the game state to default
        _store.Dispatch(new NewGameAction(GameActionTypes.NewGame, winningPlayerId));
    }

    #endregion

    #region Game Queries

    public (int Id, string PlayerName)? CheckForWinner()
    {
        PlayerSelectors.TryFindWinner(CurrentState, out var winningPlayer);

        if (winningPlayer == null)
        {
            return null; // No winner found
        }

        var winnerAction = new UpdateWinnerAction(GameActionTypes.UpdateWinner, winningPlayer.Id);

        _store.Dispatch(winnerAction);

        return (winningPlayer.Id, winningPlayer.Name);
    }

    #endregion
}
