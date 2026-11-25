# BAG-identificatienummer
Represents a Dutch [BAG-identificatienummer](https://catalogus.kadaster.nl/bag/nl/page/?uri=Objectnummering),
the 16-digit unique object number issued within the Basisregistratie Adressen
en Gebouwen (BAG).

The number is composed of a four-digit municipality code (`gggg`), a two-digit
object type code (`tt`), and a ten-digit object sequence number (`nnnnnnnnnn`).
There is no checksum.

## Object type
The nested `BagId.ObjectType` enum maps the object type code to its meaning:

| ObjectType         | Code |
|--------------------|------|
| `Verblijfsobject`  |  01  |
| `Ligplaats`        |  02  |
| `Standplaats`      |  03  |
| `Pand`             |  10  |
| `Nummeraanduiding` |  20  |
| `OpenbareRuimte`   |  30  |

## Parts
A `BagId` exposes its three parts:

| Property     | Description                    | `0363.01.0123456789` |
|--------------|--------------------------------|----------------------|
| `Gemeente`   | Municipality code (`gggg`)     | `0363`               |
| `Object`     | Object type (`tt`)             | `Verblijfsobject`    |
| `Volgnummer` | Sequence number (`nnnnnnnnnn`) | `0123456789`         |

## Formatting
`ToString()` returns all 16 digits, including leading zeros. `ToString("F")`
groups them as `gggg.tt.nnnnnnnnnn`. `Unknown` is represented as `?` by both
formats, in JSON, and in debugger displays.

## Usage
``` C#
var bagId = BagId.Parse("0363010123456789");
var created = BagId.Create(363_010_123_456_789L);
var human = 363_010_123_456_789L.BagId(); // humanizer extension

bagId.Gemeente;           // 363
bagId.Object;             // BagId.ObjectType.Verblijfsobject
bagId.Volgnummer;         // 123456789
bagId.ToString();         // 0363010123456789
bagId.ToString("F");      // 0363.01.0123456789
```
