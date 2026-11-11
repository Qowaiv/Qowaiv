# (EU) Energy label
The energy efficiency of the appliance is rated in terms of a set of energy
efficiency classes from A++++ to G on the label, A being the most energy
efficient, G the least efficient.

``` C#
var a_plusplus = EnergyLabel.A(2);
var b = EnergyLabel.B;
var parsed = EnergyLabel.Parse("A++")
```
