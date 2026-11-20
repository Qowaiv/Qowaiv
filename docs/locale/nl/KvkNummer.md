# Kvk-nummer
Represents a Dutch [KvK-nummer](https://nl.wikipedia.org/wiki/Kamer_van_Koophandel),
the number issued by the Kamer van Koophandel (Chamber of Commerce) to identify a
registered company or organisation. It has no checksum: validity is limited to
its numeric format.

A KvK-nummer is always rendered as eight digits; leading zeros are retained.
Formatting with `F` groups the digits as `dddd dddd`, separated by a non-breaking
space.

## usage
``` C#
var kvk = KvkNummer.Parse("87640540");
var created = KvkNummer.Create(8764_0540);
var fromInt = 8764_0540.KvK(); // humanizer extension

kvk.ToString();    // 87640540
kvk.ToString("F"); // 8764 0540
```
