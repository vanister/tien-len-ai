# Tiến Lên Game Engine Design - AI Training Ready

## Core Components Architecture

### 1. Immutable Game State System
```
GameState (immutable)
├── Players (readonly array)
├── Current player index
├── Current trick state
├── Played cards history
├── Game phase enum
├── Turn number
├── Winner index
└── Hash/fingerprint for memoization

PlayerState (immutable)
├── Player ID
├── Hand (readonly card array)
├── Is active flag
├── Passed this trick flag
└── Player statistics

TrickState (immutable)
├── Required hand type
├── Cards in play (readonly)
├── Minimum rank to beat
├── Starting player index
├── Last playing player index
└── Trick start turn number
```

### 2. Fast Card System for AI
```
Card
├── Rank (0-12 numeric)
├── Suit (0-3 numeric)  
├── Value (0-51 computed ranking)
├── Bitwise representation
└── Fast comparison methods

CardSet (bitset optimization)
├── 64-bit representation
├── Fast union/intersection
├── Contains operations O(1)
├── Iteration utilities
└── Conversion to Card arrays
```

### 3. AI-Optimized Hand Management
```
HandAnalyzer (static/pure functions)
├── GetHandType(cards) -> HandType
├── GetHandRank(cards, type) -> number
├── CanBeat(hand1, hand2) -> boolean
├── FindAllValidHands(cards) -> Card[][]
├── FindHandsThatBeat(cards, target) -> Card[][]
├── FindHandsContainingCard(cards, card) -> Card[][]
├── GetHandStrength(cards, gameState) -> number
└── ExtractHandFeatures(cards) -> number[]

LegalMoveGenerator
├── GetAllLegalMoves(gameState) -> Card[][]
├── GetLegalMovesForTrick(hand, trick) -> Card[][]
├── GetFirstPlayMoves(hand) -> Card[][]
├── MoveCount(gameState) -> number
└── HasLegalMoves(gameState) -> boolean
```

### 4. Performance-Optimized Rule Engine
```
RuleValidator (pure functions)
├── IsValidMove(gameState, cards) -> boolean
├── ValidateHandType(cards) -> ValidationResult
├── CanPlayOnTrick(cards, trick) -> boolean
├── CheckWinCondition(gameState) -> number | null
├── IsGameComplete(gameState) -> boolean
└── GetMoveValidationError(gameState, cards) -> string

ValidationResult
├── IsValid boolean
├── HandType enum
├── Rank number
├── Error message
└── Alternative suggestions
```

### 5. AI Training Interface Layer
```
GameEngine (immutable operations)
├── CreateInitialState(playerIds) -> GameState
├── ApplyMove(state, cards) -> GameState
├── ApplyPass(state) -> GameState
├── GetLegalMoves(state) -> Card[][]
├── IsTerminal(state) -> boolean
├── GetWinner(state) -> number | null
├── CloneState(state) -> GameState
└── GetStateHash(state) -> string

AITrainingInterface
├── ExtractFeatures(state, playerId) -> number[]
├── GetReward(state, playerId) -> number
├── GetStateValue(state) -> number[]
├── RecordGamePosition(state, move, outcome)
├── GenerateTrainingData() -> TrainingExample[]
├── GetSymmetricStates(state) -> GameState[]
└── CompressGameHistory(states) -> CompressedGame
```

### 6. MCTS-Specific Components
```
MCTSNode
├── GameState reference
├── Parent node reference
├── Children nodes map
├── Visit count
├── Win count array (per player)
├── Untried moves list
├── Is fully expanded flag
└── UCB1 calculation methods

MCTSEngine
├── SelectNode(root) -> MCTSNode
├── ExpandNode(node) -> MCTSNode
├── SimulateGame(state) -> GameResult
├── BackpropagateResult(node, result)
├── GetBestMove(root) -> Card[]
├── SearchIterations(root, count)
└── GetPrincipalVariation(root) -> Card[][]
```

## AI Training Data Structures

### Feature Extraction System
```
FeatureExtractor
├── HandCompositionFeatures(hand) -> number[52]
├── PlayedCardsFeatures(playedCards) -> number[52]
├── PlayerPositionFeatures(state, playerId) -> number[]
├── TrickStateFeatures(trick) -> number[]
├── GamePhaseFeatures(state) -> number[]
├── OpponentModelingFeatures(state, playerId) -> number[]
├── CardCountingFeatures(state) -> number[]
└── CombineAllFeatures(state, playerId) -> number[]

TrainingExample
├── Features (input vector)
├── Move (target output)
├── Outcome (game result)
├── Player perspective
├── Turn number
└── Game phase
```

