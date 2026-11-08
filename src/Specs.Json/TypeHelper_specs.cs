using Qowaiv;
using Qowaiv.Internals;

namespace TypeHelper_specs;

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
