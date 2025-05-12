using Qowaiv.CodeGeneration.SingleValueObjects;

namespace CodeGeneration.SingleValueObjects.IdTemplate_specs;

public class Initalizes
{
    [Test]
    public void instance()
    {
        var template = new IdTemplate(new()
        {

            Svo = "Int32Id",
            Namespace = "Specs.Generated",
            Raw = "System.Int32",
            Behavior = "Specs.Generated.Int32Id.Behavior",
        });

        template.ToString().Should().NotBeEmpty();
    }
}
