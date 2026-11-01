using Qowaiv.IO;

namespace Qowaiv.Humanizer;

public static partial class Humanize
{
    extension(int size)
    {
        /// <summary>Creates a <see cref="StreamSize" /> as bytes.</summary>
        [Pure]
        public StreamSize Bytes() => new(size);

        /// <summary>Creates a <see cref="StreamSize" /> as kilobytes.</summary>
        [Pure]
        public StreamSize KB() => StreamSize.FromKilobytes(size);

        /// <summary>Creates a <see cref="StreamSize" /> as megabytes.</summary>
        [Pure]
        public StreamSize MB() => StreamSize.FromMegabytes(size);

        /// <summary>Creates a <see cref="StreamSize" /> as gigabytes.</summary>
        [Pure]
        public StreamSize GB() => StreamSize.FromGigabytes(size);

        /// <summary>Creates a <see cref="StreamSize" /> as kibibytes.</summary>
        [Pure]
        public StreamSize KiB() => StreamSize.FromKibibytes(size);

        /// <summary>Creates a <see cref="StreamSize" /> as mebibytes.</summary>
        [Pure]
        public StreamSize MiB() => StreamSize.FromMebibytes(size);

        /// <summary>Creates a <see cref="StreamSize" /> as gibibytes.</summary>
        [Pure]
        public StreamSize GiB() => StreamSize.FromGibibytes(size);
    }

    extension(long size)
    {
        /// <summary>Creates a <see cref="StreamSize" /> as bytes.</summary>
        [Pure]
        public StreamSize Bytes() => new(size);

        /// <summary>Creates a <see cref="StreamSize" /> as kilobytes.</summary>
        [Pure]
        public StreamSize KB() => StreamSize.FromKilobytes(size);

        /// <summary>Creates a <see cref="StreamSize" /> as megabytes.</summary>
        [Pure]
        public StreamSize MB() => StreamSize.FromMegabytes(size);

        /// <summary>Creates a <see cref="StreamSize" /> as gigabytes.</summary>
        [Pure]
        public StreamSize GB() => StreamSize.FromGigabytes(size);

        /// <summary>Creates a <see cref="StreamSize" /> as kibibytes.</summary>
        [Pure]
        public StreamSize KiB() => StreamSize.FromKibibytes(size);

        /// <summary>Creates a <see cref="StreamSize" /> as mebibytes.</summary>
        [Pure]
        public StreamSize MiB() => StreamSize.FromMebibytes(size);

        /// <summary>Creates a <see cref="StreamSize" /> as gibibytes.</summary>
        [Pure]
        public StreamSize GiB() => StreamSize.FromGibibytes(size);
    }

    extension(double size)
    {
        /// <summary>Creates a <see cref="StreamSize" /> as kilobytes.</summary>
        [Pure]
        public StreamSize KB() => StreamSize.FromKilobytes(size);

        /// <summary>Creates a <see cref="StreamSize" /> as megabytes.</summary>
        [Pure]
        public StreamSize MB() => StreamSize.FromMegabytes(size);

        /// <summary>Creates a <see cref="StreamSize" /> as gigabytes.</summary>
        [Pure]
        public StreamSize GB() => StreamSize.FromGigabytes(size);

        /// <summary>Creates a <see cref="StreamSize" /> as kibibytes.</summary>
        [Pure]
        public StreamSize KiB() => StreamSize.FromKibibytes(size);

        /// <summary>Creates a <see cref="StreamSize" /> as mebibytes.</summary>
        [Pure]
        public StreamSize MiB() => StreamSize.FromMebibytes(size);

        /// <summary>Creates a <see cref="StreamSize" /> as gibibytes.</summary>
        [Pure]
        public StreamSize GiB() => StreamSize.FromGibibytes(size);
    }
}
