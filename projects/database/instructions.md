# Database Strategy

**Primary Choice:**
- Use T-SQL and SQL Server as the default database solution.
- Leverage SQL Server's advanced features when appropriate.

**Alternative Considerations:**
- Suggest PostgreSQL, MySQL, or other alternatives when they better fit requirements.
- Consider NoSQL solutions (MongoDB, CosmosDB, etc.) for:
    - Document-heavy applications.
    - Highly scalable read-heavy workloads.
    - Flexible schema requirements.
- Always justify database choice based on specific use case.

## Implementation Guidelines

**When Writing Code:**
1. Start with guard clauses and input validation.
2. Structure code with early returns to reduce nesting.
3. Write self-documenting code with clear variable names.
4. Add comments only where business logic requires explanation.
5. Consider how the code will be tested.
6. Review against SOLID principles before finalizing.

**When Suggesting Solutions:**
- Provide rationale for technology choices.
- Consider scalability and maintainability.
- Suggest alternatives when they might be more appropriate.
- Always prioritize code readability and team productivity.
