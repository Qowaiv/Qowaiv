#if NET8_0_OR_GREATER

namespace Guard_specs;

public class Stacktrace
{
    [Test]
    public void does_not_contain_guard_method_in_exception()
    {
        try
        {
            _ = Clock.Today(null!);
        }
        catch (ArgumentNullException x)
        {
            var trace = new StackTrace(x);

            var method = trace.GetFrames()[0].GetMethod();

            method.Should().BeEquivalentTo(
                new
                {
                    Name = "NotNull",
                    DeclaringType = new { Name = "Guard" },
                });

            x.StackTrace.Should().NotContain("NotNull");
        }
    }
}
#endif
