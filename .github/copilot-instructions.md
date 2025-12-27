# Copilot Instructions for .NET Repositories

## Comment Formatting
- Limit all comment lines to **80 characters**.
- Wrap XML documentation comments (`///`) at 80 characters.
- Wrap block comments (`/* ... */`) and inline comments (`//`) at 80 characters.
- Prefer concise, high-signal phrasing to reduce line length.
- When generating multi-line comments, break lines cleanly at natural boundaries.

## XML Documentation Style
- Use standard .NET XML doc tags: `<summary>`, `<remarks>`, `<param>`, `<returns>`.
- Keep each line within `<summary>` and `<remarks>` under 80 characters.
- Avoid overly verbose summaries; aim for clarity and brevity.
- Follow the tone and structure of existing documentation in the project.

## C# Coding Style Alignment
- Follow common .NET conventions: PascalCase for public members, camelCase for locals.
- Prefer expression-bodied members when they improve readability.
- Avoid generating unused usings or unnecessary regions.
- Match the project's existing analyzer rules and formatting patterns.

## General Guidance
- Prioritize readability and maintainability.
- Keep generated comments aligned with the surrounding code's style.
- When unsure, default to minimal, clear comments rather than long explanations.
