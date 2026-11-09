using Qowaiv.Validation.DataAnnotations;

namespace Data_annotations.Attributes.Defined_enums_only_specs;

public class Does_not_allow
{
    public class Generic
    {
        [Test]
        public void validation_on_non_enum_values()
        {
            Func<bool> validate = () => new DefinedOnlyAttribute<Number>().IsValid("value");
            validate.Should().Throw<UnsupportedType>();
        }
    }

    public class Non_generic
    {
        [Test]
        public void validation_on_non_enum_values()
        {
            Func<bool> validate = () => new DefinedEnumValuesOnlyAttribute().IsValid(1);
            validate.Should().Throw<UnsupportedType>();
        }
    }
}
public class Is_valid_for
{
    public class Generic
    {
        [Test]
        public void Null()
            => new DefinedOnlyAttribute<Number>().IsValid(null).Should().BeTrue();

        [Test]
        public void defined_enum_value()
            => new DefinedOnlyAttribute<Number>().IsValid(Number.One).Should().BeTrue();

        [Test]
        public void defined_enum_flag_value()
            => new DefinedOnlyAttribute<Banners>().IsValid(Banners.UnionJack).Should().BeTrue();

        [Test]
        public void defined_mixed_enum_flag_values()
            => new DefinedOnlyAttribute<Banners>().IsValid(Banners.American).Should().BeTrue();

        [Test]
        public void not_defined_mix_of_defined_enum_flag_values()
            => new DefinedOnlyAttribute<Banners>().IsValid(Banners.UnionJack | Banners.StarsAndStripes).Should().BeTrue();
    }

    public class Non_generic
    {
        [Test]
        public void Null()
            => new DefinedEnumValuesOnlyAttribute().IsValid(null).Should().BeTrue();

        [Test]
        public void defined_enum_value()
            => new DefinedEnumValuesOnlyAttribute().IsValid(Number.One).Should().BeTrue();

        [Test]
        public void defined_enum_flag_value()
            => new DefinedEnumValuesOnlyAttribute().IsValid(Banners.UnionJack).Should().BeTrue();

        [Test]
        public void defined_mixed_enum_flag_values()
            => new DefinedEnumValuesOnlyAttribute().IsValid(Banners.American).Should().BeTrue();

        [Test]
        public void not_defined_mix_of_defined_enum_flag_values()
            => new DefinedEnumValuesOnlyAttribute().IsValid(Banners.UnionJack | Banners.StarsAndStripes).Should().BeTrue();
    }
}

public class Is_not_valid_for
{
    public class Generic
    {
        [Test]
        public void not_defined_enum_value()
            => new DefinedOnlyAttribute<Number>().IsValid((Number)42).Should().BeFalse();

        [Test]
        public void not_defined_enum_flag_value()
            => ((Banners)42).Invoking(v => new DefinedOnlyAttribute<Number>().IsValid(v))
            .Should().Throw<UnsupportedType>();

        [Test]
        public void not_defined_mix_of_defined_enum_flag_values_when_defined_flag_combinations_are_required()
            => new DefinedOnlyAttribute<Banners>
            {
                OnlyAllowDefinedFlagsCombinations = true,
            }
            .IsValid(Banners.UnionJack | Banners.StarsAndStripes).Should().BeFalse();
    }

    public class Non_generic
    {
        [Test]
        public void not_defined_enum_value()
            => new DefinedEnumValuesOnlyAttribute().IsValid((Number)42).Should().BeFalse();

        [Test]
        public void not_defined_enum_flag_value()
            => new DefinedEnumValuesOnlyAttribute().IsValid((Banners)42).Should().BeFalse();

        [Test]
        public void not_defined_mix_of_defined_enum_flag_values_when_defined_flag_combinations_are_required()
            => new DefinedEnumValuesOnlyAttribute
            {
                OnlyAllowDefinedFlagsCombinations = true,
            }
            .IsValid(Banners.UnionJack | Banners.StarsAndStripes).Should().BeFalse();
    }
}

public class OnlyAllowDefinedFlagsCombinations
{
    [Test]
    public void is_disabled_by_default()
        => new DefinedOnlyAttribute<Banners>().OnlyAllowDefinedFlagsCombinations.Should().BeFalse();

