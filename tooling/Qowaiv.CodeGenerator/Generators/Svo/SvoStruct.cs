using System;
using System.Collections.Generic;

namespace Qowaiv.CodeGenerator.Generators.Svo
{
    /// <summary>Represents the domain struct representation for the templates.</summary>
    public class SvoStruct
    {
        /// <summary>Constructor.</summary>
        public SvoStruct()
        {
            Namespace = "Qowaiv";
            a = "a";
            UnderlyingType = typeof(string);
        }

        /// <summary>The type of the underlying value.</summary>
        public Type UnderlyingType { get; set; }
        /// <summary>The namespace of the struct.</summary>
        public string Namespace { get; set; }
        /// <summary>The class name of the struct.</summary>
        public string ClassName { get; set; }
        /// <summary>The describing class name of the struct.</summary>
        public string ClassLongName { get; set; }

        /// <summary>The class name of the unit test.</summary>
        public string ClassNameTest => ClassName + "Test";
        /// <summary>The class name of the serialize object for the unit test.</summary>
        public string ClassNameSerializeObject => ClassName + "SerializeObject";

        /// <summary>Returns true if the underlying type is String, otherwise false.</summary>
        public bool IsStringBased => UnderlyingType == typeof(string);

        /// <summary>Returns true if the underlying type is a floating point, otherwise false.</summary>
        public bool IsFloatBased => UnderlyingType == typeof(Single) || UnderlyingType == typeof(Double) || UnderlyingType == typeof(Decimal);

        /// <summary>Returns true if the underlying type is an Integer, otherwise false.</summary>
        public bool IsIntegerBased
        {
            get =>
                UnderlyingType == typeof(Byte) ||
                UnderlyingType == typeof(Int16) ||
                UnderlyingType == typeof(Int32) ||
                UnderlyingType == typeof(Int64);
        }

        /// <summary>Gets the JavaScript initial value.</summary>
        public string JavaScriptInitialValue
        {
            get
            {
                if (JavaScriptInitialValues.TryGetValue(UnderlyingType, out string str))
                {
                    return str;
                }
                return JavaScriptInitialValues[typeof(Object)];
            }
        }

        /// <summary>Gets the a(n).</summary>
        public string a { get; set; }
        /// <summary>Gets the capitalized a(n).</summary>
        public string aUpper => a[0].ToString().ToUpperInvariant() + a.Substring(1);

        /// <summary>Gets &lt;.</summary>
        public string lt => "<";
        /// <summary>Gets &gt;.</summary>
        public string gt => ">";

        /// <summary>A lookup table for JavaScirpt initial values.</summary>
        internal static readonly Dictionary<Type, string> JavaScriptInitialValues = new Dictionary<Type, string>()
        {
            { typeof(Object), "null" },
            { typeof(string), "''" },
            { typeof(Decimal), "0.0" },
            { typeof(Single), "0.0" },
            { typeof(Double), "0.0" },
            { typeof(Byte), "0" },
            { typeof(Int16), "0" },
            { typeof(Int32), "0" },
            { typeof(Int64), "0" },
        };
    }
}