### Neural Network Integration
```
NeuralNetworkInterface
├── EvaluatePosition(features) -> PolicyValue
├── TrainOnBatch(examples) -> TrainingMetrics
├── SaveModel(path) -> void
├── LoadModel(path) -> void
├── GetPolicyPrediction(features) -> number[]
├── GetValuePrediction(features) -> number
└── UpdateWeights(gradients) -> void

PolicyValue
├── Policy (move probabilities)
├── Value (position evaluation)
├── Confidence score
└── Computation time
```

## Performance Optimizations for AI

### Memory Management
```
StatePool
├── Pre-allocated GameState objects
├── Reuse during simulations
├── Reset methods for cleanup
├── Memory usage tracking
└── Garbage collection optimization

TranspositionTable
├── State hash -> evaluation cache
├── LRU eviction policy
├── Collision handling
├── Memory size limits
└── Hit rate statistics
```

### Simulation Speed Optimizations
```
FastSimulator
├── Lightweight random playouts
├── Reduced state copying
├── Bitwise operations for cards
├── Minimal object allocation
├── Vectorized computations
└── Profile-guided optimizations

MoveOrdering
├── History heuristic
├── Killer move tracking
├── Principal variation ordering
├── Capture prioritization
└── Dynamic move scoring
```

## Enhanced Interfaces for AI

### Core AI Game Interface
```typescript
interface IAIGameEngine {
  // Immutable state operations
  createInitialState(playerIds: string[]): GameState;
  applyMove(state: GameState, cards: Card[]): GameState;
  applyPass(state: GameState): GameState;
  
  // AI training essentials
  getLegalMoves(state: GameState): Card[][];
  isTerminal(state: GameState): boolean;
  getReward(state: GameState, playerId: string): number;
  extractFeatures(state: GameState, playerId: string): number[];
  
  // Performance optimizations
  getStateHash(state: GameState): string;
  cloneState(state: GameState): GameState;
  getMoveCount(state: GameState): number;
}
```

### MCTS Integration Interface
```typescript
interface IMCTSEngine {
  search(rootState: GameState, iterations: number): Card[];
  getBestMove(rootState: GameState): Card[];
  getSearchStatistics(): MCTSStatistics;
  setEvaluationFunction(fn: (state: GameState) => number): void;
  setSimulationPolicy(policy: (state: GameState) => Card[]): void;
}
```

### Training Data Interface
```typescript
interface ITrainingDataCollector {
  recordPosition(state: GameState, move: Card[], outcome: GameResult): void;
  generateTrainingBatch(batchSize: number): TrainingExample[];
  saveTrainingData(filepath: string): void;
  loadTrainingData(filepath: string): TrainingExample[];
  getDatasetStatistics(): DatasetStats;
}
```

## Testing Strategy for AI Components

### AI-Specific Test Coverage
- **State Immutability**: Verify no mutations during operations
- **Move Generation**: Complete legal move coverage
- **Feature Extraction**: Consistent feature vectors
- **MCTS Correctness**: Tree building and backpropagation
- **Performance Benchmarks**: Simulation speed targets
- **Memory Usage**: No leaks during long simulations

### AI Training Test Data
- **Known Positions**: Manually evaluated game states
- **Symmetric Positions**: Equivalent states with different representations
- **Edge Cases**: Unusual but legal game situations
- **Performance Data**: Speed and memory usage metrics

## Implementation Phases (Updated)

### Phase 1: AI-Ready Foundation (Weeks 1-2)
- Immutable GameState system
- Fast Card representation with bitwise operations
- Pure function rule validation
- Basic performance benchmarks

### Phase 2: MCTS-Optimized Engine (Weeks 3-4)
- Legal move generation (complete and fast)
- State hashing and memoization
- Cloning and state management
- Memory pool for simulations

### Phase 3: Training Infrastructure (Weeks 5-6)
- Feature extraction system
- Training data collection
- Neural network interfaces
- Performance profiling tools

### Phase 4: AI Integration (Weeks 7-8)
- MCTS implementation
- Training data pipeline
- Model evaluation framework
- Benchmarking against rule-based AI

## Architecture Principles for AI

### Immutability First
- All game operations return new states
- No side effects in core functions
- Thread-safe by design
- Enables efficient memoization

### Performance Critical
- Target: 10,000+ simulations per second
- Minimal memory allocation during search
- Optimized data structures for frequent operations
- Profile-guided optimization

### Training Data Quality
- Capture complete game context
- Include player perspective in features
- Record timing and confidence metrics
- Support data augmentation techniques

### Extensible AI Architecture
- Pluggable evaluation functions
- Multiple AI strategy implementations
- A/B testing framework for AI improvements
- Tournament system for AI validation

This enhanced design ensures your game engine will efficiently support both MCTS and neural network training while maintaining clean, testable code architecture.