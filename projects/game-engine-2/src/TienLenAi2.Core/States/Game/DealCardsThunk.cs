using System.Collections.Immutable;
using TienLenAi2.Core.Cards;
using TienLenAi2.Core.Decks;
using TienLenAi2.Core.States.Players;

namespace TienLenAi2.Core.States.Game;

public record DealCardsThunk(
    ImmutableList<Card> ShuffledDeck,
    int CardsToDeal
) : IThunk
{
    public static DealCardsThunk Create(int cardsToDeal = 13, ImmutableList<Card>? shuffledDeck = null, int seed = 0)
    {
        var deck = shuffledDeck ?? StandardDeck.CreateShuffled(seed);
        return new DealCardsThunk(deck, cardsToDeal);
    }

    public IEnumerable<IAction> Execute(RootState currentState)
    {
        ArgumentNullException.ThrowIfNull(currentState);

        if (currentState.Players.TotalPlayers == 0)
        {
            throw new InvalidOperationException("Cannot deal cards when no players exist");
        }

        var playerIds = currentState.Players.Ids.OrderBy(id => id).ToList();
        var totalCardsNeeded = playerIds.Count * CardsToDeal;

        if (ShuffledDeck.Count < totalCardsNeeded)
        {
            throw new ArgumentException($"Not enough cards in deck. Need {totalCardsNeeded}, but deck has {ShuffledDeck.Count}");
        }

        var actions = new List<IAction>
        {
            // 1. Update game phase to Dealing
            new UpdateGamePhaseAction(GameActionTypes.UpdateGamePhase, GamePhase.Dealing)
        };

        // 2. Deal cards to each player
        var playerCardActions = ShuffledDeck
            .Take(totalCardsNeeded)
            .Select((card, index) => new { 
                Card = card, 
                // Assign card to player in round-robin fashion
                PlayerId = playerIds[index % playerIds.Count] 
            })
            .GroupBy(x => x.PlayerId)
            .Select(group => new UpdatePlayerCardsAction(
                PlayerActionTypes.UpdatePlayerCards,
                group.Key,
                [.. group.Select(x => x.Card)]
            ));

        actions.AddRange(playerCardActions);

        return actions;
    }
}
