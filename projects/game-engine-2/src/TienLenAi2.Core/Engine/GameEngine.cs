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

    public void AddPlayers(int playerCount = 4)
    {
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
        // Validate we have players
        if (CurrentState.Players.TotalPlayers == 0)
        {
            throw new InvalidOperationException("Cannot deal cards when no players exist");
        }

        var deck = shuffledDeck ?? StandardDeck.CreateShuffled(seed);
        var playerIds = CurrentState.Players.Ids.OrderBy(id => id).ToList();
        var totalCardsNeeded = playerIds.Count * cardsToDeal;

        if (deck.Count < totalCardsNeeded)
        {
            throw new ArgumentException($"Not enough cards in deck. Need {totalCardsNeeded}, but deck has {deck.Count}");
        }

        // 1. Update game phase to Dealing
        UpdateGamePhase(GamePhase.Dealing);

        // 2. Deal cards to each player
        var playerCardActions = deck
            .Take(playerIds.Count * cardsToDeal)
            .Select((card, index) => new
            {
                // Assign card to player in round-robin fashion
                PlayerId = playerIds[index % playerIds.Count],
                Card = card
            })
            .GroupBy(x => x.PlayerId)
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

    public void SetupGame()
    {
        // check if the game is already started
        if (CurrentState.Game.Phase > GamePhase.Setup)
        {
            throw new InvalidOperationException("Game has already started");
        }

        // Validate we have players
        if (CurrentState.Players.TotalPlayers == 0)
        {
            throw new InvalidOperationException("Cannot start game when no players exist");
        }

        // todo -  check that we have a deck and players have cards

        var startingPlayerId = PlayerSelectors.FindPlayerWith3OfSpades(CurrentState)
            ?? throw new InvalidOperationException("Cannot setup game without a player having 3 of Spades");

        var setupAction = new SetupGameAction(GameActionTypes.SetupGame, startingPlayerId);

        _store.Dispatch(setupAction);
    }

    public bool PlayHand(int playerId, IEnumerable<Card> cards, HandType handType)
    {
        if (CurrentState.Game.CurrentPlayerId != playerId)
        {
            throw new InvalidOperationException($"It's not player {playerId}'s turn");
        }

        if (CurrentState.Game.Phase != GamePhase.Playing)
        {
            throw new InvalidOperationException($"Cannot play hand when not in 'Playing' phase. Current phase: ${CurrentState.Game.Phase}");
        }

        var player = PlayerSelectors.FindPlayerById(CurrentState, playerId)
            ?? throw new ArgumentException($"Player with ID {playerId} does not exist", nameof(playerId));

        if (!cards.All(player.Cards.Contains))
        {
            var missingCards = cards.Except(player.Cards);
            throw new ArgumentException($"Player does not have all the cards in the provided hand. Missing ${string.Join(", ", missingCards)}", nameof(cards));
        }

        HandFactory.TryCreateHand(cards, handType, out var validHand);

        if (validHand == null)
        {
            throw new ArgumentException("Invalid hand. Cannot create hand from provided cards.", nameof(cards));
        }

        // dispatch the action to play the hand on the game state
        var gameAction = new PlayHandAction(
            GameActionTypes.PlayHand,
            playerId,
            validHand
        );

        _store.Dispatch(gameAction);

        // dispatch an action to remove the cards from the player's dealt hand
        var playerAction = new RemovePlayerCardsAction(
            PlayerActionTypes.RemovePlayerCards,
            playerId,
            validHand.Cards
        );

        _store.Dispatch(playerAction);

        return true;
    }

    private void UpdateGamePhase(GamePhase phase)
    {
        _store.Dispatch(new UpdateGamePhaseAction(GameActionTypes.UpdateGamePhase, phase));
    }
}
