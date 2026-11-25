# BAG-identificatienummer
Represents a Dutch [BAG-identificatienummer](https://catalogus.kadaster.nl/bag/nl/page/?uri=Objectnummering),
the 16-digit unique object number issued within the Basisregistratie Adressen
en Gebouwen (BAG).

The number is composed of a four-digit municipality code (`gggg`), a two-digit
object type code (`tt`), and a ten-digit object sequence number (`nnnnnnnnnn`).

## usage
``` C#
var bagId = BagId.Parse("0363010123456789");
var created = BagId.Create(363_010_123_456_789L);
var human = 363_010_123_456_789L.BagId(); // humanizer extension

bagId.ToString();    // 0363010123456789
bagId.ToString("F"); // 0363.01.0123456789
```
