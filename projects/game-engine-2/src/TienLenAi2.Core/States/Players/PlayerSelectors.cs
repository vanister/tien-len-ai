using TienLenAi2.Core.Cards;

namespace TienLenAi2.Core.States.Players;

public static class PlayerSelectors
{
    public static int? FindPlayerWith3OfSpades(RootState state)
    {
        var players = state.Players.ByIds;
        var threeOfSpades = Card.ThreeOfSpades;
        var playerWithThreeOfSpades = players.
            Select(p => p.Value)
            .FirstOrDefault(p => p.Cards.Contains(threeOfSpades))?.Id;

        return playerWithThreeOfSpades;
    }
}