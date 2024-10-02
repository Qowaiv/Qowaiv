using System.Runtime.CompilerServices;

namespace Qowaiv;

internal static partial class Guard
{
    /// <summary>Guards the parameter implements <see cref="IFormattable" />,
    /// otherwise throws an argument (null) exception.
    /// </summary>
    /// <param name="parameter">
    /// The parameter to guard.
    /// </param>
    /// <param name="paramName">
    /// The name of the parameter.
    /// </param>
    [DebuggerStepThrough]
    public static Type ImplementsIFormattable(Type parameter, [CallerArgumentExpression(nameof(parameter))] string? paramName = null)
        => NotNull(parameter, paramName).GetInterfaces().Contains(typeof(IFormattable))
        ? parameter
        : throw new ArgumentException(QowaivMessages.ArgumentException_NotIFormattable, paramName);
}
