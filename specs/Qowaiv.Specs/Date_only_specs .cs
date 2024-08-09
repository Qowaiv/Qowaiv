#if NET6_0_OR_GREATER

namespace Date_only_specs;

public class Can_be_adjusted_with
{
    [Test]
    public void Date_span_with_months_first()
        => Svo.DateOnly.Add(new DateSpan(2, 20)).Should().Be(new DateOnly(2017, 08, 31));

    [Test]
    public void Date_span_with_days_first()
        => Svo.DateOnly.Add(new DateSpan(2, 20), DateSpanSettings.DaysFirst).Should().Be(new DateOnly(2017, 09, 01));

    [Test]
    public void Month_span()
        => Svo.DateOnly.Add(MonthSpan.FromMonths(3)).Should().Be(new DateOnly(2017, 09, 11));
}

public class Can_not_be_adjusted_with
{
    [TestCase(DateSpanSettings.WithoutMonths)]
    [TestCase(DateSpanSettings.DaysFirst | DateSpanSettings.MixedSigns)]
    public void Date_span_with(DateSpanSettings settings)
        => Svo.DateOnly.Invoking(d => d.Add(new DateSpan(2, 20), settings))
        .Should().Throw<ArgumentOutOfRangeException>().WithMessage("Adding a date span only supports 'Default' and 'DaysFirst'.*");
}

public class Can_be_related_to
{
    [Test]
    public void matching_month()
        => Svo.DateOnly.IsIn(Month.June).Should().BeTrue();

    [Test]
    public void non_matching_month()
       => Svo.DateOnly.IsIn(Month.February).Should().BeFalse();

    [Test]
    public void matching_year()
        => Svo.DateOnly.IsIn(2017.CE()).Should().BeTrue();

    [Test]
    public void non_matching_year()
       => Svo.DateOnly.IsIn(2018.CE()).Should().BeFalse();
}

public class Can_not_be_related_to
{
    [Test]
    public void month_empty()
        => Svo.DateOnly.IsIn(Month.Empty).Should().BeFalse();

    [Test]
    public void month_unknown()
       => Svo.DateOnly.IsIn(Month.Unknown).Should().BeFalse();

    [Test]
    public void year_empty()
        => Svo.DateOnly.IsIn(Year.Empty).Should().BeFalse();

    [Test]
    public void year_unknown()
       => Svo.DateOnly.IsIn(Year.Unknown).Should().BeFalse();
}

public class Method : SingleValueObjectSpecs
{
    private static readonly IReadOnlyCollection<MethodInfo> DateMethods = AllSvos
        .SelectMany(t => t.GetMethods().Where(WithDateParameter))
        .ToArray();

    [TestCaseSource(nameof(DateMethods))]
    public void Exist_with_DateOnly_overload(MethodInfo method)
    {
        var type = method.DeclaringType!;
        var methods = type.GetMethods();
        var overload = methods.SingleOrDefault(overload => IsOverload(method, overload));

        overload.Should().NotBeNull(because: $"Method {method.DeclaringType}.{method} should have an overload for DateOnly");
    }

    private static bool IsOverload(MethodInfo method, MethodInfo overload)
        => method.Name == overload.Name
        && method.ReturnType == overload.ReturnType
        && IsOverload(method.GetParameters().Select(p => p.ParameterType), overload.GetParameters().Select(p => p.ParameterType));

    private static bool IsOverload(IEnumerable<Type> method, IEnumerable<Type> overload)
        => method.Select(t => t == typeof(Date) ? typeof(DateOnly) : t).SequenceEqual(overload);

    private static bool WithDateParameter(MethodInfo method) 
        => method.GetParameters().Exists(p => p.ParameterType == typeof(Date))
        && method.ReturnType != typeof(Date)
        && method.DeclaringType != typeof(Date);
}
#endif
