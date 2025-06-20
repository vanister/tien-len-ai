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
}
