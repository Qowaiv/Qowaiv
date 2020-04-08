//-----------------------------------------------------------------------
// <copyright>
//    Copyright (c) Corniel Nobel. All rights reserved.
//    See: https://github.com/Corniel/Grenadiers/blob/master/LICENSE.md
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

namespace Qowaiv
{
    /// <summary>Supplies parameter guarding for methods and constructors.</summary>
    /// <remarks>
    /// Advised usage:
    /// * Change the namespace to maximum shared namespace amongst the using projects
    /// * Keep it internal and use [assembly: InternalsVisibleTo] to open up access
    /// * Add specific Guard methods if you software needs them.
    /// * Keep the checks cheep so that you also can run them in production code.
    /// </remarks>
    [ExcludeFromCodeCoverage]
    internal static partial class Guard
    {
        /// <summary>Guards the parameter if not null, otherwise throws an argument (null) exception.</summary>
        /// <typeparam name="T">The type to guard; cannot be a structure.</typeparam>
        /// <param name="parameter">The parameter to guard.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>
        /// The guarded parameter.
        /// </returns>
        [DebuggerStepThrough]
        public static T NotNull<T>([ValidatedNotNull]T parameter, string paramName)
            where T : class
        {
            if (parameter is null)
            {
                throw new ArgumentNullException(paramName);
            }

            return parameter;
        }

        /// <summary>Throws an ArgumentException if the nullable parameter has no value, otherwise the parameter value is passed.</summary>
        /// <typeparam name="T">The type to guard; must be a structure.</typeparam>
        /// <param name="parameter">The parameter to guard.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>
        /// The guarded parameter.
        /// </returns>
        [DebuggerStepThrough]
        public static T HasValue<T>(T? parameter, string paramName)
            where T : struct
        {
            if (!parameter.HasValue)
            {
                throw new ArgumentException(Messages.ArgumentException_NullableMustHaveValue, paramName);
            }

            return parameter.Value;
        }

        /// <summary>
        /// Throws an ArgumentException if the nullable parameter has no value or the default value,
        /// otherwise the parameter value is passed.
        /// </summary>
        /// <typeparam name="T">The type to guard; must be a structure.</typeparam>
        /// <param name="parameter">The parameter to guard.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>
        /// The guarded parameter.
        /// </returns>
        [DebuggerStepThrough]
        public static T NotDefault<T>(T? parameter, string paramName)
            where T : struct =>
            NotDefault(HasValue(parameter, paramName), paramName);

        /// <summary>Throws an ArgumentException if the parameter has the default value, otherwise the parameter value is passed.</summary>
        /// <typeparam name="T">The type to guard; must be a structure.</typeparam>
        /// <param name="parameter">The parameter to guard.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>
        /// The guarded parameter.
        /// </returns>
        [DebuggerStepThrough]
        public static T NotDefault<T>(T parameter, string paramName)
            where T : struct
        {
            if (parameter.Equals(default(T)))
            {
                throw new ArgumentException(Messages.ArgumentException_IsDefaultValue, paramName);
            }

            return parameter;
        }

        /// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if the parameter is not in the collection, otherwise the parameter is passed.</summary>
        /// <typeparam name="T">The type to guard; must be a structure.</typeparam>
        /// <param name="parameter">The parameter to guard.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="allowedRange">The allowed range of values.</param>
        /// <returns>
        /// The guarded parameter.
        /// </returns>
        [DebuggerStepThrough]
        public static T In<T>(T parameter, string paramName, params T[] allowedRange)
            where T : struct
        {
            if (!allowedRange.Contains(parameter))
            {
                var allowed = string.Join(", ", allowedRange);
                throw new ArgumentOutOfRangeException(paramName, string.Format(CultureInfo.CurrentCulture, Messages.ArgumentOutOfRangeException_NotInCollection, allowed));
            }

            return parameter;
        }

