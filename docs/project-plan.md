# Tiến Lên AI Project Plan

## Project Overview
Build an AI system capable of playing Tiến Lên (Vietnamese Thirteen) at a competitive level, with both game engine and intelligent decision-making components.

## Phase 1: Core Game Engine (4-6 weeks)

### 1.1 Game State Management
- **Card representation system**: Implement card objects with suit/rank properties
- **Hand management**: Player hand operations (add, remove, sort, validate)
- **Game state tracking**: Current player, round state, previous plays
- **Rule engine**: Legal move validation, hand type recognition

### 1.2 Game Logic Implementation
- **Hand types**: Singles, pairs, triples, straights, bombs (4-of-a-kind)
- **Ranking system**: Card hierarchy (3♠ lowest, 2♥ highest)
- **Turn mechanics**: Play validation, passing, trick resolution
- **Win conditions**: First player to empty hand wins

### 1.3 Basic Game Interface
- **Console-based gameplay**: Human vs human testing
- **Game state visualization**: Display hands, current play, game status
- **Input validation**: Ensure only legal moves are accepted

## Phase 2: AI Foundation (3-4 weeks)

### 2.1 Monte Carlo Tree Search (MCTS) Framework
- **Node structure**: Game state nodes with win/visit statistics
- **Selection**: UCB1 algorithm for node exploration
- **Expansion**: Add child nodes for legal moves
- **Simulation**: Random playouts from leaf nodes
- **Backpropagation**: Update statistics up the tree

### 2.2 Game State Evaluation
- **Heuristic functions**: Hand strength, card counting, positional advantage
- **Win probability estimation**: Based on remaining cards and game state
- **Performance metrics**: Track AI decision quality over time

## Phase 3: Advanced AI Strategies (4-5 weeks)

### 3.1 Card Counting & Memory
- **Played card tracking**: Remember all cards played in current game
- **Probability calculations**: Estimate opponent hand compositions
- **Information sets**: Handle imperfect information in decision making

### 3.2 Strategic Decision Making
- **Risk assessment**: When to play strong hands vs save for later
- **Opponent modeling**: Adapt strategy based on opponent behavior patterns
- **Endgame optimization**: Special strategies when few cards remain

### 3.3 Machine Learning Integration
- **Neural network evaluation**: Train network to evaluate positions
- **AlphaZero-style approach**: Self-play training with MCTS
- **Feature engineering**: Extract relevant game features for learning

## Phase 4: Web Interface (3-4 weeks)

### 4.1 Frontend Development (TypeScript/React)
- **Game board UI**: Visual card representation and game state
- **Player interaction**: Click-to-play interface, drag and drop
- **Real-time updates**: WebSocket integration for multiplayer
- **Responsive design**: Mobile and desktop compatibility

### 4.2 Backend API (C#/.NET)
- **Game session management**: Handle multiple concurrent games
- **AI integration**: Expose AI decision-making via API endpoints
- **Player authentication**: Basic user accounts and game history
- **WebSocket server**: Real-time game state synchronization

### 4.3 Database Layer (T-SQL/SQL Server)
- **Game history storage**: Store completed games for analysis
- **Player statistics**: Win rates, favorite strategies, performance metrics
- **AI training data**: Store game positions and outcomes for ML training

## Phase 5: Testing & Optimization (2-3 weeks)

### 5.1 AI Performance Testing
- **Benchmark tournaments**: AI vs AI, AI vs human players
- **Strategy validation**: Test different AI approaches and parameters
- **Performance profiling**: Optimize search algorithms and evaluation

### 5.2 Game Balance & UX
- **Difficulty levels**: Multiple AI strength settings
- **Game flow optimization**: Ensure smooth, engaging gameplay
- **Bug fixes**: Address edge cases and rule violations

## Technical Architecture

### Core Technologies
- **Backend**: C# .NET 8 with Entity Framework
- **Frontend**: TypeScript, React 18, TailwindCSS
- **Database**: SQL Server with T-SQL stored procedures
- **AI Engine**: Python scikit-learn, NumPy for ML components
- **Communication**: SignalR for real-time updates

### Key Components
1. **Game Engine** (C#): Core rules, state management, validation
2. **AI Engine** (Python): MCTS, neural networks, strategy algorithms
3. **Web API** (C#): RESTful endpoints, game session management
4. **Frontend** (TypeScript): React components, game visualization
5. **Database** (T-SQL): Game persistence, analytics, user management

## Success Metrics
- AI can beat intermediate human players 70%+ of the time
- Game completes without rule violations or crashes
- Web interface supports 4-player games with <200ms response time
- AI makes decisions within 3 seconds on average

## Risk Mitigation
- **Complex rule validation**: Start with simplified rules, add complexity incrementally
- **AI performance**: Begin with rule-based AI, add ML components gradually
- **Multiplayer synchronization**: Implement robust error handling and reconnection
- **Scalability**: Design stateless backend services for horizontal scaling

## Timeline Summary
- **Phase 1-2**: 7-10 weeks (Core engine + basic AI)
- **Phase 3**: 4-5 weeks (Advanced AI strategies)
- **Phase 4**: 3-4 weeks (Web interface)
- **Phase 5**: 2-3 weeks (Testing & polish)
- **Total**: 16-22 weeks

This plan provides a solid foundation for building a competitive Tiến Lên AI while maintaining flexibility for iterative improvements and feature additions.