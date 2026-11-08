# Qowaiv JSON
Serializing data using JSON is common practice. However, .NET has no generic
interface in the standard library to implement.

The solution provided here, is a (naming) convention based one, to serialize
Single Value Objects: Value Objects that can be represented by a single scalar.

1. There should be a public static factory method `FromJson(string)` returning 
   a new instance of the Single Value Object (SVO).
2. Optional factory methods for `double`, `long`, and `bool` can be provided.
   If not, the `string` factory method is used.
3. If a none `void` method `ToJson()` is provided, this one is used for
   serialization, otherwise `object.ToString()`.

## Sample Single Value Object

``` C#
public struct /* or class */ SingleValueObject
{
    // Required.
    public static SingleValueObject FromJson(string json);

    // Optional, otherwise FromJson(json.ToString(CultureInfo.Invariant)) is called.
    public static SingleValueObject FromJson(double json);

    // Optional, otherwise FromJson(json.ToString(CultureInfo.Invariant)) is called.
    public static SingleValueObject FromJson(long json);

    // Optional, otherwise FromJson(json ? "true": "false") is called.
    public static SingleValueObject FromJson(bool json);

    // Optional, otherwise ToString()
    public object /* or string, bool, int, long, double, decimal */ToJson();
}
```

## [System.Text.Json](https://docs.microsoft.com/en-us/dotnet/api/system.text.json)
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

## [Newtonsoft](https://www.newtonsoft.com/json)
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

## [MongoDB.Bson](https://mongodb.github.io/mongo-csharp-driver/)
MongoDB is a cross-platform document-oriented database program. Classified as a
NoSQL database program, MongoDB uses BSON documents with a schema. The .NET
library provides a mechanism to convert objects from and to BSON [(..)](src/Qowaiv.Bson.MongoDB.README.md)
