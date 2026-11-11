# Business Identifier Code (BIC)
Represents a BIC as specified in ISO 9362.

``` C#
var bic = BusinessIdentifierCode.Parse("AEGONL2UXXX");

var business = bic.Business; // "AEGO"
var country = bic.Country;   // Country.NL
var location = bic.Location; // "2U"
var branch = bic.Branch;     // "XXX"
var length = bic.Length;     // 11
```
