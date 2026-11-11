# Sex
Represents a sex based on an ISO 5218 code.

``` C#
var sex = Sex.Female;
var parsed = Sex.Parse("Male");

sex.ToString("s"); // ♀
sex.ToString("c"); // f
sex.ToString(string.Empty, new CultureInfo("pt")); // Feminino
sex.ToString("h", new CultureInfo("en-GB")); // Mrs.
```
