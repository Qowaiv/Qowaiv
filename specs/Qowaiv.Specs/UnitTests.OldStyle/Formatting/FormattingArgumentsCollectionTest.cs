namespace Qowaiv.UnitTests.Formatting;

public class FormattingArgumentsCollectionTest
{
    [Test]
    public void Ctor_WithParent_ThrowsFormatException()
    {
        var parent = new FormattingArgumentsCollection(new CultureInfo("nl"))
        {
            { typeof(Date), "yyyy-MM" }
        };

        var act = new FormattingArgumentsCollection(new CultureInfo("nl-BE"), parent);
        Assert.AreEqual(1, act.Count, "act.Count");

        parent.Add(typeof(Sex), "f");
        Assert.AreEqual(2, parent.Count, "parent.Count");
        Assert.AreEqual(1, act.Count, "act.Count");
    }

    [Test]
    public void Format_LengthPlus_ThrowsFormatException()
    {
        Assert.Catch<FormatException>(() =>
        {
            var collection = new FormattingArgumentsCollection(new CultureInfo("nl-BE"));
            collection.Format("{0,+}", 1);
        },
        "Input string was not in a correct format.");
    }
    [Test]
    public void Format_LengthA_ThrowsFormatException()
    {
        Assert.Catch<FormatException>(() =>
        {
            var collection = new FormattingArgumentsCollection(new CultureInfo("nl-BE"));
            collection.Format("{0,a}", 1);
        },
        "Input string was not in a correct format.");
    }

    [Test]
    public void Format_IndexPlus_ThrowsFormatException()
    {
        Assert.Catch<FormatException>(() =>
        {
            var collection = new FormattingArgumentsCollection(new CultureInfo("nl-BE"));
            collection.Format("{+}");
        },
        "Input string was not in a correct format.");
    }
    [Test]
    public void Format_IndexA_ThrowsFormatException()
    {
        Assert.Catch<FormatException>(() =>
        {
            var collection = new FormattingArgumentsCollection(new CultureInfo("nl-BE"));
            collection.Format("{a}");
        },
        "Input string was not in a correct format.");
    }

    [Test]
    public void Format_Index1000000_AreEqual()
    {
        var collection = new FormattingArgumentsCollection(new CultureInfo("nl-BE"));
        var args = new object[1000001];
        args[1000000] = "Test";
        var act = collection.Format("Begin {1000000} End", args);
        var exp = "Begin Test End";

        Assert.AreEqual(exp, act);
    }
    [Test]
    public void Format_InvalidFormat_ThrowsFormatException()
    {
        Assert.Catch<FormatException>(() =>
        {
            var collection = new FormattingArgumentsCollection(new CultureInfo("nl-BE"));
            collection.Format("}");
        },
        "Input string was not in a correct format.");
    }
    [Test]
    public void Format_ElementStartedButNotClosed_ThrowsFormatException()
    {
        Assert.Catch<FormatException>(() =>
        {
            var collection = new FormattingArgumentsCollection(new CultureInfo("nl-BE"));
            collection.Format("Test {0", 12);
        },
        "Input string was not in a correct format.");
    }
    [Test]
    public void Format_UnparsebleIndex_ThrowsFormatException()
    {
        Assert.Catch<FormatException>(() =>
        {
            var collection = new FormattingArgumentsCollection(new CultureInfo("nl-BE"));
            collection.Format("{a}");
        },
        "Input string was not in a correct format.");
    }
    [Test]
    public void Format_IndexOutOfRange_ThrowsFormatException()
    {
        Assert.Catch<FormatException>(() =>
        {
            var collection = new FormattingArgumentsCollection(new CultureInfo("nl-BE"));
            collection.Format("{0}{1}", 1);
        },
        "Index (zero based) must be greater than or equal to zero and less than the size of the argument list.");
    }

