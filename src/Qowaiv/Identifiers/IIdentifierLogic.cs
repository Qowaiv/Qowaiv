using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Qowaiv.Identifiers
{
    /// <summary>Injectable logic for strongly typed identfiers.</summary>
    public interface IIdentifierLogic : IEqualityComparer<object>, IComparer<object>, IComparer
    {
        /// <summary>Returns a type converter for the type of the underlying value.</summary>
        TypeConverter Converter { get; }

        /// <summary>Compares the underlying values and returns a value indicating
        /// whether one is less than, equal to, or greater than the other.
        /// </summary>
        new int Compare(object x, object y);

        /// <summary>Returns a formatted <see cref="string"/> that represents the underling value of the identifier.</summary>
        string ToString(object obj, string format, IFormatProvider formatProvider);

        /// <summary>Serializes the underlying value to a JSON node.</summary>
        object ToJson(object obj);

        /// <summary>Tries to parse the underling value of the identifier.</summary>
        /// <param name="str">
        /// The <see cref="string"/> to parse.
        /// </param>
        /// <param name="id">
        /// The parsed identifier.
        /// </param>
        /// <returns>
        /// True if the <see cref="string"/> could be parsed.
        /// </returns>
        bool TryParse(string str, out object id);
    }
}
