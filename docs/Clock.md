# Clock
The `Clock` class is an outsider within the Qowaiv library. It is a solution
for a problem that is not related to Domain-Driven Design, but to the fact that
the behavior of `System.DateTime.UtcNow` (and its equivalents) cannot be controlled.
This can be problematic for writing proper tests that rely on its behavior.

The default way to tackle this problem is by providing a lightweight service
like this one:
``` CSharp
public interface IClock
{
    DateTime UtcNow();
}

public class Clock : IClock
{
    public DateTime UtcNow() => DateTime.UtcNow;
}
```

However, providing an IClock all the time when there is time related logic is
not that elegant at all. The Qowaiv `Clock` helps to overcome this. In code
you just call `Clock.UtcNow()` or one of its derived methods. In a test you
change the behaviour, in most cases just for the scope of your current
(execution) context:

``` CSharp
[Test]
public void TestSomething()
{
    using (Clock.SetTimeForCurrentContext(() => new DateTime(2017, 06, 11)))
    {
        // test code.
    }
}
```

## TimeProvider
Since .NET 8.0, Microsoft provides a `TimeProvider`. To benefit from both the
`Qowaiv.Clock` mechanism and this time provider, `Clock.TimeProvider`—a
singleton which provides access to `Clock.UtcNow()` and `Clock.TimeZone`—has been
added.