    [Test]
    public void Format_ArrayWithNullItem_String()
    {
        var collection = new FormattingArgumentsCollection();

        var act = collection.Format("Value: '{0}'", null);
        var exp = "Value: ''";

        Assert.AreEqual(exp, act);
    }
    [Test]
    public void Format_AlignLeft_String()
    {
        var collection = new FormattingArgumentsCollection();

        var act = collection.Format("{0,-4}", "a");
        var exp = "a   ";

        Assert.AreEqual(exp, act);
    }
    [Test]
    public void Format_AlignRight_String()
    {
        var collection = new FormattingArgumentsCollection();

        var act = collection.Format("{0,3}", "a");
        var exp = "  a";

        Assert.AreEqual(exp, act);
    }
    [Test]
    public void Format_EscapeLeft_String()
    {
        var collection = new FormattingArgumentsCollection();

        var act = collection.Format("{{");
        var exp = "{";

        Assert.AreEqual(exp, act);
    }
    [Test]
    public void Format_EscapeRight_String()
    {
        var collection = new FormattingArgumentsCollection();

        var act = collection.Format("}}");
        var exp = "}";

        Assert.AreEqual(exp, act);
    }
    [Test]
    public void Format_ComplexPattern_AreEqual()
    {
        using (CultureInfoScope.NewInvariant())
        {
            var collection = new FormattingArgumentsCollection(new CultureInfo("nl-BE"))
            {
                { typeof(Date), "yyyy-MM-dd HH:mm" },
                { typeof(decimal), "0.000" }
            };

            var act = collection.Format("{0:000.00} - {1} * {1:dd-MM-yyyy} - {2} - {3} - {4}", 3, new Date(2014, 10, 8), 666, 0.8m, 0.9);
            var exp = "003,00 - 2014-10-08 00:00 * 08-10-2014 - 666 - 0,800 - 0,9";

            Assert.AreEqual(exp, act);
        }
    }
    [Test]
    public void Format_ComplexPatternWithUnitTestFormatProvider_AreEqual()
    {
        using (CultureInfoScope.NewInvariant())
        {
            var collection = new FormattingArgumentsCollection(FormatProvider.CustomFormatter)
            {
                { typeof(Date), "yyyy-MM-dd HH:mm" },
                { typeof(decimal), "0.000" }
            };

            var act = collection.Format("{0:yyyy-MM-dd} * {0}", new Date(2014, 10, 8));
            var exp = "Unit Test Formatter, value: '2014-10-08', format: 'yyyy-MM-dd' * Unit Test Formatter, value: '10/08/2014', format: ''";

            Assert.AreEqual(exp, act);
        }
    }

    [Test]
    public void ToString_IFormattableNull_IsNull()
    {
        var collection = new FormattingArgumentsCollection();
        string act = collection.ToString((IFormattable)null);
        string exp = null;

        Assert.AreEqual(exp, act);
    }
    [Test]
    public void ToString_ObjectNull_IsNull()
    {
        var collection = new FormattingArgumentsCollection();
        string act = collection.ToString((Object)null);
        string exp = null;

        Assert.AreEqual(exp, act);
    }
    [Test]
    public void ToString_TypeInt32_SystemInt32()
    {
        var collection = new FormattingArgumentsCollection();
        string act = collection.ToString((Object)typeof(Int32));
        string exp = "System.Int32";

        Assert.AreEqual(exp, act);
    }
    [Test]
    public void ToString_7_007()
    {
        var collection = new FormattingArgumentsCollection
        {
            { typeof(int), "000" }
        };
        string act = collection.ToString((Object)7);
        string exp = "007";

        Assert.AreEqual(exp, act);
    }

    #region Collection manipulation

    [Test]
    public void Add_NotIFormattebleType_ThrowsArgumentException()
    {
        var collection = new FormattingArgumentsCollection();
        Action add = () => collection.Add(typeof(Type), "");
        add.Should()
            .Throw<ArgumentException>()
            .WithMessage("The argument must implement System.IFormattable.*");
    }

    [Test]
    public void Add_DuplicateKey_ThrowsArgumentException()
    {
        var collection = new FormattingArgumentsCollection
        {
            { typeof(int), "New" }
        };
        Action add = () => collection.Add(typeof(int), "Update");
        add.Should()
            .Throw<ArgumentException>()
            .WithMessage("An item with the same key has already been added.*");
    }

