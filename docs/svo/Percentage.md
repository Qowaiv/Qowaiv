# Percentage
Represents a percentage. It supports parsing from per mile and per ten thousand
too. The basic thought is that `Percentage.Parse("14%")` has the same result
as `double.Parse("14%")`, which is `0.14`.

``` C#
// Creation
var p = Percentage.Parse("3.14");  // Parse: 3.14%;
var p = Percentage.Parse("3.14%"); // Parse: 3.14%;
var p = Percentage.Parse("31.4‰"); // Parse: 3.14%;
var p = 3.14.Percent(); // Extension on double: 3.14%;
Percentage p = 0.0314m; // Implicit casting from decimal: 3.14%.

// Explicit casting
var p = (Percentage)0.500; // 50%

// Manipulation
var p = 13.2.Percent();
p++; // 14.2%;
var total = 400;
total *= 50.Percent(); // Total = 200;
var value = 50.0;
value += 10.Percent(); // value 55;

var rounded = 17.56.Percent().Round(1); // 17.6%;

var complement = 25.Percent().Complement(); // 75%

var max = Percentage.Max(1.4.Percent(), 1.8.Percent()); // 1.8%;
var min = Percentage.Min(1.7.Percent(), 1.9.Percent()); // 1.7%;

var perMille = 15.5.Percent().ToString("PM"); // 155‰
var perTenThousand = 0.34.Percent().ToString("PT"); // 34‱
```
