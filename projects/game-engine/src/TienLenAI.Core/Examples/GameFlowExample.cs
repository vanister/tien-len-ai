using TienLenAI.Core.Cards;
using TienLenAI.Core.Hands;
using TienLenAI.Core.State;

namespace TienLenAI.Core.Examples;

/// <summary>
/// Demonstrates the integrated GameState and TrickState functionality
/// </summary>
public static class GameFlowExample
{
    public static void RunExample()
    {
        Console.WriteLine("=== Tiến Lên Game Flow Example ===\n");

        // Create players with sample hands
        var players = CreateSamplePlayers();
        var gameState = new GameState(players, isFirstGame: true);

        Console.WriteLine($"Starting player: {gameState.GetCurrentPlayer().Name} (has 3♠)");
        Console.WriteLine($"Game is first game: {gameState.IsFirstGame}");
        Console.WriteLine($"Active players: {string.Join(", ", gameState.GetActivePlayers())}");
        Console.WriteLine();

        // First play - must include 3♠
        Console.WriteLine("=== First Play ===");
        var firstHand = new SingleHand(new Card(CardRank.Three, CardSuit.Spades));
        Console.WriteLine($"Player {gameState.CurrentPlayerIndex} plays: {FormatHand(firstHand)}");

        if (gameState.IsValidPlay(firstHand))
        {
            gameState = gameState.PlayHand(firstHand);
            Console.WriteLine("✓ Play accepted");
            Console.WriteLine($"Current trick requires: {gameState.CurrentTrick.RequiredHandType}");
            Console.WriteLine($"Current hand to beat: {FormatHand(gameState.CurrentTrick.CurrentHand)}");
            Console.WriteLine($"Next player: {gameState.GetCurrentPlayer().Name}");
        }
        else
        {
            Console.WriteLine("✗ Invalid play");
        }
        Console.WriteLine();

        // Second play - must beat 3♠ with a higher single
        Console.WriteLine("=== Second Play ===");
        var secondHand = new SingleHand(new Card(CardRank.Four, CardSuit.Hearts));
        Console.WriteLine($"Player {gameState.CurrentPlayerIndex} plays: {FormatHand(secondHand)}");

        if (gameState.IsValidPlay(secondHand))
        {
            gameState = gameState.PlayHand(secondHand);
            Console.WriteLine("✓ Play accepted");
            Console.WriteLine($"Current hand to beat: {FormatHand(gameState.CurrentTrick.CurrentHand)}");
            Console.WriteLine($"Next player: {gameState.GetCurrentPlayer().Name}");
        }
        else
        {
            Console.WriteLine("✗ Invalid play");
        }
        Console.WriteLine();

        // Third player passes
        Console.WriteLine("=== Third Play (Pass) ===");
        Console.WriteLine($"Player {gameState.CurrentPlayerIndex} passes");
        gameState = gameState.PassTurn();
        Console.WriteLine($"Next player: {gameState.GetCurrentPlayer().Name}");
        Console.WriteLine();

        // Fourth player passes - trick should complete
        Console.WriteLine("=== Fourth Play (Pass) ===");
        Console.WriteLine($"Player {gameState.CurrentPlayerIndex} passes");
        gameState = gameState.PassTurn();
        Console.WriteLine($"Trick completed: {gameState.CurrentTrick.IsComplete}");

        if (gameState.CurrentTrick.IsComplete)
        {
            Console.WriteLine($"Trick winner starts new trick: Player {gameState.CurrentPlayerIndex}");
        }
        Console.WriteLine();

        Console.WriteLine("=== Game State Summary ===");
        Console.WriteLine($"Game complete: {gameState.IsGameComplete}");
        Console.WriteLine($"Players finished: {gameState.FinishOrder.Count}");
        Console.WriteLine($"First play made: {gameState.HasFirstPlayBeenMade}");
        Console.WriteLine($"Current trick number: {gameState.TrickNumber}");
        Console.WriteLine($"Completed tricks: {gameState.TrickHistory.Count}");
        Console.WriteLine();

        Console.WriteLine("=== Trick History Analysis ===");
        var cardCounts = gameState.GetCardsPlayedCounts();
        var winCounts = gameState.GetTrickWinCounts();
        var playedTypes = gameState.GetPlayedHandTypes();

        Console.WriteLine("Cards played by each player:");
        for (int i = 0; i < gameState.Players.Count; i++)
        {
            Console.WriteLine($"  Player {i} ({gameState.Players[i].Name}): {cardCounts[i]} cards, {winCounts[i]} tricks won");
        }

        Console.WriteLine($"Hand types played: {string.Join(", ", playedTypes)}");
        Console.WriteLine();

        Console.WriteLine("=== Player Hand Status ===");
        for (int i = 0; i < gameState.Players.Count; i++)
        {
            var playerHistory = gameState.GetPlayerHandHistory(i);
            Console.WriteLine($"Player {i} ({gameState.Players[i].Name}):");
            Console.WriteLine($"  Current cards: {gameState.Players[i].Hand.Count}");
            Console.WriteLine($"  Hands played: {playerHistory.Count()}");

            if (playerHistory.Any())
            {
                Console.WriteLine($"  Hand history: {string.Join(", ", playerHistory.Select(FormatHand))}");
            }
        }
    }

