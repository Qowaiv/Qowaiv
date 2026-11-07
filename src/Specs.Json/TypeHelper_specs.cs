using Qowaiv;
using Qowaiv.Internals;

namespace TypeHelper_specs;

public class Is_no_ID_Behavior
{
    [Test]
    public void Misses_interface() => TypeHelper.IdBehavior(typeof(object)).Should().BeNull();
}

public class Is_no_SVO_Behavior
{
    [Test]
    public void Misses_base_type() => TypeHelper.SvoBehavior(typeof(object)).Should().BeNull();
}

public class CandidateTypes
{
    [Test]
    public void GetCandidateTypes_skips_unsupported_converts_ids()
    {
        var types = new[]
        { 
            typeof(object), 
            typeof(Uuid),
            typeof(EmailAddress),
            typeof(List<int>),
            typeof(List<>) 
        };

        TypeHelper.GetCandidateTypes(types)
            .Should().BeEquivalentTo(
            [
                typeof(object),
                typeof(Uuid),
                typeof(EmailAddress),
                typeof(List<int>),
            ]);
    }
}
