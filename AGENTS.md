# AGENTS.md

Qowaiv is a (Single) Value Object library for DDD (Date, EmailAddress, IBAN, Money, Percentage, UUID, etc.), with serialization (JSON, SQL) and code-generation support. MIT licensed, NuGet package `Qowaiv`.

## Layout
- `qowaiv.slnx` - solution.
- `src/` - libraries: `Qowaiv` (core), `Qowaiv.Data.SqlClient`, `Qowaiv.EntityFrameworkCore`, `Qowaiv.CodeGeneration*`, `Qowaiv.Diagnostics.Contracts` (analyzer used by core), `Qowaiv.TestTools`.
- `specs/` - tests: `Qowaiv.Specs` (main), `Qowaiv.Specs.Generated`, `Qowaiv.Diagnostics.Contracts.Specs`, `Specs.EFCore`, `Bench` (BenchmarkDotNet).
- `tools/Qowaiv.SvoGenerator`, `shared/` (linked source files), `props/` (shared MSBuild props), `example/`.

## Build & test
- Build: `dotnet build qowaiv.slnx --configuration Release`
- Test: `dotnet test specs/Qowaiv.Specs --configuration Release`
- Pack: `dotnet pack qowaiv.slnx -c Release --output packages` (CI does this on `v*` tags)
- SDK pinned in `global.json` (10.0.x); core targets `netstandard2.0;net8.0;net9.0;net10.0`, tests `net8.0;net9.0;net10.0`. LangVersion 14; PolySharp supplies polyfills.

## Conventions
- Follow `.editorconfig`: CRLF, UTF-8, final newline, 4 spaces for `.cs`, 2 for `.csproj`/`.props`. Prefer expression-bodied members.
- Nullable is enabled. Analyzers (Sonar, StyleCop, Qowaiv.Analyzers, .NET analyzers) run during build; `.globalconfig` sets rule severities. Fix warnings rather than suppress.
- Spell-check dictionary: `dictionary.dic`.
- Central package management (`Directory.Packages.props`); lock files (`packages.lock.json`) are used and CI restores in locked mode, so commit updated lock files when changing dependencies.
- Assemblies are strong-name signed (`build/Qowaiv.snk`).

## Testing
- NUnit with AwesomeAssertions (FluentAssertions fork). Put tests in `specs/Qowaiv.Specs`, mirroring the type under test; use helpers from `Qowaiv.TestTools`.
- Public API is validated (ApiCompat strict, baseline in `Qowaiv.csproj`). Avoid breaking changes, including parameter renames.

## Releases
- Version, `PackageReleaseNotes` and the `ToBeReleased` notes live in `src/Qowaiv/Qowaiv.csproj`. Update the notes for user-facing changes.
