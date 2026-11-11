# Postal code
Represents a postal code. It supports validation for all countries.

``` C#
var dutch = PostalCode.Parse("2624DP");
dutch.IsValid(Country.NL); // true
dutch.IsValid(Country.BE); // false

var argentina = PostalCode.Parse("Z1230ABC");
argentina.ToString("AR"); // Z 1230 ABC
```
