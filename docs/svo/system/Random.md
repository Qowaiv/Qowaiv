# System.Random
The [`Random`](https://learn.microsoft.com/dotnet/api/system.random) class
can be used to generate [_pseudo_-random](https://en.wikipedia.org/wiki/Pseudorandomness)
data. The generation of the following SVOs is supported:

## Sequence
`NextSequence()` turns any random selector into an unbounded, lazily evaluated
sequence:

``` C#
// Calls Next() an infinite number of times.
foreach (var value in rnd.NextSequence(r => r.Next()))
{
    // ...
}
```

Values are drawn while enumerating; the sequence never ends, and values may
repeat.

## [International Bank Account Number](../InternationalBankAccountNumber.md)
``` C#
// Generates an IBAN for a random supported country.
var iban = rnd.NextInternationalBankAccountNumber();

// Generates an IBAN for the given country.
var iban = rnd.NextInternationalBankAccountNumber(Country.NL);
```

## [Burgerservicenummer](../../locale/nl/Burgerservicenummer.md)
``` C#
var bsn = rnd.NextBurgerservicenummer();
```

## [RSIN](../../locale/nl/RSIN.md)
``` C#
var rsin = rnd.NextRsin();
```

The generated Burgerservicenummer and RSIN values are nine-digit test numbers
that satisfy the elfproef. They start with `999`, so they can be used as test
data without representing a real person or organisation.
