#if !NETSTANDARD2_0

namespace Qowaiv;

/// <summary>Indicates that the SVO as a limited well defined set of values.</summary>
/// <typeparam name="TSelf">
/// The type of the SVO.
/// </typeparam>
public interface IEnumerated<TSelf> where TSelf : struct, IEnumerated<TSelf>
{
    /// <summary>Gets all defined values.</summary>
    /// <remarks>
    /// For <see cref="IEmpty{TSelf}"/> the empty value is not considered defined.
    /// For <see cref="IUnknown{TSelf}"/> the unknown value is not considered defined.
    /// </remarks>
    static abstract IReadOnlyCollection<TSelf> DefinedValues { get; }
}

#endif
