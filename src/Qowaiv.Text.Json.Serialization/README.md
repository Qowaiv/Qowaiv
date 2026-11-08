# [System.Text.Json](https://docs.microsoft.com/en-us/dotnet/api/system.text.json)
Since .NET Core 3.0, Microsoft provides a built-in JSON serialization. To use
the `Qowaiv.Text.Json.Serialization` package the following code can be used:

``` C#
using Qowaiv.Text.Json.Serialization;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddMvc() 
            .AddJsonOptions(options => 
            {
                options.JsonSerializerOptions.Converters.Add(new QowaivJsonConverter());
            });
    }
}
```
