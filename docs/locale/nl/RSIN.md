# RSIN
Represents a Dutch [RSIN](https://nl.wikipedia.org/wiki/Rechtspersonen_Samenwerkingsverbanden_Informatie_Nummer)
(Rechtspersonen Samenwerkingsverbanden Informatie Nummer), the counterpart of
the Burgerservicenummer for legal entities and partnerships. Dutch government
organisations such as the Netherlands Tax Administration, the Land Registry
Office (Kadaster), and municipalities use it to exchange information about
businesses.

An RSIN consists of nine digits and, satisfies the elfproef (eleven test).

## usage
``` C#
var rsin = Rsin.Parse("111222333");
var created = Rsin.Create(1112_22_333L);

rsin.ToString();    // 111222333
rsin.ToString("F"); // 1112.22.333
```
