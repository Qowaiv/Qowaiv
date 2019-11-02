# Qowaiv JSON serialization
For full support of JSON serialization a custom JSON converter has to be
registered. For [System.Text.Json.JsonSerializer](https://docs.microsoft.com/en-us/dotnet/api/system.text.json?view=netcore-3.0)
(.NET Core 3.0) Qowaiv provides an [example implementation](QowaivJsonConverter.cs).

To register you can do:
``` C#
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
