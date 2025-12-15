# Gemini Code Guidelines by Abdullah Hanafi using Sabrina Ramonov Template

## Implementation Best Practices

### 0 — Purpose 

These rules ensure maintainability, safety, and developer velocity within a .NET environment.
**MUST** rules are enforced by the CI pipeline; **SHOULD** rules are strongly recommended.

---

### 1 — Before Coding

- **BP-1 (MUST)** Ask the user clarifying questions to resolve ambiguities.
- **BP-2 (SHOULD)** Draft and confirm an approach for complex work.
- **BP-3 (SHOULD)** If ≥ 2 approaches exist, list clear pros and cons.

---

### 2 — While Coding

- **C-1 (MUST)** Follow TDD: scaffold a stub method or class -> write a failing test -> implement the logic.
- **C-2 (MUST)** Name methods and classes using existing domain vocabulary for consistency.
- **C-3 (SHOULD NOT)** Introduce new classes for simple, stateless logic. Prefer static helper methods in a utility class instead.
- **C-4 (SHOULD)** Prefer simple, composable, and easily testable methods.
- **C-5 (MUST)** Prefer strongly-typed IDs over primitives. Use a `record struct` for type safety and performance.
  ```csharp
  public readonly record struct UserId(Guid Value); // ✅ Good: Type-safe and clear
  public Guid UserId { get; set; }                  // ❌ Bad: Just a primitive
  ```
- **C-6 (MUST)** Use `global using` directives in a central file (e.g., `GlobalUsings.cs`) for common namespaces to reduce clutter.
- **C-7 (SHOULD NOT)** Add comments except for critical caveats or to generate XML documentation. Rely on self-explanatory code.
- **C-8 (SHOULD)** Use the right tool for the job: `class` for general objects, `interface` for contracts, `record` for immutable DTOs, and `struct` for small, value-type data aggregates.
- **C-9 (SHOULD NOT)** Extract a new method unless it will be reused, is the only way to unit-test otherwise untestable logic, or drastically improves the readability of a complex block.

---

### 3 — Testing

- **T-1 (MUST)** Place tests in a dedicated test project that mirrors the source project's structure (e.g., `MyProject.Application` -> `MyProject.Application.Tests`).
- **T-2 (MUST)** For any API change, add or extend integration tests in the dedicated integration test project (e.g., `tests/MyApi.IntegrationTests/`).
- **T-3 (MUST)** ALWAYS separate pure-logic unit tests from infrastructure-dependent integration tests (e.g., those touching a database or making network calls).
- **T-4 (SHOULD)** Prefer integration tests with real dependencies (e.g., using `Testcontainers` or an in-memory database) over heavy mocking.
- **T-5 (SHOULD)** Unit-test complex algorithms thoroughly.
- **T-6 (SHOULD)** Test the entire structure in one assertion where possible, using libraries like **FluentAssertions**.
  ```csharp
  // ✅ Good
  result.Should().BeEquivalentTo(expectedValue);

  // ❌ Bad
  result.Should().HaveCount(1);
  result[0].Should().Be(expectedValue.First());
  ```

---

### 4 — Database

- **D-1 (MUST)** Design data access methods to accept a `DbContext` instance, allowing them to participate in transactions managed by a Unit of Work pattern.
- **D-2 (SHOULD)** Use Entity Framework Core's Fluent API (`OnModelCreating`) to override incorrect or incomplete conventions (e.g., specifying precision for a `decimal` property).

---

### 5 — Code Organization

- **O-1 (MUST)** Place code in a shared class library project (e.g., `MySolution.Shared`) only if it is used by ≥ 2 other projects in the solution.

---

### 6 — Tooling Gates

- **G-1 (MUST)** `dotnet format --verify-no-changes` passes.
- **G-2 (MUST)** `dotnet build` passes without errors.

---

### 7 — Git

- **GH-1 (MUST)** Use Conventional Commits format when writing commit messages: https://www.conventionalcommits.org/en/v1.0.0
- **GH-2 (SHOULD NOT)** Refer to Gemini or Google in commit messages.

---

## Writing Methods Best Practices

When evaluating whether a method you implemented is good, use this checklist:

1.  Can you read the method and HONESTLY follow what it's doing? If yes, stop here.
2.  Does the method have high cyclomatic complexity (e.g., many nested `if`/`else`/`switch` blocks)? If so, it's a candidate for refactoring.
3.  Could common data structures or algorithms simplify this method?
4.  Are there any unused parameters?
5.  Are there any unnecessary type casts?
6.  Is the method easily testable without mocking core framework features? If not, can it be tested as part of an integration test?
7.  Does it have hidden dependencies that could be injected via its constructor or passed as arguments?
8.  Brainstorm 3 better method names. Is the current one the best and most consistent with the rest of the codebase?

**IMPORTANT**: You SHOULD NOT extract a separate method unless there's a compelling need.

---

## Writing Tests Best Practices

When evaluating whether a test you've implemented is good, use this checklist:

