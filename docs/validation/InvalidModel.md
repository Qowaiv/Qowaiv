# InvalidModel

Thrown when accessing `Value` on an invalid [result](Result.md). Contains the
`Errors` collection:

``` C#
try
{
    var value = invalidResult.Value;
}
catch (InvalidModel ex)
{
    ValidationMessages errors = ex.Errors;
}
```
