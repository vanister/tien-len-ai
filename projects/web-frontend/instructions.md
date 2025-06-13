# Frontend Development

**Build Tools & Testing:**
- Use Vite for bundling and development server.
- Use Vitest for unit testing.
- Always use the latest stable TypeScript version unless specified otherwise.

### TypeScript Standards

**Configuration & Formatting:**
- Check for `.prettierrc` file first and follow its rules.
- Use 2-space indentation (no tabs).
- Always include parentheses around arrow function parameters.

**Programming Paradigms:**
- Follow functional programming principles whenever possible.
- Keep functions pure by default - justify any side effects explicitly.
- Prefer named function declarations over arrow functions for top-level functions.
- Use arrow functions for simple inline operations and callbacks.
- Always destructure objects when extracting multiple properties.

**Example:**

```typescript
// Preferred - named function declaration
export default function calculateTotal(items: Item[]): number {
  if (!items.length) return 0;
  
  const { price, tax } = getCalculationParams();
  return items.reduce((sum, item) => sum + item.price, 0) * (1 + tax);
}

// Use arrow functions for simple operations
const processItems = items.map((item) => ({ ...item, processed: true }));
```

### React Best Practices

**Component Structure:**
- Define components as default exported functions: `export default function ComponentName(props: Props) {}`.
- Apply the same pattern to custom hooks.
- Keep each component focused on a single responsibility.
- Move business logic to custom hooks or utility functions.
- Keep component code focused on rendering and UI concerns.

**Architecture:**
- Follow React community best practices and latest patterns.
- Use custom hooks for stateful logic.
- Separate concerns: components render, hooks manage state, utilities handle business logic.

### CSS Approach

**Project-Based Selection:**
- Use CSS Modules for simple projects or component libraries.
- Use Tailwind CSS for larger applications requiring design system consistency.
- Consider the project's design requirements and team preferences.

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
