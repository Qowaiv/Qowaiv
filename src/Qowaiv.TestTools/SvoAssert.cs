using System.Reflection;

namespace Qowaiv.TestTools;

/// <summary>Assertions on the (implicit) SVO contract.</summary>
public static class SvoAssert
{
    /// <summary>Asserts that the underlying type matches the decorated type. Throws if not.</summary>
    [DebuggerStepThrough]
    public static void UnderlyingTypeMatches(Type svo, SingleValueObjectAttribute attr)
    {
        Assert.IsNotNull(svo, nameof(svo));
        Assert.IsNotNull(attr, nameof(attr));

        var m_Value = svo.GetField("m_Value", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.IsNotNull(m_Value, $"{svo} should contain a m_Value field.");
        Assert.AreEqual(attr.UnderlyingType, m_Value.FieldType, $"{svo}.m_Field");
    }

    /// <summary>Asserts that the Parse contract is met. Throws if not.</summary>
    [DebuggerStepThrough]
    public static void ParseMatches(Type svo, SingleValueObjectAttribute attr)
    {
        Assert.IsNotNull(svo, nameof(svo));
        Assert.IsNotNull(attr, nameof(attr));

        var staticMethods = svo.GetMethods(BindingFlags.Public | BindingFlags.Static);

        var parse = staticMethods.SingleOrDefault(method =>
                method.Name == "Parse" &&
                method.GetParameters().Length == 1 &&
                method.GetParameters()[0].ParameterType == typeof(string) &&
                method.ReturnType == svo);

        var parseCulture = staticMethods.SingleOrDefault(method =>
             method.Name == "Parse" &&
              method.GetParameters().Length == 2 &&
              method.GetParameters()[0].ParameterType == typeof(string) &&
              method.GetParameters()[1].ParameterType == typeof(IFormatProvider) &&
              method.ReturnType == svo);

        if (attr.StaticOptions.HasFlag(SingleValueStaticOptions.Parse))
        {
            Assert.IsNotNull(parse, $"{svo} should contain a static Parse method.");
            if (attr.StaticOptions.HasFlag(SingleValueStaticOptions.CultureDependent))
            {
                Assert.IsNotNull(parseCulture, $"{svo} should contain a static Parse method with an IFormatProvider parameter.");
            }
            else
            {
                Assert.IsNull(parseCulture, $"{svo} should not contain a static Parse method with an IFormatProvider parameter.");
            }
        }
        else
        {
            Assert.IsNull(parse, $"{svo} should not contain a static Parse method.");
            Assert.IsNull(parseCulture, $"{svo} should not contain a static Parse method an IFormatProvider parameter.");
        }
    }

    /// <summary>Asserts that the TryParse contract is met. Throws if not.</summary>
    [DebuggerStepThrough]
    public static void TryParseMatches(Type svo, SingleValueObjectAttribute attr)
    {
        Assert.IsNotNull(svo, nameof(svo));
        Assert.IsNotNull(attr, nameof(attr));

        var staticMethods = svo.GetMethods(BindingFlags.Public | BindingFlags.Static);

        // The out parameter.
        var byrefSvo = svo.MakeByRefType();

        var tryParse = staticMethods.SingleOrDefault(method =>
              method.Name == "TryParse" &&
              method.GetParameters().Length == 2 &&
              method.GetParameters()[0].ParameterType == typeof(string) &&
              method.GetParameters()[1].ParameterType == byrefSvo &&
              method.GetParameters()[1].IsOut &&
              method.ReturnType == typeof(bool));

        var tryParseCulture = staticMethods.SingleOrDefault(method =>
             method.Name == "TryParse" &&
              method.GetParameters().Length == 3 &&
              method.GetParameters()[0].ParameterType == typeof(string) &&
              method.GetParameters()[1].ParameterType == typeof(IFormatProvider) &&
              method.GetParameters()[2].ParameterType == byrefSvo &&
              method.GetParameters()[2].IsOut &&
              method.ReturnType == typeof(bool));

        if (attr.StaticOptions.HasFlag(SingleValueStaticOptions.TryParse))
        {
            Assert.IsNotNull(tryParse, $"{svo} should contain a static TryParse method.");
            if (attr.StaticOptions.HasFlag(SingleValueStaticOptions.CultureDependent))
            {
                Assert.IsNotNull(tryParseCulture, $"{svo} should contain a static TryParse method with an IFormatProvider parameter.");
            }
            else
            {
                Assert.IsNull(tryParseCulture, $"{svo} should not contain a static TryParse method with an IFormatProvider parameter.");
            }
        }
        else
        {
            Assert.IsNull(tryParse, $"{svo} should not contain a static TryParse method.");
            Assert.IsNull(tryParseCulture, $"{svo} should not contain a static TryParse method with an IFormatProvider parameter.");
        }
    }

    /// <summary>Asserts that the IsValid contract is met. Throws if not.</summary>
    [DebuggerStepThrough]
    public static void IsValidMatches(Type svo, SingleValueObjectAttribute attr)
    {
        Assert.IsNotNull(svo, nameof(svo));
        Assert.IsNotNull(attr, nameof(attr));

        var staticMethods = svo.GetMethods(BindingFlags.Public | BindingFlags.Static);

        var isValid = staticMethods.SingleOrDefault(method =>
                method.Name == "IsValid" &&
                method.GetParameters().Length == 1 &&
                method.GetParameters()[0].ParameterType == typeof(string) &&
                method.ReturnType == typeof(bool));

        var isValidCulture = staticMethods.SingleOrDefault(method =>
               method.Name == "IsValid" &&
               method.GetParameters().Length == 2 &&
               method.GetParameters()[0].ParameterType == typeof(string) &&
               method.GetParameters()[1].ParameterType == typeof(IFormatProvider) &&
               method.ReturnType == typeof(bool));

        if (attr.StaticOptions.HasFlag(SingleValueStaticOptions.IsValid))
        {
            Assert.IsNotNull(isValid, $"{svo} should contain a static IsValid method.");
            if (attr.StaticOptions.HasFlag(SingleValueStaticOptions.CultureDependent))
            {
                Assert.IsNotNull(isValidCulture, $"{svo} should contain a static IsValid method with an IFormatProvider parameter.");
            }
            else
            {
                Assert.IsNull(isValidCulture, $"{0} should not contain a static IsValid method with an IFormatProvider parameter.");
            }
        }
        else
        {
            Assert.IsNull(isValid, $"{svo} should not contain a static IsValid method.");
            Assert.IsNull(isValidCulture, $"{svo} should not contain a static IsValid method with an IFormatProvider parameter.");
        }
    }

    /// <summary>Asserts that the EmptyAndUnknown contract is met. Throws if not.</summary>
    [DebuggerStepThrough]
    public static void EmptyAndUnknownMatches(Type svo, SingleValueObjectAttribute attr)
    {
        Assert.IsNotNull(svo, nameof(svo));
        Assert.IsNotNull(attr, nameof(attr));

        var emptyValue = svo.GetFields(BindingFlags.Static | BindingFlags.Public).SingleOrDefault(field =>
                field.Name == "Empty" &&
                field.IsInitOnly &&
                field.FieldType == svo);

        var unknownValue = svo.GetFields(BindingFlags.Static | BindingFlags.Public).SingleOrDefault(field =>
                field.Name == "Unknown" &&
                field.IsInitOnly &&
                field.FieldType == svo);

        var minValue = svo.GetFields(BindingFlags.Static | BindingFlags.Public).SingleOrDefault(field =>
                field.Name == "MinValue" &&
                field.IsInitOnly &&
                field.FieldType == svo);

        var maxValue = svo.GetFields(BindingFlags.Static | BindingFlags.Public).SingleOrDefault(field =>
                field.Name == "MaxValue" &&
                field.IsInitOnly &&
                field.FieldType == svo);

        var isEmptyMethod = svo.GetMethods(BindingFlags.Instance | BindingFlags.Public).SingleOrDefault(method =>
                method.Name == "IsEmpty" &&
                method.GetParameters().Length == 0 &&
                method.ReturnType == typeof(Boolean));

        var isUnknownMethod = svo.GetMethods(BindingFlags.Instance | BindingFlags.Public).SingleOrDefault(method =>
               method.Name == "IsUnknown" &&
               method.GetParameters().Length == 0 &&
               method.ReturnType == typeof(Boolean));

        var isEmptyOrUnknownMethod = svo.GetMethods(BindingFlags.Instance | BindingFlags.Public).SingleOrDefault(method =>
               method.Name == "IsEmptyOrUnknown" &&
               method.GetParameters().Length == 0 &&
               method.ReturnType == typeof(Boolean));



        if (attr.StaticOptions.HasFlag(SingleValueStaticOptions.HasEmptyValue))
        {
            Assert.IsNotNull(emptyValue, $"{svo} should contain a static read-only Empty field.");
            Assert.IsNotNull(isEmptyMethod, $"{svo} should contain a IsEmpty method.");
        }
        else
        {
            Assert.IsNull(emptyValue, $"{svo} should not contain a static read-only Empty field.");
            Assert.IsNull(isEmptyMethod, $"{svo} should not contain a IsEmpty method.");
        }

        if (attr.StaticOptions.HasFlag(SingleValueStaticOptions.HasUnknownValue))
        {
            Assert.IsNotNull(unknownValue, $"{svo} should contain a static read-only Unknown field.");
            Assert.IsNotNull(isUnknownMethod, $"{svo} should contain a IsUnknown method.");
        }
        else
        {
            Assert.IsNull(unknownValue, $"{svo} should not contain a static read-only Unknown field.");
            Assert.IsNull(isUnknownMethod, $"{svo} should not contain a IsUnknown method.");
        }

        if (attr.StaticOptions.HasFlag(SingleValueStaticOptions.HasEmptyValue) && attr.StaticOptions.HasFlag(SingleValueStaticOptions.HasUnknownValue))
        {
            Assert.IsNotNull(isEmptyOrUnknownMethod, $"{svo} should contain a IsEmptyOrUnknown method.");
        }
        else
        {
            Assert.IsNull(isEmptyOrUnknownMethod, $"{svo} should not contain a IsEmptyOrUnknown method.");
        }

        if (attr.StaticOptions == SingleValueStaticOptions.Continuous)
        {
            Assert.IsNotNull(minValue, $"{svo} should contain a static read-only Unknown MinValue.");
            Assert.IsNotNull(maxValue, $"{svo} should contain a static read-only Unknown MaxValue.");
        }
    }

    /// <summary>Asserts that the Interface contracts are met. Throws if not.</summary>
    [DebuggerStepThrough]
    public static void ImplementsInterfaces(Type svo)
    {
        Assert.IsNotNull(svo, nameof(svo));

        var interfaces = svo.GetInterfaces().ToList();

        var iComparable = typeof(IComparable<>).MakeGenericType(svo);

        Assert.IsTrue(interfaces.Contains(typeof(ISerializable)), /*    */ $"{svo} should contain ISerializable.");
        Assert.IsTrue(interfaces.Contains(typeof(IXmlSerializable)), /* */ $"{svo} should contain IXmlSerializable.");
        Assert.IsTrue(interfaces.Contains(typeof(IFormattable)), /*     */ $"{svo} should contain IFormattable.");
        Assert.IsTrue(interfaces.Contains(typeof(IComparable)), /*      */ $"{svo} should contain IComparable.");
        Assert.IsTrue(interfaces.Contains(iComparable), /*              */ $"{svo} should contain IComparable<{svo}>.");
    }

    /// <summary>Asserts that the attribute contracts are met. Throws if not.</summary>
    [DebuggerStepThrough]
    public static void AttributesMatches(Type svo)
    {
        Assert.IsNotNull(svo, nameof(svo));

        var attributes = svo.GetCustomAttributes(false);

        Assert.IsTrue(attributes.Any(att => (att is DebuggerDisplayAttribute)), /**/ $"{svo} should have DebuggerDisplayAttribute.");
        Assert.IsTrue(attributes.Any(att => (att is SerializableAttribute)), /*   */ $"{svo} should have SerializableAttribute.");
        Assert.IsTrue(attributes.Any(att => (att is TypeConverterAttribute)), /*  */ $"{svo} should have TypeConverterAttribute.");
    }
}
