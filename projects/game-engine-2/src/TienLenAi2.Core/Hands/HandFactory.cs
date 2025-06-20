using System.Collections.Immutable;
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

        // Try to create each type of hand in order of precedence
        if (TryCreateBomb(cardList, out hand))
        {
            return true;
        }

        if (TryCreateTripleStraight(cardList, out hand))
        {
            return true;
        }

        if (TryCreateDoubleStraight(cardList, out hand))
        {
            return true;
        }

        if (TryCreateStraight(cardList, out hand))
        {
            return true;
        }

        if (TryCreateTriple(cardList, out hand))
        {
            return true;
        }

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
        if (cards.Count == 1)
        {
            var card = cards[0];
            hand = new Hand(
                HandType.Single,
                new[] { card },
                (int)card.Rank,
                card
            );
            return true;
        }

        hand = null;
        return false;
    }

    private static bool TryCreatePair(List<Card> cards, out Hand? hand)
    {
        if (cards.Count == 2 && cards[0].Rank == cards[1].Rank)
        {
            // Sort the cards to ensure the highest card is correctly identified
            var sortedCards = cards.OrderBy(c => c).ToList();
            var highestCard = sortedCards[1];

            hand = new Hand(
                HandType.Pair,
                sortedCards,
                (int)highestCard.Rank,
                highestCard
            );
            return true;
        }

        hand = null;
        return false;
    }

    private static bool TryCreateTriple(List<Card> cards, out Hand? hand)
    {
        if (cards.Count == 3 && cards[0].Rank == cards[1].Rank && cards[1].Rank == cards[2].Rank)
        {
            // Sort the cards to ensure the highest card is correctly identified
            var sortedCards = cards.OrderBy(c => c).ToList();
            var highestCard = sortedCards[2];

            hand = new Hand(
                HandType.Triple,
                sortedCards,
                (int)highestCard.Rank,
                highestCard
            );
            return true;
        }

        hand = null;
        return false;
    }

    private static bool TryCreateBomb(List<Card> cards, out Hand? hand)
    {
        if (cards.Count == 4 &&
            cards[0].Rank == cards[1].Rank &&
            cards[1].Rank == cards[2].Rank &&
            cards[2].Rank == cards[3].Rank)
        {
            // Sort the cards to ensure the highest card is correctly identified
            var sortedCards = cards.OrderBy(c => c).ToList();
            var highestCard = sortedCards[3];

            hand = new Hand(
                HandType.Bomb,
                sortedCards,
                (int)highestCard.Rank,
                highestCard
            );
            return true;
        }

        hand = null;
        return false;
    }

    private static bool TryCreateStraight(List<Card> cards, out Hand? hand)
    {
        // Need at least 3 cards for a straight
        if (cards.Count < 3)
        {
            hand = null;
            return false;
        }

        // 2s cannot be used in straights
        if (cards.Any(c => c.Rank == Rank.Two))
        {
            hand = null;
            return false;
        }

        // Max straight is 12 cards (3 through Ace)
        if (cards.Count > 12)
        {
            hand = null;
            return false;
        }

        // Sort the cards by rank
        var sortedByRank = cards
            .OrderBy(c => c.Rank)
            .ToList();

        // Check if the ranks form a consecutive sequence
        for (int i = 1; i < sortedByRank.Count; i++)
        {
            if ((int)sortedByRank[i].Rank != (int)sortedByRank[i - 1].Rank + 1)
            {
                hand = null;
                return false;
            }
        }

        // Find the highest card by full card comparison
        var highestCard = cards.Max();

        hand = new Hand(
            HandType.Straight,
            sortedByRank,
            (int)highestCard.Rank,
            highestCard
        );
        return true;
    }

    private static bool TryCreateDoubleStraight(List<Card> cards, out Hand? hand)
    {
        // Need at least 6 cards for a double straight (3 pairs)
        // And must be an even number of cards
        if (cards.Count < 6 || cards.Count % 2 != 0)
        {
            hand = null;
            return false;
        }

        // 2s cannot be used in straights
        if (cards.Any(c => c.Rank == Rank.Two))
        {
            hand = null;
            return false;
        }

        // Group by rank to find the pairs
        var rankGroups = cards.GroupBy(c => c.Rank).ToList();

        // We need the same number of pairs as half the card count
        if (rankGroups.Count != cards.Count / 2)
        {
            hand = null;
            return false;
        }

        // Each rank should have exactly 2 cards
        if (rankGroups.Any(g => g.Count() != 2))
        {
            hand = null;
            return false;
        }

        // Sort the groups by rank
        var sortedGroups = rankGroups
            .OrderBy(g => g.Key)
            .ToList();

        // Check if the ranks form a consecutive sequence
        for (int i = 1; i < sortedGroups.Count; i++)
        {
            if ((int)sortedGroups[i].Key != (int)sortedGroups[i - 1].Key + 1)
            {
                hand = null;
                return false;
            }
        }

        // Flatten the sorted groups back to a list
        var sortedCards = sortedGroups.SelectMany(g => g.OrderBy(c => c)).ToList();

        // Find the highest card by full card comparison
        var highestCard = sortedCards.Max();

        hand = new Hand(
            HandType.DoubleStraight,
            sortedCards,
            (int)highestCard.Rank,
            highestCard
        );
        return true;
    }

    private static bool TryCreateTripleStraight(List<Card> cards, out Hand? hand)
    {
        // Need at least 6 cards for a triple straight (2 triples)
        // And must be a multiple of 3
        if (cards.Count < 6 || cards.Count % 3 != 0)
        {
            hand = null;
            return false;
        }

        // 2s cannot be used in straights
        if (cards.Any(c => c.Rank == Rank.Two))
        {
            hand = null;
            return false;
        }

        // Group by rank to find the triples
        var rankGroups = cards.GroupBy(c => c.Rank).ToList();

        // We need the same number of triples as one third of the card count
        if (rankGroups.Count != cards.Count / 3)
        {
            hand = null;
            return false;
        }

        // Each rank should have exactly 3 cards
        if (rankGroups.Any(g => g.Count() != 3))
        {
            hand = null;
            return false;
        }

        // Sort the groups by rank
        var sortedGroups = rankGroups
            .OrderBy(g => g.Key)
            .ToList();

        // Check if the ranks form a consecutive sequence
        for (int i = 1; i < sortedGroups.Count; i++)
        {
            if ((int)sortedGroups[i].Key != (int)sortedGroups[i - 1].Key + 1)
            {
                hand = null;
                return false;
            }
        }

        // Flatten the sorted groups back to a list
        var sortedCards = sortedGroups.SelectMany(g => g.OrderBy(c => c)).ToList();

        // Find the highest card by full card comparison
        var highestCard = sortedCards.Max();

        hand = new Hand(
            HandType.TripleStraight,
            sortedCards,
            (int)highestCard.Rank,
            highestCard
        );
        return true;
    }

    public static IEnumerable<Hand> FindAllValidHands(IEnumerable<Card>? cards)
    {
        if (cards == null)
        {
            return [];
        }

        var cardList = cards.ToList();
        var results = new List<Hand>();

        // Group cards by rank for efficient hand finding
        var rankGroups = cardList.GroupBy(c => c.Rank).ToDictionary(g => g.Key, g => g.ToList());

        // Find all single cards
        foreach (var card in cardList)
        {
            if (TryCreateHand(new[] { card }, out var hand) && hand != null)
            {
                results.Add(hand);
            }
        }

        // Find all pairs, triples, and bombs using the rank groups
        foreach (var group in rankGroups.Values)
        {
            // For each rank, find all combinations of 2 cards (pairs)
            if (group.Count >= 2)
            {
                FindCombinations(group, 2, combo =>
                {
                    if (TryCreateHand(combo, out var hand) && hand != null)
                    {
                        results.Add(hand);
                    }
                });
            }

            // Find all combinations of 3 cards (triples)
            if (group.Count >= 3)
            {
                FindCombinations(group, 3, combo =>
                {
                    if (TryCreateHand(combo, out var hand) && hand != null)
                    {
                        results.Add(hand);
                    }
                });
            }

            // Find bombs (all 4 cards of the same rank)
            if (group.Count == 4)
            {
                if (TryCreateHand(group, out var hand) && hand != null)
                {
                    results.Add(hand);
                }
            }
        }

        // Find potential straights (excluding 2s)
        var potentialStraightCards = cardList
            .Where(c => c.Rank != Rank.Two)
            .OrderBy(c => c.Rank)
            .ToList();

        // Find straight combinations of various lengths (3 to 12 cards)
        for (int length = 3; length <= 12 && length <= potentialStraightCards.Count; length++)
        {
            FindStraightCombinations(potentialStraightCards, length, results);
        }

        // Find double straights (3+ consecutive pairs)
        FindPatternStraights(rankGroups, 2, 3, results); // 2 cards per rank, min 3 consecutive ranks

        // Find triple straights (2+ consecutive triples)
        FindPatternStraights(rankGroups, 3, 2, results); // 3 cards per rank, min 2 consecutive ranks

        return results;
    }

    // Helper method to find all combinations of k elements from the list
    private static void FindCombinations<T>(List<T> list, int k, Action<List<T>> resultAction)
    {
        var result = new List<T>(k);
        FindCombinationsInternal(list, k, 0, result, resultAction);
    }

    private static void FindCombinationsInternal<T>(List<T> list, int k, int start, List<T> result, Action<List<T>> resultAction)
    {
        if (k == 0)
        {
            resultAction(new List<T>(result));
            return;
        }

        for (int i = start; i <= list.Count - k; i++)
        {
            result.Add(list[i]);
            FindCombinationsInternal(list, k - 1, i + 1, result, resultAction);
            result.RemoveAt(result.Count - 1);
        }
    }

    // Helper method to find potential straight combinations
    private static void FindStraightCombinations(List<Card> cards, int length, List<Hand> results)
    {
        // Group cards by rank to find all ranks present
        var ranks = cards.Select(c => c.Rank).Distinct().OrderBy(r => r).ToList();

        // Find consecutive sequences of ranks of the required length
        for (int i = 0; i <= ranks.Count - length; i++)
        {
            bool isConsecutive = true;
            for (int j = i + 1; j < i + length; j++)
            {
                if ((int)ranks[j] != (int)ranks[j - 1] + 1)
                {
                    isConsecutive = false;
                    break;
                }
            }

            if (isConsecutive)
            {
                // Get one card of each rank in the sequence
                var straightCards = new List<Card>();
                for (int j = i; j < i + length; j++)
                {
                    straightCards.Add(cards.First(c => c.Rank == ranks[j]));
                }

                if (TryCreateHand(straightCards, out var hand) && hand != null)
                {
                    results.Add(hand);
                }
            }
        }
    }

    // Helper method to find pattern straights (double straights and triple straights)
    private static void FindPatternStraights(Dictionary<Rank, List<Card>> rankGroups, int cardsPerRank,
                                            int minConsecutiveRanks, List<Hand> results)
    {
        // Filter ranks that have exactly the required number of cards
        var validRanks = rankGroups.Where(g => g.Value.Count >= cardsPerRank && g.Key != Rank.Two)
                                 .Select(g => g.Key)
                                 .OrderBy(r => r)
                                 .ToList();

        // Find all consecutive sequences of ranks with minimum required length
        for (int i = 0; i <= validRanks.Count - minConsecutiveRanks; i++)
        {
            // Find the longest consecutive sequence starting at position i
            int length = 1;
            while (i + length < validRanks.Count &&
                  (int)validRanks[i + length] == (int)validRanks[i + length - 1] + 1)
            {
                length++;
            }

            // For each valid length from minimum required to the longest found
            for (int seqLength = minConsecutiveRanks; seqLength <= length; seqLength++)
            {
                var patternCards = new List<Card>();

                // Add the required number of cards for each rank in the sequence
                for (int j = i; j < i + seqLength; j++)
                {
                    var cardsOfRank = rankGroups[validRanks[j]];
                    patternCards.AddRange(cardsOfRank.Take(cardsPerRank));
                }

                if (TryCreateHand(patternCards, out var hand) && hand != null)
                {
                    results.Add(hand);
                }
            }
        }
    }

    public static IEnumerable<Hand> FindAllHandsThatCanBeat(IEnumerable<Card>? cards, Hand? handToBeat)
    {
        if (cards == null || handToBeat == null)
        {
            return Enumerable.Empty<Hand>();
        }

        return FindAllValidHands(cards).Where(h => h.CanBeat(handToBeat));
    }
}