    [Test]
    public void generic_is_applied_per_instance()
    {
        var combined = Banners.UnionJack | Banners.StarsAndStripes;
        var strict = new DefinedOnlyAttribute<Banners> { OnlyAllowDefinedFlagsCombinations = true };
        var loose = new DefinedOnlyAttribute<Banners>();

        strict.IsValid(combined).Should().BeFalse();
        loose.IsValid(combined).Should().BeTrue();
        strict.IsValid(combined).Should().BeFalse();
    }

    [Test]
    public void non_generic_is_applied_per_instance()
    {
        var combined = Banners.UnionJack | Banners.StarsAndStripes;
        var strict = new DefinedEnumValuesOnlyAttribute { OnlyAllowDefinedFlagsCombinations = true };
        var loose = new DefinedEnumValuesOnlyAttribute();

        loose.IsValid(combined).Should().BeTrue();
        strict.IsValid(combined).Should().BeFalse();
        loose.IsValid(combined).Should().BeTrue();
    }
}
public class Edge_cases
{
    [Test]
    public void generic_flags_enum_without_values_only_allows_default()
    {
        var attribute = new DefinedOnlyAttribute<NoFlags>();

        attribute.IsValid(default(NoFlags)).Should().BeTrue();
        attribute.IsValid((NoFlags)1).Should().BeFalse();
    }

    [Test]
    public void non_generic_flags_enum_without_values_only_allows_default()
    {
        var attribute = new DefinedEnumValuesOnlyAttribute();

        attribute.IsValid(default(NoFlags)).Should().BeTrue();
        attribute.IsValid((NoFlags)1).Should().BeFalse();
    }

    [Test]
    public void flags_enum_with_defined_zero_is_valid_for_both_modes_and_attributes()
    {
        foreach (var strict in new[] { false, true })
        {
            new DefinedOnlyAttribute<WithNone> { OnlyAllowDefinedFlagsCombinations = strict }
                .IsValid(WithNone.None).Should().BeTrue();
            new DefinedEnumValuesOnlyAttribute { OnlyAllowDefinedFlagsCombinations = strict }
                .IsValid(WithNone.None).Should().BeTrue();
        }
    }

    [Test]
    public void flags_enum_with_undefined_zero_is_only_valid_when_combinations_are_allowed()
    {
        new DefinedOnlyAttribute<Banners>().IsValid((Banners)0).Should().BeTrue();
        new DefinedEnumValuesOnlyAttribute().IsValid((Banners)0).Should().BeTrue();

        new DefinedOnlyAttribute<Banners> { OnlyAllowDefinedFlagsCombinations = true }
            .IsValid((Banners)0).Should().BeFalse();
        new DefinedEnumValuesOnlyAttribute { OnlyAllowDefinedFlagsCombinations = true }
            .IsValid((Banners)0).Should().BeFalse();
    }

    [Test]
    public void concurrent_first_use_is_consistent()
    {
        var results = new System.Collections.Concurrent.ConcurrentBag<bool>();

        Parallel.For(0, 256, i =>
        {
            var strict = i % 2 == 0;
            var generic = new DefinedOnlyAttribute<Concurrent> { OnlyAllowDefinedFlagsCombinations = strict };
            var nonGeneric = new DefinedEnumValuesOnlyAttribute { OnlyAllowDefinedFlagsCombinations = strict };
            var combined = Concurrent.A | Concurrent.B;

            results.Add(generic.IsValid(combined) == !strict);
            results.Add(nonGeneric.IsValid(combined) == !strict);
        });

        results.Should().OnlyContain(r => r);
    }
}

[Flags]
public enum NoFlags { }

[Flags]
public enum WithNone
{
    None = 0,
    One = 1,
    Two = 2,
}

[Flags]
public enum Concurrent
{
    A = 1,
    B = 2,
    C = 4,
}
public class With_message
{
    [Arguments("nl", "De waarde van het veld Banners is niet toegestaan.")]
    [Arguments("en", "The value of the Banners field is not allowed.")]
    public void culture_dependent(CultureInfo culture, string message)
    {
        using var _ = culture.Scoped();
        new Model().ValidateWith(new AnnotatedModelValidator<Model>())
            .Should().BeInvalid()
            .WithMessage(ValidationMessage.Error(message, "Banners"));
    }
    internal class Model
    {
        [DefinedOnly<Banners>]
        public Banners Banners { get; set; } = (Banners)42;
    }
}

[Flags]
public enum Banners
{
    UnionJack = 1,
    StarsAndStripes = 2,
    MapleLeaf = 4,
    American = StarsAndStripes | MapleLeaf,
}

public enum Number
{
    Zero,
    One,
}

