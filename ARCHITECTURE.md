# Architecture/Design
This document is intended to write down architectural and design decisions.
It is intended for both contributors and [agents](https://en.wikipedia.org/wiki/AI_agent).

## Documentation
The `docs` contain information about the package that is targeted to the
users. It explains the types and their methods, illustrated with examples and
relevant (Wikipedia) URLs.

## Extensions
The `src/Qowaiv/Extensions` directory contains `extension` methods and properties that
act as interactions between the core libraries of .NET and Qowaiv. They should
have the namespace `Qowaiv.Extensions`, a filename that reflects the type
that is extended, including its namespace (e.g. `System.String.cs`). The static
class should have a name that starts with `Qowaiv`, followed by the type name,
without any suffix (e.g. `QowaivDateTime` for `DateTime` and `QowaivType` for
`Type`). For extensions on an array or collection the type name is plural
(e.g. `QowaivBytes` for `byte[]`).

Linq-like extensions live in `src/Qowaiv/Linq` and are part of the static class `QowaivEnumerable`
in namespace `Qowaiv.Linq`.

Although `DecimalMath` (in `Qowaiv.Mathematics`) exposes all methods also as extensions, it is primarily a
utility class that provides `System.Math` functions on `decimal`. Its extensions are available in
the `Qowaiv.Mathematics` namespace (and via `global using Qowaiv.Mathematics;`).

## Humanizer
Inspired by [Humanizer.NET](https://humanizr.net), Qowaiv provides humanizer
extension methods in the `Qowaiv.Humanizer` namespace that help to create SVO instances.
All humanizer extensions are consolidated into the single `public static partial class Humanize` in `src/Qowaiv/Humanizer/`
(split across multiple files per target type). The namespace remains `Qowaiv.Humanizer`.

## Polyfill
The `src/Qowaiv/Polyfill` directory contains both [polyfill](https://en.wikipedia.org/wiki/Polyfill_(programming))
implementations and internal extensions that allow writing cleaner code but are
not part of Qowaiv's public API. None of this code should be `public`. Polyfills are typically
conditional on target frameworks (`#if !NET8_0_OR_GREATER`, `#if NETSTANDARD2_0`, etc.).
