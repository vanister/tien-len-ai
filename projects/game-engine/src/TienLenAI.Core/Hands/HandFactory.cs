using TienLenAI.Core.Cards;

namespace TienLenAI.Core.Hands;

/// <summary>
/// Factory for creating the best possible hand from a collection of cards.
/// Returns the strongest valid hand type, or null if no valid hand can be formed.
/// </summary>
public static class HandFactory
{
    /// <summary>
    /// Creates the best possible hand from the given cards.
    /// Priority order: Bomb → Triple Straight → Double Straight → Straight → Triple → Pair → Single
    /// </summary>
    /// <param name="cards">Cards to form into a hand</param>
    /// <returns>The strongest valid hand, or null if no valid hand can be formed</returns>
    public static Hand? CreateHand(IEnumerable<Card> cards)
    {
        if (cards == null)
        {
            return null;
        }

        var cardList = cards.ToList();

        if (cardList.Count == 0)
        {
            return null;
        }

        // Try each hand type in priority order (strongest first)
        var bomb = TryCreateBomb(cardList);
        if (bomb != null)
        {
            return bomb;
        }

        var tripleStraight = TryCreateTripleStraight(cardList);
        if (tripleStraight != null)
        {
            return tripleStraight;
        }

        var doubleStraight = TryCreateDoubleStraight(cardList);
        if (doubleStraight != null)
        {
            return doubleStraight;
        }

        var straight = TryCreateStraight(cardList);
        if (straight != null)
        {
            return straight;
        }

        var triple = TryCreateTriple(cardList);
        if (triple != null)
        {
            return triple;
        }

        var pair = TryCreatePair(cardList);
        if (pair != null)
        {
            return pair;
        }

        return TryCreateSingle(cardList);
    }

    private static BombHand? TryCreateBomb(List<Card> cards)
    {
        if (cards.Count != 4)
        {
            return null;
        }

        var bomb = new BombHand(cards);
        return bomb.IsValid() ? bomb : null;
    }

    private static TripleStraightHand? TryCreateTripleStraight(List<Card> cards)
    {
        if (cards.Count < 6 || cards.Count % 3 != 0)
        {
            return null;
        }

        var tripleStraight = new TripleStraightHand(cards);
        return tripleStraight.IsValid() ? tripleStraight : null;
    }

    private static DoubleStraightHand? TryCreateDoubleStraight(List<Card> cards)
    {
        if (cards.Count < 6 || cards.Count % 2 != 0)
        {
            return null;
        }

        var doubleStraight = new DoubleStraightHand(cards);
        return doubleStraight.IsValid() ? doubleStraight : null;
    }

    private static StraightHand? TryCreateStraight(List<Card> cards)
    {
        if (cards.Count < 3 || cards.Count > 12)
        {
            return null;
        }

        var straight = new StraightHand(cards);
        return straight.IsValid() ? straight : null;
    }

    private static TripleHand? TryCreateTriple(List<Card> cards)
    {
        if (cards.Count != 3)
        {
            return null;
        }

        var triple = new TripleHand(cards);
        return triple.IsValid() ? triple : null;
    }

    private static PairHand? TryCreatePair(List<Card> cards)
    {
        if (cards.Count != 2)
        {
            return null;
        }

        var pair = new PairHand(cards);
        return pair.IsValid() ? pair : null;
    }

    private static SingleHand? TryCreateSingle(List<Card> cards)
    {
        if (cards.Count != 1)
        {
            return null;
        }

        return new SingleHand(cards[0]);
    }
}
