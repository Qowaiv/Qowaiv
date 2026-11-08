# Qowaiv.Text.Json.Serialization
When running TFM .NET 10.0 or later, JSON seralization works _out-of-the-box_
with [System.Text.Json](https://docs.microsoft.com/dotnet/api/system.text.json).
If you are stuck to an older TFM, you have to register JSON converters
yourself. By registering `QowaivJsonConverter` you register a convetion-based
serializer that handles all SVOs following that convetion.

## Instalation
``` bash
dotnet add package Qowaiv.Text.Json.Serialization
```

## Register the converter
``` C#
var options = new JsonSerializerOptions()
{
    Converters = { new QowaivJsonConverter() },
};
```

Or, if you work with .NET core Web API:

``` bash
dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson
```

``` C#
using Qowaiv.Text.Json.Serialization;

public class Startup
{
    public void ConfigureServices(IServiceCollection services) => services
        .AddMvc() 
        .AddJsonOptions(options => 
        {
            options.JsonSerializerOptions.Converters.Add(new QowaivJsonConverter());
        });
}
```
