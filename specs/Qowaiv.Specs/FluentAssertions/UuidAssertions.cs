using FluentAssertions.Numeric;

namespace FluentAssertions;

internal static class UuidAssertions
{
    public static AndConstraint<ComparableTypeAssertions<Uuid>> HavePattern(this ComparableTypeAssertions<Uuid> assertions, params byte?[] pattern)
    {
        var subject = (Uuid)assertions.Subject;
        var act = new List<string>();
        var exp = new List<string>();
        var fail = false;
        var bytes = subject.ToByteArray();

        for (var i = 0; i < bytes.Length; i++)
        {
            var a = bytes[i].ToString("X2");

            if (pattern[i] is { } bits)
            {
                var e = bits.ToString("X2");

                if (e == a)
                {
                    act.Add(a);
                    exp.Add(e);
                }
                else
                {
                    act.Add('[' + a + ']');
                    exp.Add('[' + e + ']');
                    fail = true;
                }
            }
            else
            {
                act.Add(a);
                exp.Add("..");
            }
        }

        if (fail)
        {
            Assert.Fail($@"Expected: [{(string.Join(", ", exp))}]
Actual:   [{(string.Join(", ", act))}]");
        }

        subject.Version.Should().Be(UuidVersion.Sequential);

        return new AndConstraint<ComparableTypeAssertions<Uuid>>(assertions);
    }
}
