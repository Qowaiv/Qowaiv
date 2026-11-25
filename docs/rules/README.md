# Rules
* [**QW0001** - Use a testable Time Provider](QW0001.md)
* [**QW0003** - Decorate pure functions](QW0003.md)
* [**QW0004** - Characters with Trojan Horse potential are not allowed](QW0004.md)
* [**QW0005** - Seal concrete classes unless designed for inheritance](QW0005.md)
* [**QW0006** - Only unsealed concrete classes should be decorated as inheritable](QW0006.md)
* [**QW0007** - Use file-scoped namespace declarations](QW0007.md)
* [**QW0008** - Define properties as not-nullable for types with a defined empty state](QW0008.md)
* [**QW0009** - Define properties as not-nullable for enums with a defined none/empty value](QW0009.md)
* [**QW0010** - Use System.DateOnly instead of Qowaiv.Date](QW0010.md)
* [**QW0011** - Define properties as immutables](QW0011.md)
* [**QW0012** - Use immutable types for properties](QW0012.md)
* [**QW0013** - Use Qowaiv decimal rounding](QW0013.md)
* [**QW0014** - Define global using statements separately](QW0014.md)
* [**QW0015** - Define global using statements in single file](QW0015.md)
* [**QW0016** - Prefer regular over positional properties](QW0016.md)
* [**QW0017** - Apply arithmetic operations on non-nullables only](QW0017.md)
* [**QW0018** - Use Qowaiv.Clock.TimeProvider](QW0018.md)
* [**QW0019** - Prefer XML to LINQ over DOM](QW0019.md)
* [**QW0020** - Prefer System.Text.Json over Newtonsoft.Json](QW0020.md)
* [**QW0021** - Use leading zeros to define date constants](QW0021.md)
* [**QW0022** - Use compliant GUID literals](QW0022.md)
* [**QW0023** - Provide GUID values](QW0023.md)
* [**QW0024** - Create GUID's using Guid.Parse or Guid.Empty](QW0024.md)
* [**QW0025** - Use compliant UUID literals](QW0025.md)
* [**QW0026** - Provide UUID values](QW0026.md)
* [**QW0027** - Create UUID's using Uuid.Parse or Uuid.Empty](QW0027.md)
* [**QW0028** - Prefer strongly typed identifiers over GUIDs](QW0028.md)
* [**QW0029** - Prefer strongly typed identifiers over primitives](QW0029.md)
* [**QW0030** - Prefer single value objects over data annotations](QW0030.md)
* [**QW0031** - Format amount literals](QW0031.md)
* [**QW0032** - Round converted double values](QW0032.md)

## Data Annotation rules
* [**QW0100** - Define only one Required attribute](QW0100.md)
* [**QW0101** - Required attribute cannot invalidate value types](QW0101.md)
* [**QW0102** - Use compliant validation attributes](QW0102.md)
* [**QW0103** - Decorate validation attributes](QW0103.md)
* [**QW0104** - Use validates attribute on validation attributes only](QW0104.md)

# Code fixes
* Use Qowaiv.Clock ([QW0001](QW0001.md), [S6354](https://www.sonarsource.com))
* Seal class ([QW0005](QW0005.md))
* Change property type to not-nullable ([QW0008](QW0008.md), [QW0009](QW0009.md))
* Change type System.DateOnly ([QW0010](QW0010.md))
* Apply suggestions of obsolete code attribute [CS0618, CS0619](ObsoleteCode.md))
* Use Qowaiv round extensions ([QW0013](QW0013.md))
* Convert positional properties ([QW0016](QW0016.md))
* Use Qowaiv.Clock.TimeProvider ([QW0018](QW0018.md))
* Add leading zero(s) ([QW0021](QW0021.md))
* Generate GUIDs ([QW0022](QW0022.md), [QW0023](QW0023.md), [QW0024](QW0024.md))
* Generate UUIDs ([QW0025](QW0025.md), [QW0026](QW0026.md), [QW0027](QW0027.md))
* Format amount ([QW0031](QW0031.md))
* Use decimal suffix ([QW0032](QW0032.md))

# Configuration
Some of the rules are not intended for (unit) test code. Unit tests that set
`IsTestProject` to `true` are excluded. When setting `IsTestProject` is undesirable,
a project can set `IsTestCode` to `true` instead. It is worth to
mention that only a subset of the rules is considered irrelevant for test code,
the rest is always executed.
