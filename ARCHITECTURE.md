# Architecture/Design
This document is intended to write down architectural and design descisions.
Is is intended for both contributers as [agents](https://en.wikipedia.org/wiki/AI_agent).

## Documentation
The `docs` contain information about the packages that is targeted to the
users. It explains the types and their methods, illustrated with examples and
relevant (Wikipedia) URLs.

## Extensions
The `Extensions` directory contains `extension` methods and properties that
act as interaction between the core libaries of .NET and Qowaiv. the all should
have the namespace `Qowaiv.Extensions`, a filename that reflects the type
that is exctend, including its namespace. The static class should have a name
that starts with `Qowaiv` and is followed by the type name. This type can be
plural in case of an extension an array or collection. Linq like extensions
should be in the `Linq` directory, and part of  `QowaivEnumerable`.
Although `DecimalMath` exposes all methods also as extensions, it is primary a
utility class that provides `Symstem.Math` functions on `decimal`.

## Polyfill
The `Polyfill` directory contains both [polyfill](https://en.wikipedia.org/wiki/Polyfill_(programming))
implementations, as internal extensions that allow write cleaner code but are
not part of Qowaiv's public API. None of this code should be `public`.