        /// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if the parameter not in the collection, otherwise the parameter is passed.</summary>
        /// <typeparam name="T">The type to guard; must be a structure.</typeparam>
        /// <param name="parameter">The parameter to guard.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="forbiddenRange">The forbidden range of values.</param>
        /// <returns>
        /// The guarded parameter.
        /// </returns>
        [DebuggerStepThrough]
        public static T NotIn<T>(T parameter, string paramName, params T[] forbiddenRange)
            where T : struct
        {
            if (forbiddenRange.Contains(parameter))
            {
                var forbidden = string.Join(", ", forbiddenRange);
                throw new ArgumentOutOfRangeException(paramName, string.Format(CultureInfo.CurrentCulture, Messages.ArgumentOutOfRangeException_InCollection, forbidden));
            }

            return parameter;
        }

        /// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if the parameter not in not a defined value of the enum, otherwise the parameter is passed.</summary>
        /// <typeparam name="T">The type to guard; must be a structure (enum).</typeparam>
        /// <param name="parameter">The parameter to guard.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>
        /// The guarded parameter.
        /// </returns>
        /// <remarks>
        /// That <typeparamref name="T"/> is an enum is implicitly guard by <see cref="Enum.IsDefined(Type, object)"/>.
        /// </remarks>
        public static T DefinedEnum<T>(T parameter, string paramName)
            where T : struct
        {
            if (Enum.IsDefined(typeof(T), parameter))
            {
                return parameter;
            }

            throw new ArgumentOutOfRangeException(paramName, string.Format(CultureInfo.CurrentCulture, Messages.ArgumentOutOfRangeException_DefinedEnum, parameter, typeof(T)));
        }

