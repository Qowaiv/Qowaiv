namespace Formatting_arguments_collection_specs;

public class Can_be_constructed
{
    [Test]
    public void based_on_parent_collection()
    {
        var parent = new FormattingArgumentsCollection(TestCultures.nl)
        {
            { typeof(Date), "yyyy-MM" },
        };

        var collection = new FormattingArgumentsCollection(TestCultures.nl_BE, parent);
        collection.Should().ContainSingle();
    }
}

public class Formats
{
    [Test]
    public void indexes_up_to_1_000_000()
    {
        var collection = new FormattingArgumentsCollection(new CultureInfo("nl-BE"));
        var args = new object[1_000_001];
        args[1_000_000] = "Test";
        collection.Format("Begin {1000000} End", args).Should().Be("Begin Test End");
    }

    [Test]
    public void with_allign_left()
    {
        var collection = new FormattingArgumentsCollection();
        collection.Format("{0,-4}", "a").Should().Be("a   ");
    }

    [Test]
    public void with_allign_right()
    {
        var collection = new FormattingArgumentsCollection();
        collection.Format("{0,3}", "a").Should().Be("  a");
    }

    [Test]
    public void null_arguments()
    {
        var collection = new FormattingArgumentsCollection();
        var args = new object?[] { null };
        collection.Format("Value: '{0}'", args).Should().Be("Value: ''");
    }

    [TestCase("{{", "{")]
    [TestCase("}}", "}")]
    public void with_escaped_placehodlers(string format, string formatted)
    {
        var collection = new FormattingArgumentsCollection();
        collection.Format(format).Should().Be(formatted);
    }

    [Test]
    public void applying_specified_formats()
    {
        using (CultureInfoScope.NewInvariant())
        {
            var collection = new FormattingArgumentsCollection(new CultureInfo("nl-BE"))
            {
                { typeof(Date), "yyyy-MM-dd HH:mm" },
                { typeof(decimal), "0.000" }
            };

            var formatted = collection.Format("{0:000.00} - {1} * {1:dd-MM-yyyy} - {2} - {3} - {4}", 3, new Date(2014, 10, 8), 666, 0.8m, 0.9);
            formatted.Should().Be("003,00 - 2014-10-08 00:00 * 08-10-2014 - 666 - 0,800 - 0,9");
        }
    }

    [Test]
    public void applying_custom_format_provider()
    {
        using (CultureInfoScope.NewInvariant())
        {
            var collection = new FormattingArgumentsCollection(FormatProvider.CustomFormatter)
            {
                { typeof(Date), "yyyy-MM-dd HH:mm" },
                { typeof(decimal), "0.000" }
            };

            var formattted = collection.Format("{0:yyyy-MM-dd} * {0}", new Date(2014, 10, 8));
            formattted.Should().Be("Unit Test Formatter, value: '2014-10-08', format: 'yyyy-MM-dd' * Unit Test Formatter, value: '10/08/2014', format: ''");
        }
    }
}

public class Does_not_support_formats
{
    private static readonly FormattingArgumentsCollection Collection = new(TestCultures.en_GB);

    [Test]
    public void with_length_not_being_a_number()
        => "{0,a}".Invoking(format => Collection.Format(format)).Should().Throw<FormatException>();

    [Test]
    public void with_index_not_being_a_number()
        => "{x}".Invoking(format => Collection.Format(format)).Should().Throw<FormatException>();

    [Test]
    public void with_index_not_being_negative()
        => "{-1}".Invoking(format => Collection.Format(format)).Should().Throw<FormatException>();

    [TestCase("{")]
    [TestCase("{0")]
    [TestCase("}")]
    public void with_mallformed_placeholders(string format)
        => format.Invoking(f => Collection.Format(f)).Should().Throw<FormatException>();

    [Test]
    public void indexes_beging_out_of_range()
        => "{0}{1}".Invoking(format => Collection.Format(format, 1))
        .Should()
            .Throw<FormatException>()
            .WithMessage("Index (zero based) must be greater than or equal to zero and less than the size of the argument list.");
}

public class Stringifies
{
    [Test]
    public void IFormattable_null_as_null()
    {
        IFormattable? obj = null;
        var collection = new FormattingArgumentsCollection();
        collection.ToString(obj).Should().BeNull();
    }
    [Test]
    public void object_null_as_null()
    {
        var collection = new FormattingArgumentsCollection();
        collection.ToString((object?)null).Should().BeNull();
    }
    [Test]
    public void object_as_ToString_value()
    {
        var collection = new FormattingArgumentsCollection();
        collection.ToString(typeof(int)).Should().Be("System.Int32");
    }

