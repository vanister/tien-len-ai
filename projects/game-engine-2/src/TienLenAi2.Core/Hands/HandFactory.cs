using TienLenAi2.Core.Cards;

namespace TienLenAi2.Core.Hands;

public static class HandFactory
{
    public static bool TryCreateHand(IEnumerable<Card>? cards, HandType handType, out Hand? hand)
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

        switch (handType)
        {
            case HandType.Single:
                return TryCreateSingle(cardList, out hand);
            // case HandType.Pair:
            //     return TryCreatePair(cardList, out hand);
            // case HandType.Triple:
            //     return TryCreateTriple(cardList, out hand);
            // case HandType.Bomb:
            //     return TryCreateBomb(cardList, out hand);
            default:
                hand = null;
                return false;
        }
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
            [highestCard]
        );

        return true;
    }
}
