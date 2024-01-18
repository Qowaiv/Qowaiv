using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Qowaiv.Identifiers;

[StructLayout(LayoutKind.Explicit)]
internal struct GuidLayout
{
    // This will store the result of the parsing. And it will eventually be used to construct a Guid instance.
    // We'll eventually reinterpret_cast<> a GuidLayout as a Guid, so we need to give it a sequential
    // layout and ensure that its early fields match the layout of Guid exactly.
    [FieldOffset(0)]
    internal uint _a;
    [FieldOffset(4)]
    internal uint _bc;
    [FieldOffset(4)]
    internal ushort _b;
    [FieldOffset(6)]
    internal ushort _c;
    [FieldOffset(8)]
    internal uint _defg;
    [FieldOffset(8)]
    internal ushort _de;
    [FieldOffset(8)]
    internal byte _d;
    [FieldOffset(10)]
    internal ushort _fg;
    [FieldOffset(12)]
    internal uint _hijk;

    [FieldOffset(7)]
    internal byte _version;

    /// <summary>Gets the upper 4 bits of byte 7 that holds the version.</summary>
    public readonly UuidVersion Version => (UuidVersion)(_version >> 4);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Guid ToGuid() => Unsafe.As<GuidLayout, Guid>(ref Unsafe.AsRef(in this));

    [Pure]
    public static GuidLayout From(Guid value) => Unsafe.As<Guid, GuidLayout>(ref Unsafe.AsRef(in value));
}
