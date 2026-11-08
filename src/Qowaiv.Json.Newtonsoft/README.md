# [Newtonsoft](https://www.newtonsoft.com/json)
Newtonsoft's converter was the .NET de facto default, until .NET Core 3.0. To use
the `Qowaiv.Json.Newtonsoft` package the following code can be used:

``` C#
QowaivJsonConverter.Register();
```

Or, if you work with .NET core Web API:

NuGet:
```
Install-Package Microsoft.AspNetCore.Mvc.NewtonsoftJson
```

Startup.cs:
``` C#
using Qowaiv.Json.Newtonsoft;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddMvc()
            .AddNewtonsoftJson(options => 
            {
                options.SerializerSettings.Converters.Add(new QowaivJsonConverter());
            });
    }
}
```
