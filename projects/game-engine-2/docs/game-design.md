## Tiến Lên Game Engine Desigh

### **Core Architecture**
**Redux Pattern with Separate Reducers:**
- `GameReducer` - handles game-level state (current play, tricks, game phase)
- `PlayersReducer` - handles all player state (hands, scores, passes)
- `RootReducer` - combines both slices with immutable updates
- Single store with complete deterministic state management

### **State Structure**
```csharp
public record RootState
{
    public GameState Game { get; init; } = new();
    public ImmutableDictionary<int, PlayerState> Players { get; init; } = /*...*/;
}

public record GameState
{
    public ImmutableList<Card> CurrentPlay { get; init; }
    public Hand? LastPlayedHand { get; init; }
    public int CurrentPlayerId { get; init; }
    public GamePhase Phase { get; init; }
    public ImmutableList<IAction> History { get; init; }
}

public record PlayerState
{
    public int Id { get; init; }                      // Simple int (1,2,3,4)
    public string Name { get; init; }
    public ImmutableList<Card> Hand { get; init; }   // Player's cards
    public int Score { get; init; }
    public bool HasPassed { get; init; }
}
```

### **Performance-Optimized Data**
```csharp
// Cards as structs for memory efficiency
public readonly struct Card
{
    public readonly Suit Suit;
    public readonly Rank Rank;
}

// Hands represent legal Tiến Lên combinations
public record Hand : IComparable<Hand>
{
    public HandType Type { get; init; }              // Single, Pair, Straight, Bomb
    public ImmutableList<Card> Cards { get; init; }
    public int Rank { get; init; }
    // Built-in comparison logic for game rules
}
```

### **Player Architecture**
```csharp
public interface IPlayer
{
    int Id { get; }
    string Name { get; }
    // Stateless strategy - receives options, returns choice
    Hand? SelectHand(GameState gameState, List<Hand> availableHands);
}
```

**Player Types:**
- AI Players (various strategies)
- Human Players (UI-driven)
- Network Players (for multiplayer)

### **Game Flow**
1. **Game engine generates legal hands** for current player using Tiến Lên rules
2. **Player selects from valid options** (constraint-based decisions)
3. **Action dispatched to store** through appropriate reducer
4. **State updated immutably** using `with` expressions
5. **Repeat until game complete**

### **Key Benefits**

**AI Training Optimized:**
- Deterministic and reproducible
- Fast execution with struct cards
- Complete observable state
- Constraint-based learning (only valid moves)

**Clean Architecture:**
- Single responsibility per component
- Type-safe immutable state
- Centralized game rules
- Easy to test and extend

**Flexible Usage:**
- Same engine for training, PvP, human vs AI
- Easy to add new player strategies
- Supports parallel training runs

### **Implementation Plan**
**Build this foundation first** - it's already well-optimized and focused. Add advanced optimizations (object pooling, analytics, batch processing) later based on actual performance needs.

**Simple, clean, and ready for AI training.**