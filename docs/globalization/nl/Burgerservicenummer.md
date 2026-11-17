# Burgerservicenummer
Represents a Dutch [Burgerservicenummer (BSN)](https://nl.wikipedia.org/wiki/Burgerservicenummer),
the unique personal number the Dutch government assigns to citizens and
residents. It is equal to the former sofinummer, but has a broader legal basis.

## usage
``` C#
var bsn = Burgerservicenummer.Parse("123456782");
var created = Burgerservicenummer.Create(1234_56_782L);
var fromLong = 1234_56_782L.BSN(); // humanizer extension
var cast = (Burgerservicenummer)1234_56_782L;

bsn.ToString();    // 123456782
bsn.ToString("F"); // 1234.56.782
```
