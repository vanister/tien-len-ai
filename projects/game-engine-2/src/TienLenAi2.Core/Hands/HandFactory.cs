using TienLenAi2.Core.Cards;

namespace TienLenAi2.Core.Hands;

public static class HandFactory
{
    public static bool TryCreateHand(IEnumerable<Card>? cards, out Hand? hand)
    {
        if (cards == null)
        {
            hand = null;
            return false;
        }

        var cardList = cards.ToList();
        if (cardList.Count == 0)
        {
            hand = null;
            return false;
        }

        // For the initial implementation, only try to create singles and pairs
        if (TryCreatePair(cardList, out hand))
        {
            return true;
        }

        if (TryCreateSingle(cardList, out hand))
        {
            return true;
        }

        hand = null;
        return false;
    }

    private static bool TryCreateSingle(List<Card> cards, out Hand? hand)
    {
        if (cards.Count == 0)
        {
            hand = null;
            return false;
        }

        // create the highest single card hand
        var highestCard = cards.OrderByDescending(c => c.Rank).SingleOrDefault();

        hand = new Hand(
            HandType.Single,
            [highestCard],
            (int)highestCard.Rank,
            highestCard
        );

        return true;
    }

    private static bool TryCreatePair(List<Card> cards, out Hand? hand)
    {
        if (cards == null || cards.Count < 2)
        {
            hand = null;
            return false;
        }

        // Group cards by rank (high to low) and find pairs
        var groupedByRank = cards
            .GroupBy(c => c.Rank)
            .Where(g => g.Count() >= 2)
            .OrderByDescending(g => g.Key)
            .ToList();

        // create a pair with the highest rank
        if (groupedByRank.Count == 0)
        {
            hand = null;
            return false;
        }

        var highestPair = groupedByRank.First();
        var pairCards = highestPair.Take(2).ToList();

        hand = new Hand(
            HandType.Pair,
            pairCards,
            (int)highestPair.Key,
            pairCards[0] // Highest card in the pair
        );

        return true;
    }
}