        /// <summary>
        /// Guards that the parameter is an instance of T, otherwise throws an argument (null) exception.
        /// It only makes sense to use this function if the original type of the <paramref name="parameter"/>
        /// is <see cref="object"/>, otherwise one should simply use <see cref="NotNull{T}(T, string)"/>.
        /// </summary>
        /// <typeparam name="T">The type to guard.</typeparam>
        /// <param name="parameter">The parameter to guard.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>
        /// The guarded parameter.
        /// </returns>
        [DebuggerStepThrough]
#pragma warning disable S4018 // Generic methods should provide type parameters, but here it provides casting.
        public static T IsInstanceOf<T>(object parameter, string paramName)
#pragma warning restore S4018 // Generic methods should provide type parameters
        {
            NotNull(parameter, paramName);
            if (!(parameter is T instance))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Messages.ArgumentException_NotAnInstanceOf, typeof(T)), paramName);
            }

            return instance;
        }

        /// <summary>Guards that the parameter is not null or an empty collection, otherwise throws an argument (null) exception.</summary>
        /// <typeparam name="T">The type to guard; must be an <see cref="ICollection" />.</typeparam>
        /// <param name="parameter">The parameter to guard.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>
        /// The guarded parameter.
        /// </returns>
        [DebuggerStepThrough]
        public static T HasAny<T>([ValidatedNotNull]T parameter, string paramName)
            where T : class, ICollection
        {
            NotNull(parameter, paramName);
            if (parameter.Count == 0)
            {
                throw new ArgumentException(Messages.ArgumentException_EmptyCollection, paramName);
            }

            return parameter;
        }

        /// <summary>Guards that the parameter is not null or an empty enumerable, otherwise throws an argument (null) exception.</summary>
        /// <typeparam name="T">The type to guard; must be an <see cref="IEnumerable" />.</typeparam>
        /// <param name="parameter">The parameter to guard.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>
        /// The guarded parameter.
        /// </returns>
        [DebuggerStepThrough]
        public static IEnumerable<T> HasAny<T>([ValidatedNotNull]IEnumerable<T> parameter, string paramName)
        {
            NotNull(parameter, paramName);
            if (!parameter.Any())
            {
                throw new ArgumentException(Messages.ArgumentException_EmptyCollection, paramName);
            }

            return parameter;
        }

        /// <summary>Guards that the parameter is not null or an empty string, otherwise throws an argument (null) exception.</summary>
        /// <param name="parameter">The parameter to guard.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>
        /// The guarded parameter.
        /// </returns>
        [DebuggerStepThrough]
        public static string NotNullOrEmpty([ValidatedNotNull]string parameter, string paramName)
        {
            NotNull(parameter, paramName);
            if (parameter == string.Empty)
            {
                throw new ArgumentException(Messages.ArgumentException_StringEmpty, paramName);
            }

            return parameter;
        }

        /// <summary>Guards that the parameter is not an empty <see cref="Guid"/>, otherwise throws an argument exception.</summary>
        /// <param name="parameter">The parameter to guard.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>
        /// The guarded parameter.
        /// </returns>
        [DebuggerStepThrough]
        public static Guid NotEmpty(Guid? parameter, string paramName) => NotEmpty(HasValue(parameter, paramName), paramName);

        /// <summary>Guards that the parameter is not an empty <see cref="Guid"/>, otherwise throws an argument exception.</summary>
        /// <param name="parameter">The parameter to guard.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>
        /// The guarded parameter.
        /// </returns>
        [DebuggerStepThrough]
        public static Guid NotEmpty(Guid parameter, string paramName)
        {
            if (parameter == Guid.Empty)
            {
                throw new ArgumentException(Messages.ArgumentException_GuidEmpty, paramName);
            }

            return parameter;
        }

        /// <summary>Throws an ArgumentException if the parameter is not positive, otherwise the parameter is passed.</summary>
        /// <param name="parameter">The parameter to guard.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>
        /// The guarded parameter.
        /// </returns>
        [DebuggerStepThrough]
        public static int Positive(int? parameter, string paramName) => Positive(HasValue(parameter, paramName), paramName);

        /// <summary>Throws an ArgumentException if the parameter is not positive, otherwise the parameter is passed.</summary>
        /// <param name="parameter">The parameter to guard.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>
        /// The guarded parameter.
        /// </returns>
        [DebuggerStepThrough]
        public static int Positive(int parameter, string paramName)
        {
            if (parameter <= 0)
            {
                throw new ArgumentOutOfRangeException(paramName, Messages.ArgumentOutOfRangeException_NotPositive);
            }

            return parameter;
        }

        /// <summary>Throws an ArgumentException if the parameter is not positive, otherwise the parameter is passed.</summary>
        /// <param name="parameter">The parameter to guard.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>
        /// The guarded parameter.
        /// </returns>
        [DebuggerStepThrough]
        public static long Positive(long? parameter, string paramName) => Positive(HasValue(parameter, paramName), paramName);

        /// <summary>Throws an ArgumentException if the parameter is not positive, otherwise the parameter is passed.</summary>
        /// <param name="parameter">The parameter to guard.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>
        /// The guarded parameter.
        /// </returns>
        [DebuggerStepThrough]
        public static long Positive(long parameter, string paramName)
        {
            if (parameter <= 0)
            {
                throw new ArgumentOutOfRangeException(paramName, Messages.ArgumentOutOfRangeException_NotPositive);
            }

            return parameter;
        }

        /// <summary>Throws an ArgumentException if the parameter is not positive, otherwise the parameter is passed.</summary>
        /// <param name="parameter">The parameter to guard.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>
        /// The guarded parameter.
        /// </returns>
        [DebuggerStepThrough]
        public static double Positive(double? parameter, string paramName) => Positive(HasValue(parameter, paramName), paramName);

        /// <summary>Throws an ArgumentException if the parameter is not positive, otherwise the parameter is passed.</summary>
        /// <param name="parameter">The parameter to guard.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>
        /// The guarded parameter.
        /// </returns>
        [DebuggerStepThrough]
        public static double Positive(double parameter, string paramName)
        {
            if (parameter <= 0)
            {
                throw new ArgumentOutOfRangeException(paramName, Messages.ArgumentOutOfRangeException_NotPositive);
            }

            return parameter;
        }

        /// <summary>Throws an ArgumentException if the parameter is not positive, otherwise the parameter is passed.</summary>
        /// <param name="parameter">The parameter to guard.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>
        /// The guarded parameter.
        /// </returns>
        [DebuggerStepThrough]
        public static decimal Positive(decimal? parameter, string paramName) => Positive(HasValue(parameter, paramName), paramName);

        /// <summary>Throws an ArgumentException if the parameter is not positive, otherwise the parameter is passed.</summary>
        /// <param name="parameter">The parameter to guard.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>
        /// The guarded parameter.
        /// </returns>
        [DebuggerStepThrough]
        public static decimal Positive(decimal parameter, string paramName)
        {
            if (parameter <= 0)
            {
                throw new ArgumentOutOfRangeException(paramName, Messages.ArgumentOutOfRangeException_NotPositive);
            }

            return parameter;
        }

        /// <summary>Throws an ArgumentException if the parameter is not positive, otherwise the parameter is passed.</summary>
        /// <param name="parameter">The parameter to guard.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>
        /// The guarded parameter.
        /// </returns>
        [DebuggerStepThrough]
        public static TimeSpan Positive(TimeSpan? parameter, string paramName) => Positive(HasValue(parameter, paramName), paramName);

        /// <summary>Throws an ArgumentException if the parameter is not positive, otherwise the parameter is passed.</summary>
        /// <param name="parameter">The parameter to guard.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>
        /// The guarded parameter.
        /// </returns>
        [DebuggerStepThrough]
        public static TimeSpan Positive(TimeSpan parameter, string paramName)
        {
            if (parameter <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(paramName, Messages.ArgumentOutOfRangeException_NotPositive);
            }

            return parameter;
        }

        /// <summary>Throws an ArgumentException if the parameter is negative, otherwise the parameter is passed.</summary>
        /// <param name="parameter">The parameter to guard.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>
        /// The guarded parameter.
        /// </returns>
        [DebuggerStepThrough]
        public static int NotNegative(int? parameter, string paramName) => NotNegative(HasValue(parameter, paramName), paramName);

        /// <summary>Throws an ArgumentException if the parameter is negative, otherwise the parameter is passed.</summary>
        /// <param name="parameter">The parameter to guard.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>
        /// The guarded parameter.
        /// </returns>
        [DebuggerStepThrough]
        public static int NotNegative(int parameter, string paramName)
        {
            if (parameter < 0)
            {
                throw new ArgumentOutOfRangeException(paramName, Messages.ArgumentOutOfRangeException_Negative);
            }

            return parameter;
        }

        /// <summary>Throws an ArgumentException if the parameter is negative, otherwise the parameter is passed.</summary>
        /// <param name="parameter">The parameter to guard.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>
        /// The guarded parameter.
        /// </returns>
        [DebuggerStepThrough]
        public static long NotNegative(long? parameter, string paramName) => NotNegative(HasValue(parameter, paramName), paramName);

        /// <summary>Throws an ArgumentException if the parameter is negative, otherwise the parameter is passed.</summary>
        /// <param name="parameter">The parameter to guard.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>
        /// The guarded parameter.
        /// </returns>
        [DebuggerStepThrough]
        public static long NotNegative(long parameter, string paramName)
        {
            if (parameter < 0)
            {
                throw new ArgumentOutOfRangeException(paramName, Messages.ArgumentOutOfRangeException_Negative);
            }

            return parameter;
        }

        /// <summary>Throws an ArgumentException if the parameter is negative, otherwise the parameter is passed.</summary>
        /// <param name="parameter">The parameter to guard.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>
        /// The guarded parameter.
        /// </returns>
        [DebuggerStepThrough]
        public static double NotNegative(double? parameter, string paramName) => NotNegative(HasValue(parameter, paramName), paramName);

        /// <summary>Throws an ArgumentException if the parameter is negative, otherwise the parameter is passed.</summary>
        /// <param name="parameter">The parameter to guard.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>
        /// The guarded parameter.
        /// </returns>
        [DebuggerStepThrough]
        public static double NotNegative(double parameter, string paramName)
        {
            if (parameter < 0)
            {
                throw new ArgumentOutOfRangeException(paramName, Messages.ArgumentOutOfRangeException_Negative);
            }

            return parameter;
        }

        /// <summary>Throws an ArgumentException if the parameter is negative, otherwise the parameter is passed.</summary>
        /// <param name="parameter">The parameter to guard.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>
        /// The guarded parameter.
        /// </returns>
        [DebuggerStepThrough]
        public static decimal NotNegative(decimal? parameter, string paramName) => NotNegative(HasValue(parameter, paramName), paramName);

        /// <summary>Throws an ArgumentException if the parameter is negative, otherwise the parameter is passed.</summary>
        /// <param name="parameter">The parameter to guard.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>
        /// The guarded parameter.
        /// </returns>
        [DebuggerStepThrough]
        public static decimal NotNegative(decimal parameter, string paramName)
        {
            if (parameter < 0)
            {
                throw new ArgumentOutOfRangeException(paramName, Messages.ArgumentOutOfRangeException_Negative);
            }

            return parameter;
        }

        /// <summary>Throws an ArgumentException if the parameter is negative, otherwise the parameter is passed.</summary>
        /// <param name="parameter">The parameter to guard.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>
        /// The guarded parameter.
        /// </returns>
        [DebuggerStepThrough]
        public static TimeSpan NotNegative(TimeSpan? parameter, string paramName) => NotNegative(HasValue(parameter, paramName), paramName);

        /// <summary>Throws an ArgumentException if the parameter is negative, otherwise the parameter is passed.</summary>
        /// <param name="parameter">The parameter to guard.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>
        /// The guarded parameter.
        /// </returns>
        [DebuggerStepThrough]
        public static TimeSpan NotNegative(TimeSpan parameter, string paramName)
        {
            if (parameter < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(paramName, Messages.ArgumentOutOfRangeException_Negative);
            }

            return parameter;
        }

        /// <summary>Messages class to group the constants.</summary>
        private static class Messages
        {
#pragma warning disable SA1310 // Field names should not contain underscore

            public const string ArgumentException_EmptyCollection = "Argument cannot be an empty collection.";
            public const string ArgumentException_GuidEmpty = "Argument cannot be an empty GUID.";
            public const string ArgumentException_IsDefaultValue = "Argument is the not initialized/default value.";
            public const string ArgumentException_NotAnInstanceOf = "Argument is not an instance of {0}.";
            public const string ArgumentException_NullableMustHaveValue = "Nullable argument must have a value.";
            public const string ArgumentException_StringEmpty = "Argument cannot be an empty string.";

            public const string ArgumentOutOfRangeException_InCollection = "Argument was in the collection of forbidden values. Forbidden are {0}.";
            public const string ArgumentOutOfRangeException_NotInCollection = "Argument was not in the collection of allowed values. Allowed are {0}.";

            public const string ArgumentOutOfRangeException_DefinedEnum = "Argument {0} is not a defined value of {1}.";

            public const string ArgumentOutOfRangeException_Negative = "Argument should not be negative.";
            public const string ArgumentOutOfRangeException_NotPositive = "Argument should be positive.";

#pragma warning restore SA1310 // Field names should not contain underscore
        }

        /// <summary>Marks the NotNull argument as being validated for not being null, to satisfy the static code analysis.</summary>
        /// <remarks>
        /// Notice that it does not matter what this attribute does, as long as
        /// it is named ValidatedNotNullAttribute.
        ///
        /// It is marked as conditional, as does not add anything to have the attribute compiled.
        /// </remarks>
        [Conditional("Analysis")]
        [AttributeUsage(AttributeTargets.Parameter)]
        private sealed class ValidatedNotNullAttribute : Attribute { }
    }
}
