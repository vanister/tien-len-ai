namespace TienLenAI.Core.Cards;

/// <summary>
/// Card ranks from lowest to highest: 3 < 4 < 5 < 6 < 7 < 8 < 9 < 10 < J < Q < K < A < 2
/// </summary>
public enum CardRank : int
{
    /// <summary>3 - Lowest</summary>
    Three = 3,
    /// <summary>4</summary>
    Four = 4,
    /// <summary>5</summary>
    Five = 5,
    /// <summary>6</summary>
    Six = 6,
    /// <summary>7</summary>
    Seven = 7,
    /// <summary>8</summary>
    Eight = 8,
    /// <summary>9</summary>
    Nine = 9,
    /// <summary>10</summary>
    Ten = 10,
    /// <summary>Jack</summary>
    Jack = 11,
    /// <summary>Queen</summary>
    Queen = 12,
    /// <summary>King</summary>
    King = 13,
    /// <summary>Ace</summary>
    Ace = 14,
    /// <summary>2 - Highest</summary>
    Two = 15
}