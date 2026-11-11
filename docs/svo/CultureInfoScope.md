# CultureInfoScope
A CultureInfoScope is a class that allows specifying the current (UI) culture for the
duration/lifetime of the specified scope.

``` C#
using(new CultureInfoScope("es-ES"))
{
    Console.WriteLine(234.12.ToString()); // 234,12
}
// or with an extension
using(new CultureInfo("es-ES").Scoped())
{
    // ...
}
```
