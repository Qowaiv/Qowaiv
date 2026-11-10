using Qowaiv.Validation.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace TestTools.StackTrace_specs;

public class Hides
{
    [Test]
    public void assertion_logic()
    {
        var validator = new AnnotatedModelValidator<Model>();

        try
        {
            validator
                .Validate(new() { Prop = null })
                .Should().BeValid();
        }
        catch (AssertionFailed x)
        {
            var trace = new StackTrace(x);
            var method = trace.GetFrame(0)!.GetMethod();

            method.Should().BeEquivalentTo(new
            {
                Name = "Ensure",
                DeclaringType = new { Name = "Assertion" },
            });

            x.StackTrace.Should().NotContain("Ensure");
        }
    }
}

file class Model
{
    [Required]
    public string? Prop { get; init; }
}
