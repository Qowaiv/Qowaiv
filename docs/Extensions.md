# Extensions
The Qowaiv core library comes with multiple extensions

## Qowaiv.Exensions
Todo

## Qowaiv.Humanizer
Inspired by [humanizr.net](https://humanizr.net) Qowaiv provides humizer
extenion methods that help to create SVO instances.

``` csharp
305_500.50m.Amount()    // Amount
4.DividedBy(7)          // Fraction
5.Years() + 3.Months()  // MonthSpan
12.14m.Percent()        // Percentage
2017.CE()               // Year
5.Years()               // YearSpan
```

## Qowaiv.Mathmatics
All methods on `Qowaiv.Mathmatics.DecimalMath` can be used as extension
methods. It is up te the developer to choice what is prefered.

``` csharp
decimal x;
x.Round(3, DecimalRounding.ToOdd);
DecimalMath.Round(x, 3, DecimalRounding.ToOdd);
```
