using System.Collections.Immutable;
using TienLenAi2.Core.Cards;

namespace TienLenAi2.Core.States;

public record PlayerState
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    /// <summary>
    /// Represents the cards currently held by the player. His delt cards.
    /// </summary>
    public required ImmutableList<Card> Cards { get; init; }
    public bool HasPassed { get; init; }
}