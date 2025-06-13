namespace TienLenAI.Core.Cards;

/// <summary>
/// Card suits ranked from lowest to highest: Spades < Clubs < Diamonds < Hearts
/// </summary>
public enum CardSuit : int
{
    /// <summary>♠ - Lowest</summary>
    Spades = 0,
    /// <summary>♣</summary>
    Clubs = 1,
    /// <summary>♦</summary>
    Diamonds = 2,
    /// <summary>♥ - Highest</summary>
    Hearts = 3
}
