# TienLenAi2 Development Instructions

This document outlines the development guidelines and best practices for the TienLenAi2 solution.

## Core Development Rules

### 1. Follow .NET and C# Best Practices

- **Naming Conventions**: Use PascalCase for classes, methods, and properties; camelCase for fields and local variables
- **Code Organization**: Follow the standard .NET project structure and namespace conventions
- **SOLID Principles**: Design classes following Single Responsibility, Open/Closed, Liskov Substitution, Interface Segregation, and Dependency Inversion principles
- **Async/Await**: Use async/await patterns properly for asynchronous operations
- **Nullable Reference Types**: Leverage C# nullable reference types (enabled in this project) to prevent null reference exceptions
- **Exception Handling**: Use appropriate exception types and handle exceptions at the right level
- **Resource Management**: Use `using` statements and implement `IDisposable` when managing resources

### 2. Use Defensive Coding Techniques

- **Input Validation**: Always validate method parameters, especially public APIs
- **Null Checks**: Perform null checks on arguments and return values where appropriate
- **Guard Clauses**: Use guard clauses to fail fast and make code more readable
- **Immutability**: Prefer immutable objects and readonly fields where possible
- **Bounds Checking**: Validate array/collection indices and ranges
- **State Validation**: Ensure objects are in valid states before operations
- **Exception Safety**: Write exception-safe code that maintains object integrity

Example defensive coding pattern:
```csharp
public void ProcessCards(IEnumerable<Card> cards)
{
    ArgumentNullException.ThrowIfNull(cards);
    
    var cardList = cards.ToList();
    if (cardList.Count == 0) {
        throw new ArgumentException("Card collection cannot be empty", nameof(cards));
    }
    
    // Process cards...
}
```

### 3. Comments Only When Necessary

- **Self-Documenting Code**: Write code that explains itself through clear naming and structure
- **When to Comment**: 
  - Complex business logic or algorithms
  - Non-obvious design decisions
  - Public API documentation (XML comments)
  - Workarounds or temporary solutions
- **When NOT to Comment**:
  - Obvious code that speaks for itself
  - Redundant descriptions of what the code does
  - Outdated or misleading comments

### 4. Confirm Before Writing Code

- **Approval Process**: Always confirm implementation approach before writing significant code changes
- **Design Discussion**: Discuss architectural decisions and design patterns before implementation
- **Breaking Changes**: Confirm any changes that might affect existing functionality
- **New Dependencies**: Get approval before adding new NuGet packages or external dependencies

### 5. Prefer Concise Code Over Crafty Code

- **Readability First**: Write code that is easy to read and understand
- **Simple Solutions**: Choose straightforward implementations over clever ones
- **Clear Intent**: Make the code's purpose obvious to other developers
- **Maintainability**: Prioritize code that is easy to modify and extend
- **Avoid Over-Engineering**: Don't add complexity unless it's clearly needed
- **Use Curly Braces**: Always use curly braces for code blocks, even for single statements

Examples of concise vs crafty code:

**Concise (Preferred):**
```csharp
public bool IsValidCard(Card card)
{
    return card != null && card.IsValid();
}

if (condition)
{
    DoSomething();
}
```

**Crafty (Avoid):**
```csharp
public bool IsValidCard(Card card) => card?.IsValid() ?? false;

if (condition) DoSomething(); // Missing braces
```

## Project Structure

### TienLenAi2.Core ### 

Core game logic and domain models. Keep a focus on training an Ai model to play the game.

- Keep business logic separate from infrastructure concerns
- Use dependency injection for loose coupling
- Follow clean architecture principles
- Keep state immutable

## Code Quality

- Write unit tests for business logic
- Use meaningful variable and method names
- Keep methods focused and small
- Prefer composition over inheritance
- Use interfaces to define contracts

## Review Checklist

Before submitting code, ensure:
- [ ] Code follows naming conventions
- [ ] Input validation is implemented
- [ ] No obvious security vulnerabilities
- [ ] Code is self-documenting with minimal comments
- [ ] Implementation approach was confirmed
- [ ] Code is concise and readable
