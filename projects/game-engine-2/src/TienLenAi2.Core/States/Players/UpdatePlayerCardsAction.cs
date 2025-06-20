using System.Collections.Immutable;
using TienLenAi2.Core.Cards;

namespace TienLenAi2.Core.States.Players;

public record UpdatePlayerCardsAction(
    string Type,
    int PlayerId,
    ImmutableList<Card> Cards
) : IAction;
