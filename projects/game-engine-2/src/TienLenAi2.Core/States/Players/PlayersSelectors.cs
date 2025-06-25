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

    public static PlayerState? FindPlayerById(RootState state, int playerId)
    {
        var player = state.Players.ByIds.GetValueOrDefault(playerId);

        return player;
    }   
    
    public static bool TryFindWinner(RootState state, out PlayerState? winner)
    {
        var players = state.Players.ByIds.Values;

        // Check if any player has no cards left
        winner = players.FirstOrDefault(p => p.Cards.Count == 0);

        return winner != null;
    }   

    public static bool IsWinner(RootState state, int playerId)
    {
        var player = FindPlayerById(state, playerId);

        if (player == null)
        {
            return false; // Player does not exist
        }

        return player.Cards.Count == 0;
    }
}