    [Test]
    public void Add_Int32Format_Contains1Item()
    {
        var collection = new FormattingArgumentsCollection
        {
            { typeof(int), "Int32Format" }
        };

        Assert.AreEqual(1, collection.Count, "Count");
    }
    [Test]
    public void Add_Int32CultureInfo_Contains1Item()
    {
        var collection = new FormattingArgumentsCollection
        {
            { typeof(int), new CultureInfo("nl-NL") }
        };

        Assert.AreEqual(1, collection.Count, "Count");
    }
    [Test]
    public void Add_Int32FormatAndFormatProvider_Contains1Item()
    {
        var collection = new FormattingArgumentsCollection
        {
            { typeof(Int32), "Int32Format", new CultureInfo("nl-NL") }
        };

        Assert.AreEqual(1, collection.Count, "Count");
    }

    [Test]
    public void Set_Int32Format_Contains1Item()
    {
        var collection = new FormattingArgumentsCollection();
        collection.Set(typeof(Int32), "Int32Format");

        Assert.AreEqual(1, collection.Count, "Count");
    }
    [Test]
    public void Set_Int32CultureInfo_Contains1Item()
    {
        var collection = new FormattingArgumentsCollection();
        collection.Set(typeof(Int32), new CultureInfo("nl-NL"));

        Assert.AreEqual(1, collection.Count, "Count");
    }
    [Test]
    public void Set_Int32FormatAndFormatProviderContains1Item()
    {
        var collection = new FormattingArgumentsCollection();
        collection.Set(typeof(Int32), "Int32Format", new CultureInfo("nl-NL"));

        Assert.AreEqual(1, collection.Count, "Count");
    }
    [Test]
    public void Set_SameTypeTwice_Contains1Item()
    {
        var collection = new FormattingArgumentsCollection();
        collection.Set(typeof(Int32), "New");
        collection.Set(typeof(Int32), "Update");

        Assert.AreEqual(1, collection.Count, "Count");
    }

    [Test]
    public void Remove_Int32_Unsuccessful()
    {
        var collection = new FormattingArgumentsCollection();
        var act = collection.Remove(typeof(Int32));
        var exp = false;

        Assert.AreEqual(exp, act);
    }
    [Test]
    public void Remove_AddedInt32_Successful()
    {
        var collection = new FormattingArgumentsCollection
        {
            { typeof(int), "Int32Format" }
        };
        var act = collection.Remove(typeof(Int32));
        var exp = true;

        Assert.AreEqual(exp, act);
    }

    [Test]
    public void Types_CollectionWithTwoItems_Int32AndDate()
    {
        var collection = new FormattingArgumentsCollection
        {
            { typeof(int), "Int32Format" },
            { typeof(Date), "Date" }
        };

        var act = collection.Types;
        var exp = new [] { typeof(int), typeof(Date) };

        CollectionAssert.AreEqual(exp, act);
    }

    [Test]
    public void GetEnumerator_IEnumerableKeyValuePair_IsNotNull()
    {
        var collection = new FormattingArgumentsCollection
        {
            { typeof(int), "Int32Format" },
            { typeof(Date), "Date" }
        };

        var ienumerable = collection as IEnumerable<KeyValuePair<Type, FormattingArguments>>;

        var act = ienumerable.GetEnumerator();
        Assert.IsNotNull(act);
    }
    [Test]
    public void GetEnumerator_IEnumerable_IsNotNull()
    {
        var collection = new FormattingArgumentsCollection
        {
            { typeof(int), "Int32Format" },
            { typeof(Date), "Date" }
        };

        var ienumerable = collection as IEnumerable;

        var act = ienumerable.GetEnumerator();
        Assert.IsNotNull(act);
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

        Assert.AreEqual(exp, act);
    }

    [Test]
    public void Contains_FilledCollectionInt32_IsTrue()
    {
        var collection = new FormattingArgumentsCollection
        {
            { typeof(int), "00" }
        };
        var act = collection.Contains(typeof(Int32));
        var exp = true;
        Assert.AreEqual(exp, act);
    }
    [Test]
    public void Contains_EmptyCollectionInt32_IsFalse()
    {
        var collection = new FormattingArgumentsCollection();
        var act = collection.Contains(typeof(Int32));
        var exp = false;
        Assert.AreEqual(exp, act);
    }

    #endregion
}
