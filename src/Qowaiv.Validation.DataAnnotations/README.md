# Qowaiv DataAnnotation validation


## Validation messages
The difference with Microsoft's default ValidationResult and ValidationMessages is that in this PR we support a severity: info, warning, or error.

Those messages can be created via factory methods:
``` C#
var none = ValidationMessage.None;
var info = ValidationMessage.Info(message, args);
var warn = ValidationMessage.Warning(message, args);
var error = ValidationMessage.Error(message, args);
```

## Validation attributes

### Mandatory attribute
The `RequiredAttribute` does not work for value types. The `MandatoryAttribute`
does. The default value of the `struct` is not valid. It also is not valid for
the Unknown value, unless that is explicitly allowed.

``` C#
public class Model
{
    [Mandatory(AllowUnknownValue = true)]
    public EmailAddress Email { get; set; }

    [Mandatory()]
    public string SomeString { get; set; }
}
```

### Any attribute
The `RequiredAttribute` does not work for collections. The `AnyAttribute`
does. It is only valid as he collection contains at least one item.

``` C#
public class Model
{
    [Any()]
    public List<int> Numbers { get; set; }
}
```

### AllowedValues attribute
The `AllowedValuesAttribute` allows to define a subset of allowed values. It
supports type converters to get the allowed values based on a string value.

``` C#
public class Model
{
    [AllowedValues("DE", "FR", "GB")]
    public Country CountryOfBirth { get; set; }
}
```

### ForbiddenValues attribute
The `ForbiddenValuesAttribute` allows to define a subset of forbidden values. It
supports type converters to get the allowed values based on a string value.

``` C#
public class Model
{
    [ForbiddenValues("US", "IR")]
    public Country CountryOfBirth { get; set; }
}
```

### DefinedEnumValuesOnly attribute
The `DefinedEnumValuesOnlyAttribute` limits the allowed values to defined
enums only. By default it supports all possible combinations of defined enums 
when dealing with flags, but that can be restricted by setting 
`OnlyAllowDefinedFlagsCombinations` to true.

``` C#
public class Model
{
    [DefinedEnumValuesOnly(OnlyAllowDefinedFlagsCombinations = false)]
    public SomeEnum CountryOfBirth { get; set; }
}
```

### DistinctValues attribute
The `DistinctValuesAttribute` validates that all items of the collection are
distinct. If needed, a custom `IEqualityComparer` comparer can be defined.

``` C#
public class Model
{
    [DistinctValues(typeof(CustomEqualityComparer))]
    public IEnumerable<int> Numbers { get; set; }
}
```

### NestedModel attribute
The `AnnotatedModelValidator` of this packages support nested validation.
Microsoft's default implementation doesn't. The validator will only do that if
a class has been decorated as such.

``` C#
public class NestedModelWithChildren
{
    public ChildModel[] Children { get; set; }

    [NestedModel]
    public class ChildModel
    {
        [Mandatory]
        public string Name { get; set; }
    }
}
```
