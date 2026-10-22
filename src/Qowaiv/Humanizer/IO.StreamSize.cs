using Qowaiv.IO;

namespace Qowaiv.Humanizer;

public static partial class Humanize
{
    extension(int bytes)
    {
        /// <summary>Creates a <see cref="StreamSize" /> as bytes.</summary>
        [Pure]
        public StreamSize Bytes() => new(bytes);
    }

    extension(long bytes)
    {
        /// <summary>Creates a <see cref="StreamSize" /> as bytes.</summary>
        [Pure]
        public StreamSize Bytes() => new(bytes);
    }

    extension(int kb)
    {
        /// <summary>Creates a <see cref="StreamSize" /> as kilobytes.</summary>
        [Pure]
        public StreamSize KB() => StreamSize.FromKilobytes(kb);
    }

    extension(long kb)
    {
        /// <summary>Creates a <see cref="StreamSize" /> as kilobytes.</summary>
        [Pure]
        public StreamSize KB() => StreamSize.FromKilobytes(kb);
    }

    extension(double kb)
    {
        /// <summary>Creates a <see cref="StreamSize" /> as kilobytes.</summary>
        [Pure]
        public StreamSize KB() => StreamSize.FromKilobytes(kb);
    }

    extension(int mb)
    {
        /// <summary>Creates a <see cref="StreamSize" /> as megabytes.</summary>
        [Pure]
        public StreamSize MB() => StreamSize.FromMegabytes(mb);

        /// <summary>Creates a <see cref="StreamSize" /> as gigabytes.</summary>
        [Pure]
        public StreamSize GB() => StreamSize.FromGigabytes(mb);
    }

    extension(long mb)
    {
        /// <summary>Creates a <see cref="StreamSize" /> as megabytes.</summary>
        [Pure]
        public StreamSize MB() => StreamSize.FromMegabytes(mb);

        /// <summary>Creates a <see cref="StreamSize" /> as gigabytes.</summary>
        [Pure]
        public StreamSize GB() => StreamSize.FromGigabytes(mb);
    }

    extension(double mb)
    {
        /// <summary>Creates a <see cref="StreamSize" /> as megabytes.</summary>
        [Pure]
        public StreamSize MB() => StreamSize.FromMegabytes(mb);

        /// <summary>Creates a <see cref="StreamSize" /> as gigabytes.</summary>
        [Pure]
        public StreamSize GB() => StreamSize.FromGigabytes(mb);
    }

    extension(int kib)
    {
        /// <summary>Creates a <see cref="StreamSize" /> as kibibytes.</summary>
        [Pure]
        public StreamSize KiB() => StreamSize.FromKibibytes(kib);
    }

    extension(long kib)
    {
        /// <summary>Creates a <see cref="StreamSize" /> as kibibytes.</summary>
        [Pure]
        public StreamSize KiB() => StreamSize.FromKibibytes(kib);
    }

    extension(double kib)
    {
        /// <summary>Creates a <see cref="StreamSize" /> as kibibytes.</summary>
        [Pure]
        public StreamSize KiB() => StreamSize.FromKibibytes(kib);
    }

    extension(int mib)
    {
        /// <summary>Creates a <see cref="StreamSize" /> as mebibytes.</summary>
        [Pure]
        public StreamSize MiB() => StreamSize.FromMebibytes(mib);
    }

    extension(long mib)
    {
        /// <summary>Creates a <see cref="StreamSize" /> as mebibytes.</summary>
        [Pure]
        public StreamSize MiB() => StreamSize.FromMebibytes(mib);
    }

    extension(double mib)
    {
        /// <summary>Creates a <see cref="StreamSize" /> as mebibytes.</summary>
        [Pure]
        public StreamSize MiB() => StreamSize.FromMebibytes(mib);
    }

    extension(int gib)
    {
        /// <summary>Creates a <see cref="StreamSize" /> as gibibytes.</summary>
        [Pure]
        public StreamSize GiB() => StreamSize.FromGibibytes(gib);
    }

    extension(long gib)
    {
        /// <summary>Creates a <see cref="StreamSize" /> as gibibytes.</summary>
        [Pure]
        public StreamSize GiB() => StreamSize.FromGibibytes(gib);
    }

    extension(double gib)
    {
        /// <summary>Creates a <see cref="StreamSize" /> as gibibytes.</summary>
        [Pure]
        public StreamSize GiB() => StreamSize.FromGibibytes(gib);
    }
}
