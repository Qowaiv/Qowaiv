using System.ComponentModel;
using System.Diagnostics.Contracts;

namespace Qowaiv.TestTools
{
    /// <summary>Helper method to convert objects using <see cref="TypeConverter"/>s.</summary>
    public static class Converting
    {
        /// <summary>Creates a new instance of the <see cref="ConvertTo{T}"/> class.</summary>
        [Pure]
        public static ConvertTo<T> Value<T>(T value) => new(value);

        /// <summary>Creates a new instance of the <see cref="ConvertFrom{T}"/> class.</summary>
        [Pure]
        public static ConvertFrom<T> To<T>() => new();
    }

    /// <summary>ConvertFrom builder.</summary>
    public sealed class ConvertFrom<T>
    {
        internal ConvertFrom() { }

        /// <summary>Converts the text to the desired type using the <see cref="TypeConverter"/> of the desired type.</summary>
        [Pure]
        public T FromString(string text) => (T)Converter().ConvertFromString(text);

        /// <summary>Converts the value to the desired type using the <see cref="TypeConverter"/> of the desired type.</summary>
        [Pure]
        public T From(object value) => (T)Converter().ConvertFrom(value);

        [Pure]
        private TypeConverter Converter() => TypeDescriptor.GetConverter(typeof(T));
    }

    /// <summary>ConvertTo builder.</summary>
    public sealed class ConvertTo<T>
    {
        internal ConvertTo(T subject) => Subject = subject;

        /// <summary>The subject that can be convertered to a destination type.</summary>
        public T Subject { get; }

        /// <summary>Converts the value to a string, using the <see cref="TypeConverter"/> defined for the type of the value.</summary>
        [Pure]
        public TDesintation To<TDesintation>() => (TDesintation)Converter().ConvertTo(Subject, typeof(TDesintation));

        /// <summary>Converts the value to a destination type, using the <see cref="TypeConverter"/> defined for the type of the value.</summary>
        [Pure]
        public override string ToString() => Converter().ConvertToString(Subject);

        [Pure]
        private TypeConverter Converter() => TypeDescriptor.GetConverter(typeof(T));
    }

    
}
