using TienLenAI.Core.Cards;

namespace TienLenAI.Core.Hands;

/// <summary>
/// Factory for creating hands from collections of cards with various strategies.
/// Provides methods to create best hands, specific hand types, and hands that beat targets.
/// </summary>
public static class HandFactory
{
    /// <summary>
    /// Creates the best possible hand from the given cards.
    /// Priority order: Bomb → Triple Straight → Double Straight → Straight → Triple → Pair → Single
    /// </summary>
    /// <param name="cards">Cards to form into a hand</param>
    /// <returns>The strongest valid hand, or null if no valid hand can be formed</returns>
    public static Hand? CreateBestHand(IEnumerable<Card> cards)
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

    /// <summary>
    /// Creates all possible hands of the specified type from the given cards.
    /// For straights, returns all possible lengths (3-card, 4-card, etc.).
    /// </summary>
    /// <param name="cards">Cards available to form hands</param>
    /// <param name="handType">Type of hand to create</param>
    /// <returns>All valid hands of the specified type</returns>
    public static IReadOnlyList<Hand> CreateHandsForType(IEnumerable<Card> cards, HandType handType)
    {
        if (cards == null)
        {
            return [];
        }

        var cardList = cards.ToList();

        if (cardList.Count == 0)
        {
            return [];
        }

        return handType switch
        {
            HandType.Single => CreateAllSingles(cardList),
            HandType.Pair => CreateAllPairs(cardList),
            HandType.Triple => CreateAllTriples(cardList),
            HandType.Straight => CreateAllStraights(cardList),
            HandType.DoubleStraight => CreateAllDoubleStraights(cardList),
            HandType.TripleStraight => CreateAllTripleStraights(cardList),
            HandType.Bomb => CreateAllBombs(cardList),
            _ => []
        };
    }

    #region Private Helper Methods for CreateBestHand

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

    #endregion

    #region Private Helper Methods for CreateHandsForType

    private static IReadOnlyList<Hand> CreateAllSingles(List<Card> cards)
    {
        return cards.Select(card => new SingleHand(card)).ToList();
    }

    private static IReadOnlyList<Hand> CreateAllPairs(List<Card> cards)
    {
        var pairs = new List<Hand>();

        // Group cards by rank to find pairs
        var rankGroups = cards.GroupBy(c => c.Rank).ToList();

        foreach (var group in rankGroups.Where(g => g.Count() >= 2))
        {
            var cardsOfRank = group.ToList();

            // Generate all combinations of 2 cards from this rank
            for (int i = 0; i < cardsOfRank.Count - 1; i++)
            {
                for (int j = i + 1; j < cardsOfRank.Count; j++)
                {
                    var pair = new PairHand([cardsOfRank[i], cardsOfRank[j]]);
                    if (pair.IsValid())
                    {
                        pairs.Add(pair);
                    }
                }
            }
        }

        return pairs;
    }

    private static IReadOnlyList<Hand> CreateAllTriples(List<Card> cards)
    {
        var triples = new List<Hand>();

        // Group cards by rank to find triples
        var rankGroups = cards.GroupBy(c => c.Rank).ToList();

        foreach (var group in rankGroups.Where(g => g.Count() >= 3))
        {
            var cardsOfRank = group.ToList();

            // Generate all combinations of 3 cards from this rank
            for (int i = 0; i < cardsOfRank.Count - 2; i++)
            {
                for (int j = i + 1; j < cardsOfRank.Count - 1; j++)
                {
                    for (int k = j + 1; k < cardsOfRank.Count; k++)
                    {
                        var triple = new TripleHand([cardsOfRank[i], cardsOfRank[j], cardsOfRank[k]]);
                        if (triple.IsValid())
                        {
                            triples.Add(triple);
                        }
                    }
                }
            }
        }

        return triples;
    }

