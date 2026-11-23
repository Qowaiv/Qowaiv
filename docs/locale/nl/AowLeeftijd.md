# AOW-leeftijd

Represents the Dutch state pension age ([AOW-leeftijd](https://nl.wikipedia.org/wiki/AOW-leeftijd))
that applies to a date of birth. The Sociale Verzekeringsbank (SVB) defines the
age as a table of inclusive birth-date ranges; this value object exposes one
value per range.

The AOW age has been definitively established for birth dates before 1 October 1965.
From 1 October 1965 onwards it is an expected age that can change when the SVB updates
its life-expectancy figures. `Definitief` is `Yes` for established values and `No`
for expected ones.

## properties

| Property     | Description                                              |
|--------------|----------------------------------------------------------|
| `Vanaf`      | First birth date (inclusive) for which the value applies |
| `Tot`        | Last birth date (inclusive) for which the value applies  |
| `Leeftijd`   | The AOW age as a [`MonthSpan`](../../svo/MonthSpan.md)   |
| `Definitief` | Whether the AOW age has been definitively established    |

## usage

``` C#
var aow = AowLeeftijd.Geboortedatum(new Date(1980, 02, 14));

aow.Van;        // 1978-10-01
aow.Tot;        // 1982-06-30
aow.Leeftijd;   // 68 years and 6 months
aow.Definitief; // No
```

## defined intervals

| Geboortedata            | AOW-leeftijd          | Def. |
|-------------------------|-----------------------|:----:|
|           <= 1947-12-31 | 65 jaar               | ja   |
| 1948-01-01 - 1948-11-30 | 65 jaar en 1 maand    | ja   |
| 1948-12-01 - 1949-10-31 | 65 jaar en 2 maanden  | ja   |
| 1949-11-01 - 1950-09-30 | 65 jaar en 3 maanden  | ja   |
| 1950-10-01 - 1951-06-30 | 65 jaar en 6 maanden  | ja   |
| 1951-07-01 - 1952-03-31 | 65 jaar en 9 maanden  | ja   |
| 1952-04-01 - 1952-12-31 | 66 jaar               | ja   |
| 1953-01-01 - 1955-08-31 | 66 jaar en 4 maanden  | ja   |
| 1955-09-01 - 1956-05-31 | 66 jaar en 7 maanden  | ja   |
| 1956-06-01 - 1957-02-28 | 66 jaar en 10 maanden | ja   |
| 1957-03-01 - 1960-12-31 | 67 jaar               | ja   |
| 1961-01-01 - 1965-09-30 | 67 jaar en 3 maanden  | ja   |
| 1965-10-01 - 1966-09-30 | 67 jaar en 3 maanden  | nee  |
| 1966-10-01 - 1970-09-30 | 67 jaar en 6 maanden  | nee  |
| 1970-10-01 - 1973-03-31 | 67 jaar en 9 maanden  | nee  |
| 1973-04-01 - 1975-12-31 | 68 jaar               | nee  |
| 1976-01-01 - 1978-09-30 | 68 jaar en 3 maanden  | nee  |
| 1978-10-01 - 1982-06-30 | 68 jaar en 6 maanden  | nee  |
| 1982-07-01 - 1985-03-31 | 68 jaar en 9 maanden  | nee  |
| 1985-04-01 - 1988-12-31 | 69 jaar               | nee  |
| 1989-01-01 - 1991-09-30 | 69 jaar en 3 maanden  | nee  |
| 1991-10-01 - 1995-06-30 | 69 jaar en 6 maanden  | nee  |
| 1995-07-01 - 1999-03-31 | 69 jaar en 9 maanden  | nee  |
| 1999-04-01 - 2000-12-31 | 70 jaar               | nee  |
| 2001-01-01 <=           | 70 jaar               | nee  |
*bron: [svb.nl](https://www.svb.nl/nl/aow/aow-leeftijd/uw-aow-leeftijd)*
