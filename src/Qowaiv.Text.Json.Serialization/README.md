# [System.Text.Json](https://docs.microsoft.com/en-us/dotnet/api/system.text.json)
When running TFM .NET 10.0 or later, JSON seralization works _out-of-the-box_.
If you are stuck to an older TFM, you have to register JSON converters
yourself. By registering `QowaivJsonConverter` you register a convention-based
serializer that handles all SVOs following that convention.

``` C#
var options = new JsonSerializerOptions()
{
    Converters = { new QowaivJsonConverter() },
};
```