    private static IReadOnlyList<Hand> CreateAllStraights(List<Card> cards)
    {
        var straights = new List<Hand>();

        // Get unique ranks and sort them (excluding 2s as they can't be in straights)
        var availableRanks = cards
            .Where(c => c.Rank != CardRank.Two)
            .Select(c => c.Rank)
            .Distinct()
            .OrderBy(r => r)
            .ToList();

        // Try all possible straight lengths (3 to 12)
        for (int length = 3; length <= Math.Min(12, availableRanks.Count); length++)
        {
            // Try all possible starting positions
            for (int start = 0; start <= availableRanks.Count - length; start++)
            {
                // Check if we have consecutive ranks
                bool isConsecutive = true;
                for (int i = 0; i < length - 1; i++)
                {
                    if ((int)availableRanks[start + i + 1] - (int)availableRanks[start + i] != 1)
                    {
                        isConsecutive = false;
                        break;
                    }
                }

                if (isConsecutive)
                {
                    var requiredRanks = availableRanks.Skip(start).Take(length).ToList();

                    // For each required rank, try all available cards of that rank
                    var cardsByRank = requiredRanks.Select(rank =>
                        cards.Where(c => c.Rank == rank).ToList()).ToList();

                    // Generate all combinations (one card per rank)
                    foreach (var combination in GetCartesianProduct(cardsByRank))
                    {
                        var straight = new StraightHand(combination);
                        if (straight.IsValid())
                        {
                            straights.Add(straight);
                        }
                    }
                }
            }
        }

        return straights;
    }

    private static IReadOnlyList<Hand> CreateAllBombs(List<Card> cards)
    {
        var bombs = new List<Hand>();

        // Group cards by rank to find bombs (4 of a kind)
        var rankGroups = cards.GroupBy(c => c.Rank).ToList();

        foreach (var group in rankGroups.Where(g => g.Count() >= 4))
        {
            var cardsOfRank = group.ToList();

            // Generate all combinations of 4 cards from this rank
            // (In most cases there will be exactly 4, but this handles edge cases)
            var combinations = GetCombinations(cardsOfRank, 4);
            foreach (var combination in combinations)
            {
                var bomb = new BombHand(combination);
                if (bomb.IsValid())
                {
                    bombs.Add(bomb);
                }
            }
        }

        return bombs;
    }

    private static IReadOnlyList<Hand> CreateAllDoubleStraights(List<Card> cards)
    {
        // TODO: Implement double straight generation
        // This is complex - need to find consecutive pairs
        return [];
    }

    private static IReadOnlyList<Hand> CreateAllTripleStraights(List<Card> cards)
    {
        // TODO: Implement triple straight generation
        // This is complex - need to find consecutive triples
        return [];
    }

    #endregion

    #region Utility Methods

    /// <summary>
    /// Generates all combinations of specified size from a collection
    /// </summary>
    private static IEnumerable<List<T>> GetCombinations<T>(IList<T> items, int size)
    {
        if (size == 0)
        {
            yield return new List<T>();
            yield break;
        }

        if (items.Count < size)
        {
            yield break;
        }

        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            var remainingItems = items.Skip(i + 1).ToList();

            foreach (var combination in GetCombinations(remainingItems, size - 1))
            {
                combination.Insert(0, item);
                yield return combination;
            }
        }
    }

    /// <summary>
    /// Generates cartesian product of multiple collections
    /// </summary>
    private static IEnumerable<List<T>> GetCartesianProduct<T>(IEnumerable<IEnumerable<T>> sequences)
    {
        var sequenceList = sequences.ToList();
        if (!sequenceList.Any())
        {
            yield return new List<T>();
            yield break;
        }

        var first = sequenceList.First().ToList();
        var rest = sequenceList.Skip(1);

        foreach (var item in first)
        {
            foreach (var restCombination in GetCartesianProduct(rest))
            {
                var result = new List<T> { item };
                result.AddRange(restCombination);
                yield return result;
            }
        }
    }

    #endregion
}
