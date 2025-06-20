// this is the interface for a player in the Tien Len AI game
namespace TienLenAi2.Core.Players;

public interface IPlayer
{
    int Id { get; }
    string Name { get; }

    // todo - Hand? PlayHand(GameState gameState, currentHand: Hand);
}