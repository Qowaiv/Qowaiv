using FluentValidation;
using MathNet.Numerics.Random;
using Qowaiv.Validation.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Qowaiv.Validation.TestData;

public sealed class BenchmarkModel : IValidatableObject
{
    public static BenchmarkModel New(int count)
    {
        var rnd = new MersenneTwister(count);

        var list = new List<Child>();

        for (var i = 0; i < count; i++)
        {
            var child = new Child
            {
                JustIgnore1 = rnd.Next(0, 100),
                JustIgnore2 = rnd.Next(0, 100),
                JustIgnore3 = rnd.Next(0, 100),
                UseLessAnntation = rnd.NextDouble() > 0.6,
                SomeChild = rnd.Next() > 0.3
                ? new()
                {
                    JustIgnore1 = rnd.Next(0, 100),
                    JustIgnore2 = rnd.Next(0, 100),
                    JustIgnore3 = rnd.Next(0, 100),
                    UseLessAnntation = rnd.NextDouble() > 0.6,
                }
                : null,
            };

            list.Add(child);
        }
        return new()
        {
            JustIgnore1 = rnd.Next(),
            JustIgnore2 = rnd.Next(),
            JustIgnore3 = rnd.Next(),
            Children = list,
        };
    }

    [Length.AtLeast(6)]
    [Required]
    public string RequiredName { get; init; } = "123456";

    [Optional]
    public string? OptionalName { get; init; }

    public int JustIgnore1 { get; init; }

    public int JustIgnore2 { get; init; }

    public int JustIgnore3 { get; init; }

    [Any]
    public List<Child> Children { get; init; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (RequiredName.Any(char.IsLetter)) yield return ValidationMessage.Error("Only digits are allowed", nameof(RequiredName));
    }

    public class Child : IValidatableObject
    {
        [Mandatory]
        public EmailAddress Email { get; init; } = EmailAddress.Parse("info@qowaiv.org");

#pragma warning disable QW0101 // Required attribute cannot invalidate value types
        // This is part of the test
        [Required]
#pragma warning restore QW0101 // Required attribute cannot invalidate value types
        public bool UseLessAnntation { get; init; }

        public int JustIgnore1 { get; init; }

        public int JustIgnore2 { get; init; }

        public int JustIgnore3 { get; init; }

        public Child? SomeChild { get; init; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (JustIgnore1 + JustIgnore2 + JustIgnore3 > short.MaxValue) yield return ValidationMessage.Error("Too much");
        }
    }
}

public sealed class BenchmarkModelValidator : AbstractValidator<BenchmarkModel>
{
    public BenchmarkModelValidator()
    {
        RuleFor(m => m.RequiredName).Required().MinimumLength(6).Must(NoLetters);
        RuleForEach(m => m.Children).SetValidator(ChildValdator.Instance);
    }

    private static bool NoLetters(string name) => !name.Any(char.IsLetter);
}

public sealed class ChildValdator : Fluent.ModelValidator<BenchmarkModel.Child>
{
    public static readonly ChildValdator Instance = new();
    public ChildValdator()
    {
        RuleFor(m => m.Email).Required();
        RuleFor(m => m).Must(AddUp);
        RuleFor(m => m.SomeChild).SetValidator(this!);
    }

    private static bool AddUp(BenchmarkModel.Child child) => child.JustIgnore1 + child.JustIgnore2 + child.JustIgnore3 <= short.MaxValue;
}
