# Qowaiv JSON
Serializing data using JSON has become the defacto default. All Qowaiv SVOs
support JSON serialization; they are all decorated with [`JsonConverter(typeof(SvoJsonConverter))]`](https://docs.microsoft.com/dotnet/api/system.text.json.serialization.jsonconverterattribute).

## Skip serializion of empty SVOs
Since .NET 8.0, it is possible to register modifiers for the [`System.Text.Json.TypeInfoResolver`](https://docs.microsoft.com/dotnet/api/system.text.json.jsonserializeroptions.typeinforesolver).
This allows you to change serialization behavior, for example by editing
`ShouldSerialize(object model, object? prop)`. This can be useful if you do not
want to serialize empty SVOs (such as `EmailAddress.Empty`). To get this
(non-default) behavior you have to provide `JsonSerializerOptions` when serializing:

``` C#
var options = new JsonSerializerOptions()
{
    TypeInfoResolver = new DefaultJsonTypeInfoResolver()
    {
        Modifiers = { Qowaiv.Json.ModifyTypeInfo.IgnoreEmptySvos },
    },
};
```

## Other JSON frameworks
If you you need JSON/BSON serialization but can not use `System.Text.Json` (as
part of the .NET framework) Qowaiv provides an alternative.

### Convention based contract
When an SVO follows the following contract (all Qowaiv SVOs do)
framework specific converters can be build:

1. Has a public static factory method `FromJson(string)` returning a new
   instance of the SVO.
2. Optional factory methods for `double`, `long`, and `bool` can be provided.
   If not, the `string` factory method is used.
3. If a none `void` method `ToJson()` is provided, this one is used for
   serialization, otherwise `svo.ToString()`.

``` C#
public struct/class/record SVO
{
    // Required.
    public static SVO FromJson(string json);

    // Optional, otherwise FromJson(json.ToString(CultureInfo.Invariant)) is called.
    public static SVO FromJson(double json);

    // Optional, otherwise FromJson(json.ToString(CultureInfo.Invariant)) is called.
    public static SVO FromJson(long json);

    // Optional, otherwise FromJson(json ? "true": "false") is called.
    public static SVO FromJson(bool json);

    // Optional, otherwise ToString()
    public object /* or string, bool, int, long, double, decimal */ToJson();
}
```


#### Implementations
There are two _out-of-the-box_ implementations that support this convention-
based contract.
* [Qowaiv.Bson.MongoDB ](Qowaiv.Bson.MongoDB.md)  
* [Qowaiv.Json.Newtonsoft](Qowaiv.Json.Newtonsoft.md)  
* [Qowaiv.Text.Json.Serialization](Qowaiv.Text.Json.Serialization.md)[*]

[*] _Only needed when running pre-.NET 10.0_
