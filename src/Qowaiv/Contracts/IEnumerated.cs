namespace Qowaiv;

/// <summary>Indicates that the SVO has a limited well defined set of values.</summary>
public interface IEnumerated;

/// <summary>Indicates that the SVO has a limited well defined set of values.</summary>
/// <typeparam name="TSelf">
/// The type of the SVO.
/// </typeparam>
public interface IEnumerated<TSelf> : IEnumerated where TSelf : struct, IEnumerated<TSelf>
{
#if !NETSTANDARD2_0
    /// <summary>Gets all defined values.</summary>
    /// <remarks>
    /// Only values that carry domain meaning are defined values. The empty value
    /// (<see cref="IEmpty{TSelf}"/>) and the unknown value (<see cref="IUnknown{TSelf}"/>)
    /// are not, as both express the absence of information and are, to a large
    /// extent, equivalent to each other.
    /// </remarks>
    static abstract IReadOnlyCollection<TSelf> DefinedValues { get; }
#endif
}
