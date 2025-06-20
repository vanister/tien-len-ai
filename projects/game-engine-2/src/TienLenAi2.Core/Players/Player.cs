namespace TienLenAi2.Core.Players;

public abstract class Player(int id, string name): IPlayer
{
    public int Id { get; } = id;
    public string Name { get; } = name;

    // Override ToString for better debugging and logging
    public override string ToString()
    {
        return $"{Name} (ID: {Id})";
    }
}