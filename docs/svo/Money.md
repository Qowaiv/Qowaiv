# Money
Represents the amount and the currency. Technically this is not SVO. However it
acts identically as a SVO.

``` C#
Money money = 125.34 + Currency.EUR;
var sum = (12 + Currency.EUR) + (15 + Currency.USD); // Throws CurrencyMismatchException()
var rounded = money.Round(0); // EUR 125.00
var (amount, currency) = money; // deconstructs.
```
