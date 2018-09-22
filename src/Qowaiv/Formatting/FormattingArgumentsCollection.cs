using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace Qowaiv.Formatting
{
    /// <summary>Represents a collection of formatting arguments.</summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class FormattingArgumentsCollection : IEnumerable<KeyValuePair<Type, FormattingArguments>>
    {
        /// <summary>Initializes a new instance of a formatting arguments collection based on the current culture.</summary>
        public FormattingArgumentsCollection() : this(CultureInfo.CurrentCulture) { }

        /// <summary>Initializes a new instance of a formatting arguments collection based on the specified format provider.</summary>
        /// <param name="formatProvider">
        /// The default format provider.
        /// </param>
        public FormattingArgumentsCollection(IFormatProvider formatProvider) : this(formatProvider, null) { }

        /// <summary>Initializes a new instance of a formatting arguments collection based on the specified format provider.</summary>
        /// <param name="formatProvider">
        /// The default format provider.
        /// </param>
        /// <param name="parent">
        /// the optional parent collection to inherit item from.
        /// </param>
        public FormattingArgumentsCollection(IFormatProvider formatProvider, FormattingArgumentsCollection parent)
        {
            FormatProvider = Guard.NotNull(formatProvider, "formatProvider");

            if (parent != null)
            {
                foreach (var kvp in parent.dict)
                {
                    dict[kvp.Key] = kvp.Value;
                }
            }
        }

        /// <summary>The underlying dictionary.</summary>
        private readonly Dictionary<Type, FormattingArguments> dict = new Dictionary<Type, FormattingArguments>();

        /// <summary>Gets the default format provider of the collection.</summary>
        public IFormatProvider FormatProvider { get; protected set; }

        /// <summary>Formats the object using the formatting arguments from the collection.</summary>
        /// <param name="obj">
        /// The IFormattable object to get the formatted string representation from.
        /// </param>
        /// <returns>
        /// A formatted string representing the object.
        /// </returns>
        public string ToString(IFormattable obj)
        {
            if (obj is null)
            {
                return null;
            }
            var arguments = Get(obj.GetType());

            return arguments.ToString(obj);
        }

        /// <summary>Formats the object using the formatting arguments of the collection.</summary>
        /// <param name="obj">
        /// The object to get the formatted string representation from.
        /// </param>
        /// <returns>
        /// A formatted string representing the object.
        /// </returns>
        /// <remarks>
        /// If the object does not implement IFormattable, the ToString() will be used.
        /// </remarks>
        public string ToString(object obj)
        {
            return 
                obj is IFormattable formattable
                ? ToString(formattable)
                : obj?.ToString();
        }

        /// <summary>Replaces the format item in a specified string with the string representation
        /// of a corresponding object in a specified array. 
        /// </summary>
        /// <param name="format">
        /// A composite format string.
        /// </param>
        /// <param name="args">
        /// An object array that contains zero or more objects to format.
        /// </param>
        /// <returns>
        /// A copy of format in which the format items have been replaced by the string
        /// representation of the corresponding objects in args.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// format or args is null.
        /// </exception>
        /// <exception cref="FormatException">
        /// format is invalid.-or- The index of a format item is less than zero, or greater
        /// than or equal to the length of the args array.
        /// </exception>
        /// <remarks>
        /// This implementation is a (tweaked) copy of the implementation of <see cref="string"/>.Format().
        /// </remarks>
        [SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames", MessageId = "0#",
            Justification = "Follows the origin string.Format(format, args).")]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", 
            Justification = "Just a copy of the .NET code. It is as complex as it is.")]
        public string Format(string format, params object[] args)
        {
            Guard.NotNull(format, "format");
            Guard.NotNull(args, "args");

            var sb = new StringBuilder();
            int pos = 0;
            int len = format.Length;
            char ch = '\x0';

            var provider = this.FormatProvider;

            // This is different form string.Format, as the provider is never null.
            ICustomFormatter cf = (ICustomFormatter)provider.GetFormat(typeof(ICustomFormatter));

            while (true)
            {
                int p = pos;
                int i = pos;
                while (pos < len)
                {
                    ch = format[pos];

                    pos++;
                    if (ch == '}')
                    {
                        if (pos < len && format[pos] == '}') // Treat as escape character for }}
                            pos++;
                        else
                            FormatError();
                    }

                    if (ch == '{')
                    {
                        if (pos < len && format[pos] == '{') // Treat as escape character for {{
                            pos++;
                        else
                        {
                            pos--;
                            break;
                        }
                    }

                    sb.Append(ch);
                }

                if (pos == len) break;
                pos++;
                if (pos == len || (ch = format[pos]) < '0' || ch > '9') FormatError();
                int index = 0;
                do
                {
                    index = index * 10 + ch - '0';
                    pos++;
                    if (pos == len) FormatError();
                    ch = format[pos];
                } while (ch >= '0' && ch <= '9' && index < 1000000);
                if (index >= args.Length) FormatErrorIndexOutOfRange();
                while (pos < len && (ch = format[pos]) == ' ') pos++;
                bool leftJustify = false;
                int width = 0;
                if (ch == ',')
                {
                    pos++;
                    while (pos < len && format[pos] == ' ') pos++;

                    if (pos == len) FormatError();
                    ch = format[pos];
                    if (ch == '-')
                    {
                        leftJustify = true;
                        pos++;
                        if (pos == len) FormatError();
                        ch = format[pos];
                    }
                    if (ch < '0' || ch > '9') FormatError();
                    do
                    {
                        width = width * 10 + ch - '0';
                        pos++;
                        if (pos == len) FormatError();
                        ch = format[pos];
                    } while (ch >= '0' && ch <= '9' && width < 1000000);
                }

                while (pos < len && (ch = format[pos]) == ' ') pos++;
                Object arg = args[index];
                StringBuilder fmt = null;
                if (ch == ':')
                {
                    pos++;
                    p = pos;
                    i = pos;
                    while (true)
                    {
                        if (pos == len) FormatError();
                        ch = format[pos];
                        pos++;
                        if (ch == '{')
                        {
                            if (pos < len && format[pos] == '{')  // Treat as escape character for {{
                                pos++;
                            else
                                FormatError();
                        }
                        else if (ch == '}')
                        {
                            if (pos < len && format[pos] == '}')  // Treat as escape character for }}
                                pos++;
                            else
                            {
                                pos--;
                                break;
                            }
                        }

                        if (fmt == null)
                        {
                            fmt = new StringBuilder();
                        }
                        fmt.Append(ch);
                    }
                }
                if (ch != '}') FormatError();
                pos++;
                string sFmt = null;
                string s = null;
                if (cf != null)
                {
                    if (fmt != null)
                    {
                        sFmt = fmt.ToString();
                    }
                    s = cf.Format(sFmt, arg, provider);
                }

                if (s == null)
                {
                    IFormattable formattableArg = arg as IFormattable;

                    if (formattableArg != null)
                    {
                        if (sFmt == null && fmt != null)
                        {
                            sFmt = fmt.ToString();
                        }

                        // This is different from string.Format.
                        // If no format is specified, search for a preferred format in the collection.
                        if (string.IsNullOrEmpty(sFmt))
                        {
                            s = ToString(formattableArg);
                        }
                        else
                        {
                            s = formattableArg.ToString(sFmt, provider);
                        }
                    }
                    else if (arg != null)
                    {
                        s = arg.ToString();
                    }
                }

                if (s == null) s = string.Empty;
                int pad = width - s.Length;
                if (!leftJustify && pad > 0) sb.Append(' ', pad);
                sb.Append(s);
                if (leftJustify && pad > 0) sb.Append(' ', pad);
            }
            return sb.ToString();
        }

        private static void FormatError() { throw new FormatException(QowaivMessages.FormatException_InvalidFormat); }
        private static void FormatErrorIndexOutOfRange() { throw new FormatException(QowaivMessages.FormatException_IndexOutOfRange); }

        #region Collection/dictionary related

        /// <summary>Adds a format for the specified type.</summary>
        /// <param name="type">
        /// The type to specify a format for.
        /// </param>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The type is null.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// The type represents a type not implementing System.IFormattable.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// An element with the same type already exists in the collection.
        /// </exception>
        [SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", 
            MessageId = "Qowaiv.Formatting.FormattingArguments.#ctor(System.String)",
            Justification = "Right culture selected by the default constructor.")]
        public void Add(Type type, string format) { Add(type, new FormattingArguments(format)); }

        /// <summary>Adds a format provider for the specified type.</summary>
        /// <param name="type">
        /// The type to specify a format for.
        /// </param>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The type is null.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// The type represents a type not implementing System.IFormattable.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// An element with the same type already exists in the collection.
        /// </exception>
        public void Add(Type type, IFormatProvider formatProvider) { Add(type, new FormattingArguments(formatProvider)); }

        /// <summary>Adds a format and format provider for the specified type.</summary>
        /// <param name="type">
        /// The type to specify a format for.
        /// </param>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The type is null.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// The type represents a type not implementing System.IFormattable.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// An element with the same type already exists in the collection.
        /// </exception>
        public void Add(Type type, string format, IFormatProvider formatProvider) { Add(type, new FormattingArguments(format, formatProvider)); }

        /// <summary>Adds a format and format provider for the specified type.</summary>
        /// <param name="type">
        /// The type to specify a format for.
        /// </param>
        /// <param name="arguments">
        /// The formatting arguments.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The type is null.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// The type represents a type not implementing System.IFormattable.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// An element with the same type already exists in the collection.
        /// </exception>
        public void Add(Type type, FormattingArguments arguments) 
        {
            Guard.ImplementsInterface(type, "type", typeof(IFormattable), QowaivMessages.ArgumentException_NotIFormattable);
            dict.Add(type, arguments); 
        }


        /// <summary>Sets a format for the specified type.</summary>
        /// <param name="type">
        /// The type to specify a format for.
        /// </param>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The type is null.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// The type represents a type not implementing System.IFormattable.
        /// </exception>
        [SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider",
            MessageId = "Qowaiv.Formatting.FormattingArguments.#ctor(System.String)",
            Justification = "Right culture selected by the default constructor.")]
        public void Set(Type type, string format) { Set(type, new FormattingArguments(format)); }

        /// <summary>Sets a format provider for the specified type.</summary>
        /// <param name="type">
        /// The type to specify a format for.
        /// </param>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The type is null.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// The type represents a type not implementing System.IFormattable.
        /// </exception>
        public void Set(Type type, IFormatProvider formatProvider) { Set(type, new FormattingArguments(formatProvider)); }

        /// <summary>Sets a format and format provider for the specified type.</summary>
        /// <param name="type">
        /// The type to specify a format for.
        /// </param>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The type is null.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// The type represents a type not implementing System.IFormattable.
        /// </exception>
        public void Set(Type type, string format, IFormatProvider formatProvider) { Set(type, new FormattingArguments(format, formatProvider)); }

        /// <summary>Sets a format and format provider for the specified type.</summary>
        /// <param name="type">
        /// The type to specify a format for.
        /// </param>
        /// <param name="arguments">
        /// The formatting arugments.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The type is null.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// The type represents a type not implementing System.IFormattable.
        /// </exception>
        public void Set(Type type, FormattingArguments arguments) {
            Guard.ImplementsInterface(type, "type", typeof(IFormattable), QowaivMessages.ArgumentException_NotIFormattable);
            dict[type] = arguments; 
        }

        /// <summary>Returns true if the collection contains the type, otherwise false.</summary>
        /// <param name="type">
        /// The type to test for.
        /// </param>
        public bool Contains(Type type) { return dict.ContainsKey(type); }

        /// <summary>Removes the formatting arguments for the specified type.</summary>
        /// <param name="type">
        /// The type to remove from the collection.
        /// </param>
        /// <returns>
        /// True if type was removed, otherwise false.
        /// </returns>
        public bool Remove(Type type) { return dict.Remove(type); }

        /// <summary>Gets the formatting arguments for the specified type.</summary>
        /// <param name="type">
        /// The type to get the format for.
        /// </param>
        /// <returns>
        /// Returns formatting arguments.
        /// </returns>
        /// <remarks>
        /// If no specific formatting arguments are specified for the type, the
        /// default formatting arguments are returned.
        /// </remarks>
        public FormattingArguments Get(Type type)
        {
            string format = null;
            IFormatProvider formatProvider = null;

            FormattingArguments arguments;

            if (dict.TryGetValue(type, out arguments))
            {
                format = arguments.Format;
                formatProvider = arguments.FormatProvider;
            }
            arguments = new FormattingArguments(format, formatProvider ?? this.FormatProvider);

            return arguments;
        }

        /// <summary>Gets a collection containing the types for the collection.</summary>
        public ICollection<Type> Types { get { return dict.Keys; } }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        IEnumerator<KeyValuePair<Type, FormattingArguments>> IEnumerable<KeyValuePair<Type, FormattingArguments>>.GetEnumerator() { return GetEnumerator(); }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <remarks>
        /// this is used by IEnumerable.GetObjectData() so that it can be
        /// changed by derived classes.
        /// </remarks>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "Required by the interface.")]
        protected virtual IEnumerator<KeyValuePair<Type, FormattingArguments>> GetEnumerator() { return dict.GetEnumerator(); }

        /// <summary>Clears all formatting arguments in the collection.</summary>
        public void Clear() { dict.Clear(); }

        /// <summary>Gets the number of formatting arguments in the collection.</summary>
        public int Count { get { return dict.Count; } }

        #endregion

        /// <summary>Returns a <see cref="string"/> that represents the current formatting arguments collection for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never), SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called by Debugger.")]
        private string DebuggerDisplay
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    "FormattingArgumentsCollection: '{0}', Items: {1}", this.FormatProvider, this.Count);
            }
        }
    }
}
