namespace TienLenAi2.Core.Players;

public class HumanPlayer(int id, string name) : Player(id, name), IPlayer
{
    // Human players will implement their own logic for playing hands
    // For now, we can leave this empty or add methods for user interaction if needed

    // Override ToString for better debugging and logging
    public override string ToString()
    {
        return $"{Name} (Human Player, ID: {Id})";
    }
}