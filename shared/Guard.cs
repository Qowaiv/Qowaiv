//-----------------------------------------------------------------------
// <copyright>
//    Copyright (c) Corniel Nobel. All rights reserved.
//    See: https://github.com/Corniel/Grenadiers/blob/master/LICENSE.md
// </copyright>
//-----------------------------------------------------------------------
#nullable enable

using System.IO;
using System.Runtime.CompilerServices;

namespace Qowaiv;

/// <summary>Supplies parameter guarding for methods and constructors.</summary>
/// <remarks>
/// Advised usage:
/// * Change the namespace to maximum shared namespace amongst the using projects
/// * Keep it internal and use [assembly: InternalsVisibleTo] to open up access
/// * Add specific Guard methods if you software needs them.
/// * Keep the checks cheap so that you also can run them in production code.
/// </remarks>
[ExcludeFromCodeCoverage]
[StackTraceHidden]
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
    public static T NotNull<T>([ValidatedNotNull] T? parameter, [CallerArgumentExpression(nameof(parameter))] string? paramName = null)
        where T : class
        => parameter ?? throw new ArgumentNullException(paramName);

    /// <summary>Throws an ArgumentException if the nullable parameter has no value, otherwise the parameter value is passed.</summary>
    /// <typeparam name="T">The type to guard; must be a structure.</typeparam>
    /// <param name="parameter">The parameter to guard.</param>
    /// <param name="paramName">The name of the parameter.</param>
    /// <returns>
    /// The guarded parameter.
    /// </returns>
    [DebuggerStepThrough]
    public static T HasValue<T>(T? parameter, [CallerArgumentExpression(nameof(parameter))] string? paramName = null)
        where T : struct
        => parameter ?? throw new ArgumentException(Messages.ArgumentException_NullableMustHaveValue, paramName);

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
    public static T NotDefault<T>(T? parameter, [CallerArgumentExpression(nameof(parameter))] string? paramName = null)
        where T : struct => NotDefault(HasValue(parameter, paramName), paramName);

    /// <summary>Throws an ArgumentException if the parameter has the default value, otherwise the parameter value is passed.</summary>
    /// <typeparam name="T">The type to guard; must be a structure.</typeparam>
    /// <param name="parameter">The parameter to guard.</param>
    /// <param name="paramName">The name of the parameter.</param>
    /// <returns>
    /// The guarded parameter.
    /// </returns>
    [DebuggerStepThrough]
    public static T NotDefault<T>(T parameter, [CallerArgumentExpression(nameof(parameter))] string? paramName = null)
        where T : struct
        => parameter.Equals(default(T))
        ? throw new ArgumentException(Messages.ArgumentException_IsDefaultValue, paramName)
        : parameter;

    /// <summary>Throws an <see cref="ArgumentOutOfRangeException" /> if the parameter not in not a defined value of the enum, otherwise the parameter is passed.</summary>
    /// <typeparam name="T">The type to guard; must be a structure (enum).</typeparam>
    /// <param name="parameter">The parameter to guard.</param>
    /// <param name="paramName">The name of the parameter.</param>
    /// <returns>
    /// The guarded parameter.
    /// </returns>
    /// <remarks>
    /// That <typeparamref name="T" /> is an enum is implicitly guard by <see cref="Enum.IsDefined(Type, object)" />.
    /// </remarks>
    [DebuggerStepThrough]
    public static T DefinedEnum<T>(T parameter, [CallerArgumentExpression(nameof(parameter))] string? paramName = null)
        where T : struct
        => Enum.IsDefined(typeof(T), parameter)
        ? parameter
        : throw new ArgumentOutOfRangeException(paramName, string.Format(Messages.ArgumentOutOfRangeException_DefinedEnum, parameter, typeof(T)));

    /// <summary>
    /// Guards that the parameter is an instance of T, otherwise throws an argument (null) exception.
    /// It only makes sense to use this function if the original type of the <paramref name="parameter" />
    /// is <see cref="object" />, otherwise one should simply use <see cref="NotNull{T}(T, string)" />.
    /// </summary>
    /// <typeparam name="T">The type to guard.</typeparam>
    /// <param name="parameter">The parameter to guard.</param>
    /// <param name="paramName">The name of the parameter.</param>
    /// <returns>
    /// The guarded parameter.
    /// </returns>
    [DebuggerStepThrough]
    public static T IsInstanceOf<T>([ValidatedNotNull] object? parameter, [CallerArgumentExpression(nameof(parameter))] string? paramName = null)
        => NotNull(parameter, paramName) is T guarded
        ? guarded
        : throw new ArgumentException(string.Format(Messages.ArgumentException_NotAnInstanceOf, typeof(T)), paramName);

    /// <summary>Guards that the parameter is not null or an empty collection, otherwise throws an argument (null) exception.</summary>
    /// <typeparam name="T">The type to guard; must be an <see cref="ICollection" />.</typeparam>
    /// <param name="parameter">The parameter to guard.</param>
    /// <param name="paramName">The name of the parameter.</param>
    /// <returns>
    /// The guarded parameter.
    /// </returns>
    [DebuggerStepThrough]
    public static T HasAny<T>([ValidatedNotNull] T? parameter, [CallerArgumentExpression(nameof(parameter))] string? paramName = null)
        where T : class, ICollection
        => NotNull(parameter, paramName) is var guarded && guarded.Count == 0
        ? throw new ArgumentException(Messages.ArgumentException_EmptyCollection, paramName)
        : guarded;

    /// <summary>Guards that the parameter is not null or an empty enumerable, otherwise throws an argument (null) exception.</summary>
    /// <typeparam name="T">The type to guard; must be an <see cref="IEnumerable" />.</typeparam>
    /// <param name="parameter">The parameter to guard.</param>
    /// <param name="paramName">The name of the parameter.</param>
    /// <returns>
    /// The guarded parameter.
    /// </returns>
    [DebuggerStepThrough]
    public static IEnumerable<T> HasAny<T>([ValidatedNotNull] IEnumerable<T>? parameter, [CallerArgumentExpression(nameof(parameter))] string? paramName = null)
        => NotNull(parameter, paramName) is var guarded && guarded.Any()
        ? guarded
        : throw new ArgumentException(Messages.ArgumentException_EmptyCollection, paramName);

    /// <summary>Guards that the parameter is not null or an empty string, otherwise throws an argument (null) exception.</summary>
    /// <param name="parameter">The parameter to guard.</param>
    /// <param name="paramName">The name of the parameter.</param>
    /// <returns>
    /// The guarded parameter.
    /// </returns>
    [DebuggerStepThrough]
    public static string NotNullOrEmpty([ValidatedNotNull] string? parameter, [CallerArgumentExpression(nameof(parameter))] string? paramName = null)
        => NotNull(parameter, paramName) is { Length: > 0 } guarded
        ? guarded
        : throw new ArgumentException(Messages.ArgumentException_StringEmpty, paramName);

    /// <summary>Guards that the parameter is not an empty <see cref="Guid" />, otherwise throws an argument exception.</summary>
    /// <param name="parameter">The parameter to guard.</param>
    /// <param name="paramName">The name of the parameter.</param>
    /// <returns>
    /// The guarded parameter.
    /// </returns>
    [DebuggerStepThrough]
    public static Guid NotEmpty(Guid? parameter, [CallerArgumentExpression(nameof(parameter))] string? paramName = null)
        => NotEmpty(HasValue(parameter, paramName), paramName);

    /// <summary>Guards that the parameter is not an empty <see cref="Guid" />, otherwise throws an argument exception.</summary>
    /// <param name="parameter">The parameter to guard.</param>
    /// <param name="paramName">The name of the parameter.</param>
    /// <returns>
    /// The guarded parameter.
    /// </returns>
    [DebuggerStepThrough]
    public static Guid NotEmpty(Guid parameter, [CallerArgumentExpression(nameof(parameter))] string? paramName = null)
        => parameter == Guid.Empty
        ? throw new ArgumentException(Messages.ArgumentException_GuidEmpty, paramName)
        : parameter;

    /// <summary>Throws an ArgumentException if the parameter is not positive, otherwise the parameter is passed.</summary>
    /// <param name="parameter">The parameter to guard.</param>
    /// <param name="paramName">The name of the parameter.</param>
    /// <returns>
    /// The guarded parameter.
    /// </returns>
    [DebuggerStepThrough]
    public static int Positive(int? parameter, [CallerArgumentExpression(nameof(parameter))] string? paramName = null)
        => Positive(HasValue(parameter, paramName), paramName);

    /// <summary>Throws an ArgumentOutOfRangeException if the parameter is not positive, otherwise the parameter is passed.</summary>
    /// <param name="parameter">The parameter to guard.</param>
    /// <param name="paramName">The name of the parameter.</param>
    /// <returns>
    /// The guarded parameter.
    /// </returns>
    [DebuggerStepThrough]
    public static int Positive(int parameter, [CallerArgumentExpression(nameof(parameter))] string? paramName = null)
        => parameter <= 0
        ? throw new ArgumentOutOfRangeException(paramName, Messages.ArgumentOutOfRangeException_NotPositive)
        : parameter;

    /// <summary>Throws an ArgumentOutOfRangeException if the parameter is not positive, otherwise the parameter is passed.</summary>
    /// <param name="parameter">The parameter to guard.</param>
    /// <param name="paramName">The name of the parameter.</param>
    /// <returns>
    /// The guarded parameter.
    /// </returns>
    [DebuggerStepThrough]
    public static long Positive(long? parameter, [CallerArgumentExpression(nameof(parameter))] string? paramName = null) => Positive(HasValue(parameter, paramName), paramName);

    /// <summary>Throws an ArgumentOutOfRangeException if the parameter is not positive, otherwise the parameter is passed.</summary>
    /// <param name="parameter">The parameter to guard.</param>
    /// <param name="paramName">The name of the parameter.</param>
    /// <returns>
    /// The guarded parameter.
    /// </returns>
    [DebuggerStepThrough]
    public static long Positive(long parameter, [CallerArgumentExpression(nameof(parameter))] string? paramName = null)
        => parameter <= 0
        ? throw new ArgumentOutOfRangeException(paramName, Messages.ArgumentOutOfRangeException_NotPositive)
        : parameter;

    /// <summary>Throws an ArgumentOutOfRangeException if the parameter is not positive, otherwise the parameter is passed.</summary>
    /// <param name="parameter">The parameter to guard.</param>
    /// <param name="paramName">The name of the parameter.</param>
    /// <returns>
    /// The guarded parameter.
    /// </returns>
    [DebuggerStepThrough]
    public static double Positive(double? parameter, [CallerArgumentExpression(nameof(parameter))] string? paramName = null)
        => Positive(HasValue(parameter, paramName), paramName);

    /// <summary>Throws an ArgumentOutOfRangeException if the parameter is not positive, otherwise the parameter is passed.</summary>
    /// <param name="parameter">The parameter to guard.</param>
    /// <param name="paramName">The name of the parameter.</param>
    /// <returns>
    /// The guarded parameter.
    /// </returns>
    [DebuggerStepThrough]
    public static double Positive(double parameter, [CallerArgumentExpression(nameof(parameter))] string? paramName = null)
        => parameter <= 0
        ? throw new ArgumentOutOfRangeException(paramName, Messages.ArgumentOutOfRangeException_NotPositive)
        : parameter;

    /// <summary>Throws an ArgumentOutOfRangeException if the parameter is not positive, otherwise the parameter is passed.</summary>
    /// <param name="parameter">The parameter to guard.</param>
    /// <param name="paramName">The name of the parameter.</param>
    /// <returns>
    /// The guarded parameter.
    /// </returns>
    [DebuggerStepThrough]
    public static decimal Positive(decimal? parameter, [CallerArgumentExpression(nameof(parameter))] string? paramName = null)
        => Positive(HasValue(parameter, paramName), paramName);

    /// <summary>Throws an ArgumentOutOfRangeException if the parameter is not positive, otherwise the parameter is passed.</summary>
    /// <param name="parameter">The parameter to guard.</param>
    /// <param name="paramName">The name of the parameter.</param>
    /// <returns>
    /// The guarded parameter.
    /// </returns>
    [DebuggerStepThrough]
    public static decimal Positive(decimal parameter, [CallerArgumentExpression(nameof(parameter))] string? paramName = null)
        => parameter <= 0
        ? throw new ArgumentOutOfRangeException(paramName, Messages.ArgumentOutOfRangeException_NotPositive)
        : parameter;

    /// <summary>Throws an ArgumentOutOfRangeException if the parameter is not positive, otherwise the parameter is passed.</summary>
    /// <param name="parameter">The parameter to guard.</param>
    /// <param name="paramName">The name of the parameter.</param>
    /// <returns>
    /// The guarded parameter.
    /// </returns>
    [DebuggerStepThrough]
    public static TimeSpan Positive(TimeSpan? parameter, [CallerArgumentExpression(nameof(parameter))] string? paramName = null)
        => Positive(HasValue(parameter, paramName), paramName);

    /// <summary>Throws an ArgumentOutOfRangeException if the parameter is not positive, otherwise the parameter is passed.</summary>
    /// <param name="parameter">The parameter to guard.</param>
    /// <param name="paramName">The name of the parameter.</param>
    /// <returns>
    /// The guarded parameter.
    /// </returns>
    [DebuggerStepThrough]
    public static TimeSpan Positive(TimeSpan parameter, [CallerArgumentExpression(nameof(parameter))] string? paramName = null)
        => parameter <= TimeSpan.Zero
        ? throw new ArgumentOutOfRangeException(paramName, Messages.ArgumentOutOfRangeException_NotPositive)
        : parameter;

    /// <summary>Throws an ArgumentException if the parameter is negative, otherwise the parameter is passed.</summary>
    /// <param name="parameter">The parameter to guard.</param>
    /// <param name="paramName">The name of the parameter.</param>
    /// <returns>
    /// The guarded parameter.
    /// </returns>
    [DebuggerStepThrough]
    public static int NotNegative(int? parameter, [CallerArgumentExpression(nameof(parameter))] string? paramName = null)
        => NotNegative(HasValue(parameter, paramName), paramName);

    /// <summary>Throws an ArgumentException if the parameter is negative, otherwise the parameter is passed.</summary>
    /// <param name="parameter">The parameter to guard.</param>
    /// <param name="paramName">The name of the parameter.</param>
    /// <returns>
    /// The guarded parameter.
    /// </returns>
    [DebuggerStepThrough]
    public static int NotNegative(int parameter, [CallerArgumentExpression(nameof(parameter))] string? paramName = null)
        => parameter < 0
        ? throw new ArgumentOutOfRangeException(paramName, Messages.ArgumentOutOfRangeException_Negative)
        : parameter;

    /// <summary>Throws an ArgumentException if the parameter is negative, otherwise the parameter is passed.</summary>
    /// <param name="parameter">The parameter to guard.</param>
    /// <param name="paramName">The name of the parameter.</param>
    /// <returns>
    /// The guarded parameter.
    /// </returns>
    [DebuggerStepThrough]
    public static long NotNegative(long? parameter, [CallerArgumentExpression(nameof(parameter))] string? paramName = null)
        => NotNegative(HasValue(parameter, paramName), paramName);

    /// <summary>Throws an ArgumentException if the parameter is negative, otherwise the parameter is passed.</summary>
    /// <param name="parameter">The parameter to guard.</param>
    /// <param name="paramName">The name of the parameter.</param>
    /// <returns>
    /// The guarded parameter.
    /// </returns>
    [DebuggerStepThrough]
    public static long NotNegative(long parameter, [CallerArgumentExpression(nameof(parameter))] string? paramName = null)
        => parameter < 0
        ? throw new ArgumentOutOfRangeException(paramName, Messages.ArgumentOutOfRangeException_Negative)
        : parameter;

    /// <summary>Throws an ArgumentException if the parameter is negative, otherwise the parameter is passed.</summary>
    /// <param name="parameter">The parameter to guard.</param>
    /// <param name="paramName">The name of the parameter.</param>
    /// <returns>
    /// The guarded parameter.
    /// </returns>
    [DebuggerStepThrough]
    public static double NotNegative(double? parameter, [CallerArgumentExpression(nameof(parameter))] string? paramName = null)
        => NotNegative(HasValue(parameter, paramName), paramName);

    /// <summary>Throws an ArgumentException if the parameter is negative, otherwise the parameter is passed.</summary>
    /// <param name="parameter">The parameter to guard.</param>
    /// <param name="paramName">The name of the parameter.</param>
    /// <returns>
    /// The guarded parameter.
    /// </returns>
    [DebuggerStepThrough]
    public static double NotNegative(double parameter, [CallerArgumentExpression(nameof(parameter))] string? paramName = null)
        => parameter < 0
        ? throw new ArgumentOutOfRangeException(paramName, Messages.ArgumentOutOfRangeException_Negative)
        : parameter;

    /// <summary>Throws an ArgumentException if the parameter is negative, otherwise the parameter is passed.</summary>
    /// <param name="parameter">The parameter to guard.</param>
    /// <param name="paramName">The name of the parameter.</param>
    /// <returns>
    /// The guarded parameter.
    /// </returns>
    [DebuggerStepThrough]
    public static decimal NotNegative(decimal? parameter, [CallerArgumentExpression(nameof(parameter))] string? paramName = null)
        => NotNegative(HasValue(parameter, paramName), paramName);

    /// <summary>Throws an ArgumentException if the parameter is negative, otherwise the parameter is passed.</summary>
    /// <param name="parameter">The parameter to guard.</param>
    /// <param name="paramName">The name of the parameter.</param>
    /// <returns>
    /// The guarded parameter.
    /// </returns>
    [DebuggerStepThrough]
    public static decimal NotNegative(decimal parameter, [CallerArgumentExpression(nameof(parameter))] string? paramName = null)
        => parameter < 0
        ? throw new ArgumentOutOfRangeException(paramName, Messages.ArgumentOutOfRangeException_Negative)
        : parameter;

    /// <summary>Throws an ArgumentException if the parameter is negative, otherwise the parameter is passed.</summary>
    /// <param name="parameter">The parameter to guard.</param>
    /// <param name="paramName">The name of the parameter.</param>
    /// <returns>
    /// The guarded parameter.
    /// </returns>
    [DebuggerStepThrough]
    public static TimeSpan NotNegative(TimeSpan? parameter, [CallerArgumentExpression(nameof(parameter))] string? paramName = null)
        => NotNegative(HasValue(parameter, paramName), paramName);

    /// <summary>Throws an ArgumentException if the parameter is negative, otherwise the parameter is passed.</summary>
    /// <param name="parameter">The parameter to guard.</param>
    /// <param name="paramName">The name of the parameter.</param>
    /// <returns>
    /// The guarded parameter.
    /// </returns>
    [DebuggerStepThrough]
    public static TimeSpan NotNegative(TimeSpan parameter, [CallerArgumentExpression(nameof(parameter))] string? paramName = null)
        => parameter < TimeSpan.Zero
        ? throw new ArgumentOutOfRangeException(paramName, Messages.ArgumentOutOfRangeException_Negative)
        : parameter;

    /// <summary>Throws an ArgumentException if the parameter is negative, otherwise the parameter is passed.</summary>
    /// <param name="parameter">The parameter to guard.</param>
    /// <param name="paramName">The name of the parameter.</param>
    /// <returns>
    /// The guarded parameter.
    /// </returns>
    [DebuggerStepThrough]
    public static FileInfo Exists([ValidatedNotNull] FileInfo? parameter, [CallerArgumentExpression(nameof(parameter))] string? paramName = null)
        => NotNull(parameter).Exists
        ? parameter!
        : throw new ArgumentException(string.Format(Messages.ArgumentException_NotExists, parameter!.FullName), paramName);

    /// <summary>Messages class to group the constants.</summary>
    private static class Messages
    {
        public const string ArgumentException_EmptyCollection = "Argument cannot be an empty collection.";
        public const string ArgumentException_GuidEmpty = "Argument cannot be an empty GUID.";
        public const string ArgumentException_IsDefaultValue = "Argument is the not initialized/default value.";
        public const string ArgumentException_NotAnInstanceOf = "Argument is not an instance of {0}.";
        public const string ArgumentException_NotExists = "Argument '{0}'does not exist.";
        public const string ArgumentException_NullableMustHaveValue = "Nullable argument must have a value.";
        public const string ArgumentException_StringEmpty = "Argument cannot be an empty string.";
        public const string ArgumentOutOfRangeException_DefinedEnum = "Argument {0} is not a defined value of {1}.";
        public const string ArgumentOutOfRangeException_Negative = "Argument should not be negative.";
        public const string ArgumentOutOfRangeException_NotPositive = "Argument should be positive.";
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
