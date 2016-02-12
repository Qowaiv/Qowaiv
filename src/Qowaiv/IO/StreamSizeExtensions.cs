using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Qowaiv.IO
{
	/// <summary>Contains extensions for stream size.</summary>
	public static class StreamSizeExtensions
	{
		/// <summary>Gets the size of the byte array.</summary>
		public static StreamSize GetStreamSize(this byte[] bytes)
		{
			return StreamSize.FromByteArray(bytes);
		}

		/// <summary>Gets the size of the current file.</summary>
		public static StreamSize GetStreamSize(this FileInfo fileInfo)
		{
			return StreamSize.FromFileInfo(fileInfo);
		}

		/// <summary>Gets the stream size of the current directory.</summary>
		public static StreamSize GetStreamSize(this DirectoryInfo directoryInfo)
		{
			Guard.NotNull(directoryInfo, "directoryInfo");
			return directoryInfo
				.EnumerateFiles("*", SearchOption.AllDirectories)
				.Sum(file => file.Length);
		}
		
		/// <summary>Gets the stream size of the current stream.</summary>
		public static StreamSize GetStreamSize(this Stream stream)
		{
			return StreamSize.FromStream(stream);
		}

		/// <summary>Computes the sum of a sequence of stream sizes.</summary>
		public static StreamSize Sum(this IEnumerable<StreamSize> streamSizes)
		{
			return new StreamSize(streamSizes.Sum(streamSize => (long)streamSize));
		}
		/// <summary>Computes the average of a sequence of stream sizes.</summary>
		public static StreamSize Average(this IEnumerable<StreamSize> streamSizes)
		{
			return new StreamSize((long)streamSizes.Average(streamSize => (long)streamSize));
		}
	}
}
