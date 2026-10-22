using Qowaiv;
using Qowaiv.Identifiers;

namespace System;

/// <summary>Extensions on <see cref="Guid" />.</summary>
public static class QowaivGuidExtensions
{
    extension(Guid guid)
    {
        /// <summary>Gets the version of the <see cref="Guid" />.</summary>
        [Pure]
        public UuidVersion GetVersion() => GuidLayout.From(guid).Version;
    }
}
