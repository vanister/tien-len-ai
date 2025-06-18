# Backend Development

**Technology Stack:**
- Prefer .NET and C# for backend services.
- Follow .NET community best practices and conventions.
- Consider the specific requirements and existing infrastructure.
- Use MSTest for unit testing.
  - Try and aim for 80% coverage.
  - Make sure to test business rules/logic instead of boilerplate code
  - Ask me before writing tests
- Keep nuget/dotnet tools installs locally.

**Code Organization:**
- Apply clean architecture principles.
- Separate business logic from infrastructure concerns.
- Implement proper dependency injection.
- Follow established patterns like Repository, Unit of Work, etc.
- Use the simplified `namespace` syntax where the namespace is one line.
- Keep classes to one per file.

## Implementation Guidelines

**When Writing Code:**
1. Start with guard clauses and input validation.
2. Structure code with early returns to reduce nesting.
3. Write self-documenting code with clear variable names.
4. Add comments only where business logic requires explanation.
5. Consider how the code will be tested.
6. Review against SOLID principles before finalizing.
7. Do not make example usage file unless requested to do so.

**When Suggesting Solutions:**
- Provide rationale for technology choices.
- Consider scalability and maintainability.
- Suggest alternatives when they might be more appropriate.
- Always prioritize code readability and team productivity.
- Always ask before generating code.

**Code Quality & Structure:**
- Implement guard clauses first and return early.
- Avoid deeply nested `if` statements (max 2-3 levels).
- Never put return statements on the same line as `if` statements.
- Keep functions focused on a single responsibility.
- Format code to 100 characters per line maximum.
- Follow SOLID principles consistently.
- Design with testability in mind from the start.

**Code Style:**
- Keep it simple and concise - avoid "clever" or overly complex solutions.
- Limit comments to essential explanations of business logic or complex algorithms.
- Prefer explicit, readable code over terse implementations.
- Use descriptive variable and function names.
- Follow the rules in the `.editorconfig` file.
- Use collection expression when possible.
