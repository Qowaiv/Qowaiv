using Qowaiv;
using Qowaiv.Identifiers;

namespace Qowaiv.Extensions;

/// <summary>Extensions on <see cref="Guid" />.</summary>
public static class QowaivGuid
{
    extension(Guid guid)
    {
        /// <summary>Gets the version of the <see cref="Guid" />.</summary>
        [Pure]
        public UuidVersion UuidVersion => GuidLayout.From(guid).Version;
    }
}
