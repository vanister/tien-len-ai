using System.Collections.Immutable;
using TienLenAi2.Core.Cards;

namespace TienLenAi2.Core.States.Players;

/// <summary>
/// Represents the state of a single player in the game.
/// </summary>
public record PlayerState(int Id, string Name, ImmutableList<Card> Cards)
{
    public static PlayerState WithIdAndName(int id, string name)
    {
        return new PlayerState(id, name, ImmutableList<Card>.Empty);
    }
}