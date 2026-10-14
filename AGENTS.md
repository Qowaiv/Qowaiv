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

## Country languages
Country names are localized via `src/Qowaiv/Globalization/CountryLabels*.resx`. Supported languages:
- English (`en`, default/neutral `CountryLabels.resx`)
- Arabic (`ar`), German (`de`), Spanish (`es`), French (`fr`), Italian (`it`), Japanese (`ja`), Dutch (`nl`), Portuguese (`pt`), Russian (`ru`)
- Chinese (`zh`), Chinese Hong Kong (`zh-HK`)
- Taiwanese (`zh-TW`)

## Translations
- Add missing translations for all supported languages, except country labels (`CountryLabels*.resx`) and currency labels (`CurrencyLabels*.resx`).
- Do not translate messages used by exceptions (`QowaivMessages.resx`), with one exception: the `FormatException*` messages (e.g. `FormatExceptionDate`) must be translated into all supported languages.
- Non-linguistic values (e.g. `int_*` and `symbol_*` in `SexLabels`) are not translated.
- Default (neutral) resources are American English. Add a British English (`en-GB`) resource only when the spelling or convention differs (e.g. `-ise` vs `-ize`, `colour`, `centre`), and only for the differing keys. Skip country and currency labels.

## Testing
- NUnit with AwesomeAssertions (FluentAssertions fork). Put tests in `specs/Qowaiv.Specs`, mirroring the type under test; use helpers from `Qowaiv.TestTools`.
- Public API is validated (ApiCompat strict, baseline in `Qowaiv.csproj`). Avoid breaking changes, including parameter renames.

## Temporary files
- If a temp location is needed, use `.temp` in the repository root (not the OS temp directory).

## Releases
- Version, `PackageReleaseNotes` and the `ToBeReleased` notes live in `src/Qowaiv/Qowaiv.csproj`. Update the notes for user-facing changes.
