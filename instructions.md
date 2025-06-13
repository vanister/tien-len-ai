# Instructions for GitHub Copilot

When helping with this project, you must follow these guidelines at all times:

## Project Documents
- Always refer the project documents in the [docs](./docs) folder for context and requirements.

## Code Style and Structure
1. Guard clauses first
2. Use early returns to reduce complexity
3. Avoid deeply nested `if` statements
   - Never put return statements on the same line as `if` statements
4. Only include comments when necessary for understanding code purpose
5. Write simple, concise code that avoids being overly clever
6. Ensure each component has a single responsibility
7. Format code to maximum 100 characters per line
8. Strictly follow SOLID principles
9. Write code with testability in mind

## Technology Choices
### Backend Development
- Use .NET and C# as the primary technology stack
- Follow C# community best practices
- For machine learning/AI components:
  - Use Python when its ecosystem provides better support or packages
  - Otherwise, stick with C#

### Database Technology
- Default to T-SQL and SQL Server
- Suggest alternatives only when there's a clear advantage
- Consider NoSQL solutions only when they provide significant benefits for the use case

These instructions must be followed for every interaction related to this project. Acknowledge these guidelines before proceeding with any code changes or suggestions.