# Yes-no
A Yes-no is a (bi-)polar that obviously has the values "yes" and "no". It also
has an "empty"(unset) and "unknown" value. It maps easily with a `boolean`, but
supports all kinds of formatting (and both empty and unknown) that can not be
achieved when modeling a property as `bool` instead of a `YesNo`.

``` C#
// Creation
var yn = YesNo.Parse("ja", new CultureInfo("nl-NL"));
var answer = YesNo.Yes;

if (!answer.Known)
{
   // Do logic.
}

if (answer) // Equal to answer.IsYes()
{
    // Do logic.
}
```