1.  **SHOULD** use test case theories (`[Theory]`, `[TestCase]`) to parameterize inputs; avoid unexplained magic values.
2.  **SHOULD NOT** add a test unless it can fail for a real defect.
3.  **SHOULD** ensure the test description (`[Fact(DisplayName = "...")]`) states exactly what the assertion verifies.
4.  **SHOULD** compare results to independent, pre-computed expectations.
5.  **SHOULD** follow the same style and analysis rules as production code.
6.  **SHOULD** express invariants or axioms using property-based testing libraries like **FsCheck** where practical.
    ```csharp
    [Property]
    public void ConcatenationLengthIsSumOfLengths(string a, string b)
    {
        // Property: The length of a concatenated string is the sum of the original lengths.
        Assert.Equal((a + b).Length, a.Length + b.Length);
    }
    ```

7.  **SHOULD** group unit tests for a class in a test class named `[ClassName]Tests`.
8.  **SHOULD** use argument matchers (e.g., `It.IsAny<string>()` in Moq) for parameters that are not relevant to the specific behavior being tested.
9.  **ALWAYS** use strong assertions (e.g., `Should().Be(1)`) over weaker ones (`Should().BeGreaterThanOrEqualTo(1)`).
10. **SHOULD** test edge cases, realistic inputs, invalid inputs, and boundary values.
11. **SHOULD NOT** test conditions already guaranteed by the C# type system (e.g., passing `null` to a non-nullable reference type parameter).

---

## Code Organization (Example .NET Solution)

A typical .NET solution following Clean Architecture principles might look like this:

- `MySolution.sln`
- `src/`
    - `MyApi/` - ASP.NET Core Web API project
    - `MySolution.Application/` - Application logic, services, commands, queries
    - `MySolution.Domain/` - Core domain entities and business rules
    - `MySolution.Infrastructure/` - EF Core `DbContext`, external service clients
    - `MySolution.Shared/` -  DTOs ,Shared , enums and utilities
- `tests/`
    - `MySolution.Application.Tests/` - Unit tests for the application layer
    - `MySolution.Domain.Tests/` - Unit tests for the domain layer
    - `MyApi.IntegrationTests/` - Integration tests for the API

---

## Remember Shortcuts

Remember the following shortcuts which the user may invoke at any time.

### QNEW

When I type "qnew", this means:
`Understand all BEST PRACTICES listed in GEMINI.md. Your code SHOULD ALWAYS follow these best practices.`

### QPLAN

When I type "qplan", this means:
`Analyze similar parts of the codebase and determine whether your plan: is consistent, introduces minimal changes, and reuses existing code don't code.`

### QCODE

When I type "qcode", this means:
`Implement your plan. Ensure new tests pass and existing tests are not broken. Run "dotnet format" on any new or modified files.`

### QCHECK

When I type "qcheck", this means:
```
You are a SKEPTICAL senior software engineer. Perform this analysis for every MAJOR code change you introduced (skip minor changes):
1. GEMINI.md checklist "Writing Methods Best Practices".
2. GEMINI.md checklist "Writing Tests Best Practices".
```

### QCHECKF

When I type "qcheckf", this means:
```
You are a SKEPTICAL senior software engineer. Perform this analysis for every MAJOR method you added or edited (skip minor changes):
1. GEMINI.md checklist "Writing Methods Best Practices".
```

### QCHECKT

When I type "qcheckt", this means:
`You are a SKEPTICAL senior software engineer. Perform this analysis for every MAJOR test you added or edited (skip minor changes):`
`1. GEMINI.md checklist "Writing Tests Best Practices".`

### QUX

When I type "qux", this means:
```
Imagine you are a human UX tester of the feature you implemented. 
Output a comprehensive list of scenarios you would test, sorted by highest priority.
```

### QGIT

When I type "qgit", this means:
```
Add all changes to staging, create a commit, and push to remote.

Follow this checklist for writing your commit message:
- do not refer to Gemini or Google in the message.
- SHOULD use Conventional Commits format: https://www.conventionalcommits.org/en/v1.0.0
- SHOULD NOT refer to Claude or Anthropic in the commit message.
- SHOULD structure commit message as follows:
  <type>[optional scope]: <description>
  [optional body]
  [optional footer(s)]
- commit SHOULD contain the following structural elements to communicate intent:
  fix: a commit of the type fix patches a bug in your codebase (this correlates with PATCH in Semantic Versioning).
  feat: a commit of the type feat introduces a new feature to the codebase (this correlates with MINOR in Semantic Versioning).
  BREAKING CHANGE: a commit that has a footer BREAKING CHANGE:, or appends a ! after the type/scope, introduces a breaking API change (correlating with MAJOR in Semantic Versioning). A BREAKING CHANGE can be part of commits of any type.
  types other than fix: and feat: are allowed, for example @commitlint/config-conventional (based on the Angular convention) recommends build:, chore:, ci:, docs:, style:, refactor:, perf:, test:, and others.
  footers other than BREAKING CHANGE: <description> may be provided and follow a convention similar to git trailer format.
  ```
