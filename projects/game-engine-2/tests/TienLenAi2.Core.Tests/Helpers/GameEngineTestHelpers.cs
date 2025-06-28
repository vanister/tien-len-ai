using System.Collections.Immutable;
using TienLenAi2.Core.Cards;
using TienLenAi2.Core.Hands;
using TienLenAi2.Core.States;
using TienLenAi2.Core.States.Game;
using TienLenAi2.Core.States.Players;

namespace TienLenAi2.Core.Tests.Helpers;

public static class GameEngineTestHelpers
{
    public static ImmutableList<Card> CreateTestDeck()
    {
        // Create all cards in deterministic order (3♠ will be first naturally)
        var deck = new[] { Suit.Spades, Suit.Clubs, Suit.Diamonds, Suit.Hearts }
            .SelectMany(suit => Enum.GetValues<Rank>().Select(rank => new Card(rank, suit)));

        return [.. deck];
    }

    public static RootState CreateStateForDealCards()
    {
        // Create players state with 4 players
        var players = CreatePlayers();
        var playersState = new PlayersState(players);

        // Create game state in Dealing phase
        var gameState = GameState.CreateDefault() with
        {
            Phase = GamePhase.Dealing
        };

        return new RootState(gameState, playersState);
    }

    public static RootState CreateStateForStartGame(int playerCount = 4, int cardsPerPlayer = 13)
    {
        var playerCards = CreateCardsForPlayer(playerCount, cardsPerPlayer);
        var players = CreatePlayers(playerCards);
        var playersState = new PlayersState(players);

        var gameState = GameState.CreateDefault() with
        {
            Phase = GamePhase.Starting
        };

        return new RootState(gameState, playersState);
    }

    public static RootState CreateStateForPlayHand(int playerCount = 4, int cardsPerPlayer = 13)
    {
        var playerCards = CreateCardsForPlayer(playerCount, cardsPerPlayer);
        var players = CreatePlayers(playerCards);
        var playersState = new PlayersState(players);

        var gameState = GameState.CreateDefault() with
        {
            Phase = GamePhase.Playing,
            CurrentPlayerId = 1, // should be the player with the 3♠
            GameNumber = 1,
        };

        return new RootState(gameState, playersState);
    }

    public static RootState CreateStateForPassTurn(
        int playerCount = 4,
        int cardsPerPlayer = 13,
        int currentPlayerId = 1,
        ImmutableHashSet<int>? passedPlayers = null)
    {
        var playerCards = CreateCardsForPlayer(playerCount, cardsPerPlayer);
        var players = CreatePlayers(playerCards);
        var player1 = players.GetValueOrDefault(currentPlayerId)!;
        var hand = new Hand(HandType.Single, [Card.ThreeOfSpades]);

        // the game state for the very first game and hand
        var gameState = GameState.CreateDefault() with
        {
            Phase = GamePhase.Playing,
            CurrentPlayerId = player1.Id, // should be the player with the 3♠
            GameNumber = 1,
            TrickNumber = 1,
            StartingTrickPlayerId = player1.Id,
            CurrentHand = hand,
            PlayedHands = [hand],
            PlayersPassed = passedPlayers ?? []
        };

        // Player 1 has the 3♠ and plays it, so we need to remove it from their cards
        player1 = player1 with { Cards = player1.Cards.Remove(Card.ThreeOfSpades) };

        var updatedPlayers = players.SetItem(player1.Id, player1);
        var playersState = new PlayersState(updatedPlayers);

        return new RootState(gameState, playersState);
    }

    public static RootState CreateStateForNextTurn(int playerCount = 4, int cardsPerPlayer = 13, int currentPlayerId = 1)
    {
        // for now it's the same as play hand
        var initialState = CreateStateForPlayHand(playerCount, cardsPerPlayer);
        var gameState = initialState.Game with
        {
            CurrentPlayerId = currentPlayerId,
        };

        return initialState with
        {
            Game = gameState
        };
    }

    private static ImmutableDictionary<int, ImmutableList<Card>> CreateCardsForPlayer(int playerCount = 4, int cardsPerPlayer = 13)
    {
        var testDeck = CreateTestDeck();
        var playerCards = Enumerable.Range(1, playerCount)
            .ToImmutableDictionary(
                i => i,
                i => testDeck.Skip((i - 1) * cardsPerPlayer).Take(cardsPerPlayer).ToImmutableList());

        return playerCards;
    }

    private static ImmutableDictionary<int, PlayerState> CreatePlayers(ImmutableDictionary<int, ImmutableList<Card>>? playerCards = null)
    {
        var playersDict = Enumerable.Range(1, 4).ToImmutableDictionary(
            i => i,
            i => new PlayerState(i, $"Player {i}", playerCards?.GetValueOrDefault(i) ?? []));

        return playersDict;
    }
}