# Qowaiv
## ASP.NET Core MVC Model Binding
Model Binding with ASP.NET Core MVC works out-of-the-box with one exception:
If the binding fails, a generic error message:
> 'String input' is invalid.

This can be quite inconvenient, and the only way to overcome to provide a
custom model binder.

### Example Implementation
Basically, this implementation uses the exception message thrown by the
type converter to update the model state if the binding failed:
[TypeConverterModelBinder.cs](TypeConverterModelBinder.cs)

### Registration
A custom model binder should be registered in the `Startup` class:
``` C#
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc((options) =>
        {
            options.ModelBinderProviders.Insert(0, new TypeConverterModelBinder());
        });
    }
}
```
