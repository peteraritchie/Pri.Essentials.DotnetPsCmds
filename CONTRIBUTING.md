# Contributing to This .NET Project

Thank you for your interest in contributing! Whether you're fixing a bug,
improving documentation, or proposing a new feature, your contributions
help strengthen the project and the broader .NET ecosystem.

This guide explains how to work with the codebase, follow our conventions,
and collaborate effectively.

---

## Ways to Contribute

### 1. Report Issues
Use the **Issues** tab to report:
- Bugs  
- Performance regressions  
- API design concerns  
- Feature requests  
- Documentation improvements  

When reporting a bug:
- Include your .NET SDK/runtime version (`dotnet --info`)  
- Provide clear reproduction steps  
- Share expected vs. actual behavior  
- Add stack traces or minimal repro code when possible  

---

## Development Environment

### Prerequisites
- .NET SDK (LTS or STS version used by the project)  
- An IDE or editor such as Visual Studio, VS Code, or Rider  
- Git  

### Setup
```
git clone https://github.com/peteraritchie/Pri.Essentials.DotnetPsCmds.git
cd Pri.Essentials.DotnetPsCmds/src
dotnet restore
dotnet build
dotnet test
```

### Project Structure (example)
```
/src            → main libraries or application code  
/src/tests      → unit and integration tests  
```
---

## Testing Guidelines

We take test coverage and reliability seriously.

- Use **xUnit**.
- Follow the Arrange–Act–Assert pattern (although verbose comments like
`// Arrange` are not necessary
- Prefer deterministic tests—avoid time‑dependent or environment‑dependent behavior  
- Add tests for:
  - New features  
  - Bug fixes  
  - Edge cases  
  - Public API changes  

Run the full test suite before submitting a PR:
```cmd
dotnet test
```

---

## Coding Standards

### Style
- Follow the project’s `.editorconfig`  
- Use `dotnet format` to apply consistent formatting  
- Prefer modern C# features when they improve clarity (e.g., pattern matching, `using` declarations, `Span<T>` when appropriate)  
- Keep public APIs clean, predictable, and well‑documented  

### Documentation
- Use XML documentation comments for public APIs  
- Update `/docs` or `/README.md` when behavior changes  
- Include examples when adding new features  

### Performance
If your change affects performance:
- Add benchmarks using **BenchmarkDotNet**  
- Avoid unnecessary allocations  
- Consider async/await best practices  
- Validate behavior across different runtimes (e.g., Windows, Linux, macOS) if relevant  

---

## Pull Request Process

### Before You Start
- Check for an existing issue  
- Discuss large or breaking changes with maintainers  
- Ensure your proposal aligns with the project’s goals  

### Branching
```cmd
git checkout -b feature/my-change
```

### PR Checklist
- [ ] Linked to an issue (e.g., “Fixes #123”)  
- [ ] Builds successfully (`dotnet build`)  
- [ ] Tests pass (`dotnet test`)  
- [ ] New tests added where appropriate  
- [ ] Documentation updated  
- [ ] Commit messages are clear and descriptive  

### CI
All PRs must pass automated checks, including:
- Build validation  
- Test execution  
- Static analysis (e.g., Roslyn analyzers, StyleCop, Sonar, or project‑specific rules)  

---

## Versioning & Releases

We follow **Semantic Versioning (SemVer)**:
- **MAJOR**: breaking changes  
- **MINOR**: new features, no breaking changes  
- **PATCH**: bug fixes  

Release notes are generated from merged PRs and issue history.

---

## Code of Conduct

By participating, you agree to uphold our community standards.  
See **CODE_OF_CONDUCT.md** for details.

---

## Getting Help

If you need support:
- Comment on the issue you’re working on  
- Start a thread in **Discussions**  
- Ask maintainers for clarification when needed  

We’re excited to collaborate with you and appreciate your contributions to the .NET community.
