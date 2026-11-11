# System.Globalization.CultureInfo
The [`CultureInfo`](https://learn.microsoft.com/dotnet/api/system.globalization.cultureinfo)
lacks a `JsonConverter`:
* `Qowaiv.Json.Globalization.CultureInfoJsonConverter`

The current culture can be set for a (`IDisposable`) scope using the [`.Scoped()`](docs/svo/CultureInfoScope.md)
extension method.
