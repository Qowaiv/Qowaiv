using System.Diagnostics.Contracts;

namespace Qowaiv.IO
{
    /// <summary>Extensions to create <see cref="IO.StreamSize"/>s, inspired by Humanizer.NET.</summary>
    public static class  NumberToStreamSizeExtensions
    {
        /// <summary>Creates a <see cref="StreamSize"/> as bytes.</summary>
        [Pure]
        public static StreamSize Bytes(this int bytes) => new(bytes);

        /// <summary>Creates a <see cref="StreamSize"/> as bytes.</summary>
        [Pure]
        public static StreamSize Bytes(this long bytes) => new(bytes);

        /// <summary>Creates a <see cref="StreamSize"/> as kilobytes.</summary>
        [Pure]
        public static StreamSize KB(this int kb) => StreamSize.FromKilobytes(kb);

        /// <summary>Creates a <see cref="StreamSize"/> as kilobytes.</summary>
        [Pure]
        public static StreamSize KB(this long kb) => StreamSize.FromKilobytes(kb);

        /// <summary>Creates a <see cref="StreamSize"/> as kilobytes.</summary>
        [Pure]
        public static StreamSize KB(this double kb) => StreamSize.FromKilobytes(kb);

        /// <summary>Creates a <see cref="StreamSize"/> as megabytes.</summary>
        [Pure]
        public static StreamSize MB(this int mb) => StreamSize.FromMegabytes(mb);

        /// <summary>Creates a <see cref="StreamSize"/> as megabytes.</summary>
        [Pure]
        public static StreamSize MB(this long mb) => StreamSize.FromMegabytes(mb);

        /// <summary>Creates a <see cref="StreamSize"/> as megabytes.</summary>
        [Pure]
        public static StreamSize MB(this double mb) => StreamSize.FromMegabytes(mb);

        /// <summary>Creates a <see cref="StreamSize"/> as gigabytes.</summary>
        [Pure]
        public static StreamSize GB(this int mb) => StreamSize.FromGigabytes(mb);

        /// <summary>Creates a <see cref="StreamSize"/> as gigabytes.</summary>
        [Pure]
        public static StreamSize GB(this long mb) => StreamSize.FromGigabytes(mb);

        /// <summary>Creates a <see cref="StreamSize"/> as gigabytes.</summary>
        [Pure]
        public static StreamSize GB(this double mb) => StreamSize.FromGigabytes(mb);

        /// <summary>Creates a <see cref="StreamSize"/> as kibibytes.</summary>
        [Pure]
        public static StreamSize KiB(this int kib) => StreamSize.FromKibibytes(kib);

        /// <summary>Creates a <see cref="StreamSize"/> as kibibytes.</summary>
        [Pure]
        public static StreamSize KiB(this long kib) => StreamSize.FromKibibytes(kib);

        /// <summary>Creates a <see cref="StreamSize"/> as kibibytes.</summary>
        [Pure]
        public static StreamSize KiB(this double kib) => StreamSize.FromKibibytes(kib);

        /// <summary>Creates a <see cref="StreamSize"/> as mebibytes.</summary>
        [Pure]
        public static StreamSize MiB(this int mib) => StreamSize.FromMebibytes(mib);

        /// <summary>Creates a <see cref="StreamSize"/> as mebibytes.</summary>
        [Pure]
        public static StreamSize MiB(this long mib) => StreamSize.FromMebibytes(mib);

        /// <summary>Creates a <see cref="StreamSize"/> as mebibytes.</summary>
        [Pure]
        public static StreamSize MiB(this double mib) => StreamSize.FromMebibytes(mib);

        /// <summary>Creates a <see cref="StreamSize"/> as gibibytes.</summary>
        [Pure]
        public static StreamSize GiB(this int gib) => StreamSize.FromGibibytes(gib);

        /// <summary>Creates a <see cref="StreamSize"/> as gibibytes.</summary>
        [Pure]
        public static StreamSize GiB(this long gib) => StreamSize.FromGibibytes(gib);

        /// <summary>Creates a <see cref="StreamSize"/> as gibibytes.</summary>
        [Pure]
        public static StreamSize GiB(this double gib) => StreamSize.FromGibibytes(gib);
    }
}