    private static List<Player> CreateSamplePlayers()
    {
        return
        [
            new Player("Alice",
            [
                new Card(CardRank.Three, CardSuit.Spades), // Starting card
                new Card(CardRank.Five, CardSuit.Hearts),
                new Card(CardRank.Seven, CardSuit.Diamonds),
                new Card(CardRank.Nine, CardSuit.Clubs),
                new Card(CardRank.Jack, CardSuit.Spades)
            ]),
            new Player("Bob",
            [
                new Card(CardRank.Four, CardSuit.Hearts),
                new Card(CardRank.Six, CardSuit.Diamonds),
                new Card(CardRank.Eight, CardSuit.Clubs),
                new Card(CardRank.Ten, CardSuit.Spades),
                new Card(CardRank.Queen, CardSuit.Hearts)
            ]),
            new Player("Carol",
            [
                new Card(CardRank.Three, CardSuit.Hearts),
                new Card(CardRank.Five, CardSuit.Diamonds),
                new Card(CardRank.Seven, CardSuit.Clubs),
                new Card(CardRank.Nine, CardSuit.Spades),
                new Card(CardRank.Jack, CardSuit.Hearts)
            ]),
            new Player("Dave",
            [
                new Card(CardRank.Four, CardSuit.Diamonds),
                new Card(CardRank.Six, CardSuit.Clubs),
                new Card(CardRank.Eight, CardSuit.Spades),
                new Card(CardRank.Ten, CardSuit.Hearts),
                new Card(CardRank.Queen, CardSuit.Diamonds)
            ])
        ];
    }

    private static string FormatHand(Hand? hand)
    {
        if (hand == null) return "None";

        var cardStrings = hand.Cards.Select(FormatCard);
        return $"{hand.Type}: [{string.Join(", ", cardStrings)}]";
    }

    private static string FormatCard(Card card)
    {
        var rankStr = card.Rank switch
        {
            CardRank.Three => "3",
            CardRank.Four => "4",
            CardRank.Five => "5",
            CardRank.Six => "6",
            CardRank.Seven => "7",
            CardRank.Eight => "8",
            CardRank.Nine => "9",
            CardRank.Ten => "10",
            CardRank.Jack => "J",
            CardRank.Queen => "Q",
            CardRank.King => "K",
            CardRank.Ace => "A",
            CardRank.Two => "2",
            _ => "?"
        };

        var suitStr = card.Suit switch
        {
            CardSuit.Spades => "♠",
            CardSuit.Clubs => "♣",
            CardSuit.Diamonds => "♦",
            CardSuit.Hearts => "♥",
            _ => "?"
        };

        return $"{rankStr}{suitStr}";
    }
}
