# TienLenAi2 Development Instructions

> **Project Goal**: Build an AI engine for Tiến Lên (Vietnamese Thirteen) card game

This document outlines development guidelines and best practices for the TienLenAi2 solution.

---

## Game Design and Rules

Familiarize yourself with the technical design of the game engine in [./docs/game-design.md](./docs/game-design.md).
The game rules are in the [./docs/game-rules.md](./docs/game-rules.md).

## 🎯 Core Development Rules

### 1. .NET and C# Best Practices

#### Naming Conventions
- **PascalCase**: Classes, methods, properties (`Card`, `GetValidMoves()`, `PlayerHand`)
- **camelCase**: Fields, local variables (`cardCount`, `isValid`)

#### Code Organization
- **One Type Per File**: Each class, enum, interface, or record in separate files
- **SOLID Principles**: Single Responsibility, Open/Closed, Liskov Substitution, Interface Segregation, Dependency Inversion
- **Namespace Structure**: Follow `TienLenAi2.Core.{Domain}` pattern

#### Modern C# Features
- **Nullable Reference Types**: Enabled in this project - use appropriately
- **Records**: For immutable data structures (cards, game states)
- **Pattern Matching**: For game logic and card evaluation
- **Async/Await**: For any asynchronous operations

#### Resource Management
- Use `using` statements for disposable resources
- Implement `IDisposable` when managing unmanaged resources

---

### 2. Defensive Coding Techniques

> **Principle**: Fail fast and maintain object integrity

#### Input Validation
```csharp
public void ProcessCards(IEnumerable<Card> cards)
{
    ArgumentNullException.ThrowIfNull(cards);
    
    var cardList = cards.ToList();
    if (cardList.Count == 0)
    {
        throw new ArgumentException("Card collection cannot be empty", nameof(cards));
    }
    
    // Process cards...
}
```

#### Key Practices
- **Guard Clauses**: Validate inputs early and return/throw immediately
- **Null Checks**: Perform null checks on arguments and return values
- **Immutability**: Prefer immutable objects and `readonly` fields
- **Bounds Checking**: Validate array/collection indices and ranges
- **State Validation**: Ensure objects are in valid states before operations
- **Exception Safety**: Write code that maintains integrity even when exceptions occur

---

### 3. Comments - Only When Necessary

#### When TO Comment
- ✅ Complex game algorithms (e.g., hand evaluation logic)
- ✅ Non-obvious design decisions
- ✅ Workarounds or temporary solutions

#### When NOT to Comment
- ❌ Obvious code that speaks for itself
- ❌ Redundant descriptions of what code does
- ❌ Outdated or misleading comments
- ❌ Don't comment unless I ask you to

**Goal**: Write self-documenting code through clear naming and structure.

---

### 4. Confirmation Process

#### Before Writing Significant Code
- **Approval Process**: Confirm implementation approach
- **Design Discussion**: Discuss architectural decisions and design patterns
- **Breaking Changes**: Confirm changes that affect existing functionality
- **New Dependencies**: Get approval before adding NuGet packages

#### What Qualifies as "Significant"
- New classes or major refactoring
- Changes to public APIs
- New algorithms or game logic
- Performance-critical code

---

### 5. Approval Before Implementation

> **Principle**: Always confirm implementation direction before writing code

#### Requirement
- **Pre-Implementation Summary**: Provide a clear summary of the planned implementation approach
- **Wait for Approval**: Do not proceed with code writing until explicit approval
- **Include**: Expected classes/methods, key algorithms, and rationale for approach
- **Alternatives**: When relevant, outline alternative implementations considered

#### Example Process
1. Understand the requirement
2. Formulate an implementation plan
3. Present the plan in clear, concise terms
4. Wait for approval
5. Proceed with implementation after receiving explicit approval

#### Benefits
- Prevents unnecessary work on incorrect approaches
- Ensures alignment with project vision
- Reduces need for major refactoring
- Improves overall code quality through collaborative design

