# Fraction
A fraction (from Latin _fractus_, "broken") represents a part of a whole or, more
generally, any number of equal parts. It can be written as a/b (a divided by b).

It is worth noting that the `default` value of a `Fraction` is `Fraction.Zero`
and that `NaN` and `Infinity` are not supported.

## Creation
Fractions can be created in multiple ways:
``` C#
Fraction fluent = 4.DividedBy(13); // Preferred way

Fraction ctor = new(4, 13);

Fraction parsed = Fraction.Parse("4/13");
Fraction parsed = Fraction.Parse("4½"); // single fraction character
Fraction parsed = Fraction.Parse("4²³⁄₄₇"); // Unicode super- and subscript

Fraction fromFloating = Fraction.Create(0.3456786754m);

Fraction cast = (Fraction)34;
Fraction cast = (Fraction)0.3333;
var (numerator, denominator) = fluent; // deconstructs.
```

## Operations
``` C#
var add      = 4.DividedBy(3) + 7.DividedBy(4); // 3 1/12
var subtract = 4.DividedBy(3) - 2.DividedBy(3); // 2/3
var multiply = 3.DividedBy(5) * 2.DividedBy(3); // 6/15
var divide   = 3.DividedBy(5) / 2.DividedBy(3); // 9/10
```

## Formatting
There are multiple types of formatting supported. Without
a fraction bar, the fraction is formatted as a decimal.
``` C#
var dec = 17.DividedBy(5).ToString("0.##"); // 3,40
```

If the whole part should be formatted as such, by adding the preferred formatting
between brackets.
``` C#
var withWhole = 17.DividedBy(5).ToString("[0]0/0"); // 3 2/5
``` 

To specify fraction bar of choice, just define that one in the format:
### Fraction bars
name           |   c    | code 
---------------|:------:|------
slash          |   /    | 002F
colon          |   :    | 003A
division sign  |   ÷    | 00F7
fraction slash |   ⁄    | 2044
division slash |   ∕    | 2215
short slash    | &0337; | 0337
long slash     | &0338; | 0338

``` C#
var customBar = 3.DividedBy(4).ToString("0/÷0"); // 3÷4
```

Unicode supports super- and subscript, and so does `Fraction`:
```C#
var super = -17.DividedBy(5).ToString("[0]super/sub"); // -3²⁄₅
var super = 5.DividedBy(3).ToString("super/sub"); // ⁵⁄₃
```

The default format (so if you do not specify anything) is:
```C#
var basic = 3.DividedBy(4).ToString("0/0"); // 3/4
```
