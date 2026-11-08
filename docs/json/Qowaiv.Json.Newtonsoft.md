# Qowaiv.Json.Newtonsoft
[Newtonsoft.Json](https://www.newtonsoft.com/json)'s converter was the .NET
defacto default, until .NET Core 3.0. Qowaiv has not _out-of-the-box_ support
for Newtonsoft, but with the `Qowaiv.Json.Newtonsoft` package, seamless
integration can be added.

## Instalation
``` bash
dotnet add package Qowaiv.Json.Newtonsoft
```

## Register the converter
To globally register the converter:
``` C#
QowaivJsonConverter.Register();
```

Or, if you work with .NET core Web API:

``` bash
dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson
```

``` C#
using Qowaiv.Json.Newtonsoft;

public class Startup
{
    public void ConfigureServices(IServiceCollection services) => services
        .AddMvc()
        .AddNewtonsoftJson(options => 
        {
            options.SerializerSettings.Converters.Add(new QowaivJsonConverter());
        });
}
```