---

### 6. Concise Over Crafty Code

> **Principle**: Readability and maintainability over cleverness

#### Preferred Style ✅
```csharp
public bool IsValidCard(Card card)
{
    return card != null && card.IsValid();
}

if (hasValidMove)
{
    PlayCard();
}
```

#### Avoid This Style ❌
```csharp
public bool IsValidCard(Card card) => card?.IsValid() ?? false;

if (hasValidMove) PlayCard(); // Missing braces
```

#### Guidelines
- **Readability First**: Code should be easy to read and understand
- **Simple Solutions**: Choose straightforward implementations
- **Clear Intent**: Make the code's purpose obvious
- **Maintainability**: Prioritize code that's easy to modify and extend
- **Avoid Over-Engineering**: Don't add complexity unless clearly needed
- **Always Use Braces**: Even for single statements
- **Prefer LiNQ over loops**: unless loops make more sense or is clearer in intent

---

## 🏗️ Project Structure

### TienLenAi2.Core

**Purpose**: Core game logic and domain models for Tiến Lên card game

#### Focus Areas
| Area                      | Description                                           |
| ------------------------- | ----------------------------------------------------- |
| **Game State Management** | Tracking players, hands, tricks, and game progression |
| **Rule Validation**       | Ensuring moves comply with Tiến Lên rules             |
| **Hand Evaluation**       | Determining valid plays and hand rankings             |
| **AI Training Support**   | Providing game state representations for ML           |

#### Architecture Guidelines
- **Separation of Concerns**: Business logic separate from infrastructure
- **Dependency Injection**: Loose coupling between components
- **Clean Architecture**: Domain-driven design principles
- **Immutability**: Support AI training scenarios with immutable state
- **Domain Modeling**: Game entities as immutable records where possible

#### Suggested Structure
```
TienLenAi2.Core/
├── Cards/          # Card related entities
├── Hands/          # Hand, HandFactory, etc.
├── States/         # GameState, TrickState, etc.
├── Players/        # Player related entities
├── Rules/          # Rule validation components
├── Evaluators/     # Hand evaluation logic
└── Interfaces/     # Contracts and abstractions
```

---

## ✅ Code Quality Standards

### Testing Requirements
- **Unit Tests**: All business logic, especially game rule validation
- **Edge Cases**: Test invalid game states and boundary conditions
- **Game Scenarios**: Both valid and invalid game situations
- **Coverage**: Aim for high coverage on game logic

### Code Standards
- **Meaningful Names**: Use game terminology in variable/method names
- **Focused Methods**: Keep methods small and single-purpose
- **Composition Over Inheritance**: Prefer composition for flexibility
- **Interface Contracts**: Define clear contracts for game components
- **Robust Error Handling**: Ensure AI training data integrity

### Performance Considerations
- **Immutable Collections**: Use when appropriate for game state
- **Memory Efficiency**: Important for AI training scenarios
- **Algorithmic Efficiency**: Hand evaluation should be fast

---

## 📋 Review Checklist

Before submitting code, verify:

#### Code Standards
- [ ] Follows naming conventions (PascalCase/camelCase)
- [ ] Input validation implemented with guard clauses
- [ ] No security vulnerabilities
- [ ] Self-documenting code with minimal comments
- [ ] Implementation approach was confirmed
- [ ] Code is concise and readable

#### Game-Specific Requirements
- [ ] Game rules correctly implemented (reference [`docs/game-rules.md`](docs/game-rules.md))
- [ ] Edge cases and invalid states properly handled
- [ ] Tests cover valid and invalid game scenarios
- [ ] Game terminology used in naming
- [ ] AI training considerations addressed

#### Technical Requirements
- [ ] Nullable reference types used appropriately
- [ ] Immutability preferred where applicable
- [ ] SOLID principles followed
- [ ] Exception safety maintained
- [ ] Performance implications considered

---

*This document is a living guide - update it as the project evolves.*
