# Qowaiv
## ASP.NET MVC Model Binding
Model Binding with ASP.NET MVC works out-of-the-box with one exception:
It does not handle `null` and `string.Empty` values incorrect. In those
cases the model state is not updated, instead of setting the default value,
or failing when those value do not represent a valid state.

This can be quite inconvenient, and the only way to overcome to provide a
custom model binder.

### Example Implementation
Basically, this implementation does not skip `null` and `string.Empty` values:
[TypeConverterModelBinder.cs](TypeConverterModelBinder.cs)

### Registration
A custom model binder should be registered in the `Global` class:
``` C#
public class Global
{
    protected override void Application_Start()
    {
        base.Application_Start();
        TypeConverterModelBinder.RegisterForAll(ModelBinders.Binders);
    }
}
```
