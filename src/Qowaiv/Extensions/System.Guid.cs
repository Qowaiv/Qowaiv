using Qowaiv;
using Qowaiv.Identifiers;

namespace System;

/// <summary>Extensions on <see cref="Guid"/>.</summary>
public static class QowaivGuidExtensions
{
    /// <summary>Gets the version of the <see cref="Guid"/>.</summary>
    [Pure]
    public static UuidVersion GetVersion(this Guid guid) => GuidLayout.From(guid).Version;
}