    [Test]
    public void appying_peferred_formats()
    {
        var collection = new FormattingArgumentsCollection
        {
            { typeof(int), "000" }
        };
        collection.ToString((object)7).Should().Be("007");
    }
}

public class Can_be_modified
{
    public class Adding
    {
        [Test]
        public void format()
        {
            var collection = new FormattingArgumentsCollection();
            collection.Should().BeEmpty();
            collection.Add(typeof(int), "Int32Format");
            collection.Should().ContainSingle();
        }

        [Test]
        public void IFormatProvider()
        {
            var collection = new FormattingArgumentsCollection();
            collection.Should().BeEmpty();
            collection.Add(typeof(int), new CultureInfo("nl-NL"));
            collection.Should().ContainSingle();
        }

        [Test]
        public void format_with_IFormatProvider()
        {
            var collection = new FormattingArgumentsCollection();
            collection.Should().BeEmpty();
            collection.Add(typeof(int), "Int32Format", new CultureInfo("nl-NL"));
            collection.Should().ContainSingle();
        }
    }

    public class Setting
    {
        [Test]
        public void format()
        {
            var collection = new FormattingArgumentsCollection();
            collection.Should().BeEmpty();
            collection.Set(typeof(int), "Int32Format");
            collection.Should().ContainSingle();
        }

        [Test]
        public void IFormatProvider()
        {
            var collection = new FormattingArgumentsCollection();
            collection.Should().BeEmpty();
            collection.Set(typeof(int), new CultureInfo("nl-NL"));
            collection.Should().ContainSingle();
        }

        [Test]
        public void format_with_IFormatProvider()
        {
            var collection = new FormattingArgumentsCollection();
            collection.Should().BeEmpty();
            collection.Set(typeof(int), "Int32Format", new CultureInfo("nl-NL"));
            collection.Should().ContainSingle();
        }

        [Test]
        public void same_key_twice()
        {
            var collection = new FormattingArgumentsCollection();
            collection.Set(typeof(int), "New");
            collection.Set(typeof(int), "Update");
            collection.Should().ContainSingle();
        }
    }

    public class Removing
    {
        [Test]
        public void non_cotained_key()
        {
            var collection = new FormattingArgumentsCollection();
            collection.Remove(typeof(int)).Should().BeFalse();
        }

        [Test]
        public void contained_key()
        {
            var collection = new FormattingArgumentsCollection
            {
                { typeof(int), "Int32Format" },
                { typeof(long), "Int64Format" },
            };

            collection.Remove(typeof(int)).Should().BeTrue();
            collection.Should().ContainSingle();
        }
    }

    [Test]
    public void Clear_CollectionWithTwoItems_0Items()
    {
        var collection = new FormattingArgumentsCollection
        {
            { typeof(int), "Int32Format" },
            { typeof(Date), "Date" }
        };
        collection.Clear();

        var act = collection.Count;
        var exp = 0;

        act.Should().Be(exp);
    }
}

public class Can_be_queried
{
    [Test]
    public void on_types()
    {
        var collection = new FormattingArgumentsCollection
        {
            { typeof(int), "Int32Format" },
            { typeof(Date), "Date" }
        };

        var act = collection.Types;
        var exp = new[] { typeof(int), typeof(Date) };

        act.Should().BeEquivalentTo(exp);
    }

    [Test]
    public void on_types_contained()
    {
        var collection = new FormattingArgumentsCollection
        {
            { typeof(int), "00" }
        };
        collection.Contains(typeof(int)).Should().BeTrue();
    }

    [Test]
    public void on_types_not_contained()
    {
        var collection = new FormattingArgumentsCollection();
        collection.Contains(typeof(int)).Should().BeFalse();
    }

    [Test]
    public void via_enumerator()
    {
        var collection = new FormattingArgumentsCollection
        {
            { typeof(int), "Int32Format" },
            { typeof(Date), "Date" }
        };

        var ienumerable = collection as IEnumerable<KeyValuePair<Type, FormattingArguments>>;
        ienumerable.Should().HaveCount(2);
    }
}

public class Does_not_support
{
    private static readonly FormattingArgumentsCollection Collection = new(TestCultures.en_GB);

    [Test]
    public void types_who_are_not_formattable()
        => Collection.Invoking(c => c.Add(typeof(Type), ""))
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("The argument must implement System.IFormattable.*");

    [Test]
    public void same_key_twice()
    {
        var collection = new FormattingArgumentsCollection
        {
            { typeof(int), "New" }
        };

        collection.Invoking(c => c.Add(typeof(int), "Update"))
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("An item with the same key has already been added.*");
    }
}