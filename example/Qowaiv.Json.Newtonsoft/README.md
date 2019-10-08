# Qowaiv JSON serialization
For full support of JSON serialization a custom JSON converter has to be
registered. For [Newtonsoft](https://www.newtonsoft.com)'s converter (the .NET
de facto default) Qowaiv provides an [example implementation](QowaivJsonConverter.cs).

To register you can do:
``` C#
QowaivJsonConverter.Register();
```

Or, if you work with .NET core Web API:

``` C#
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddMvc()
            .AddJsonOptions(options => 
            {
                options.SerializerSettings.Converters.Add(new QowaivJsonConverter());
            });
    }
}
```
