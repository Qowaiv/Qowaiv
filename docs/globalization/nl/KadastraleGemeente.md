# Kadastrale gemeente
Represents a Dutch [kadastrale gemeente](https://nl.wikipedia.org/wiki/Kadastrale_gemeente)
via its [AKR-code](https://catalogus.kadaster.nl/brk/nl/page/KadastraleGemeente):
three letters followed by two digits (for example `DEL00` for Delft).

## usage
``` C#
var delft = KadastraleGemeente.Parse("DEL00");
var lowercase = KadastraleGemeente.Parse("del00"); // DEL00
```
