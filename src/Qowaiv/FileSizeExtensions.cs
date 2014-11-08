using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Qowaiv
{
	/// <summary>Contains extensions for file size.</summary>
	public static class FileSizeExtensions
	{
		/// <summary>Gets the size of the current file.</summary>
		public static FileSize GetFileSize(this FileInfo fileInfo)
		{
			return FileSize.FromFileInfo(fileInfo);
		}
		
		/// <summary>Gets the file size of the current stream.</summary>
		public static FileSize GetFileSize(this Stream stream)
		{
			return FileSize.FromStream(stream);
		}

		/// <summary>Computes the sum of a sequence of file sizes.</summary>
		public static FileSize Sum(this IEnumerable<FileSize> fileSizes)
		{
			return new FileSize(fileSizes.Sum(fileSize => (long)fileSize));
		}
		/// <summary>Computes the average of a sequence of file sizes.</summary>
		public static FileSize Average(this IEnumerable<FileSize> fileSizes)
		{
			return new FileSize((long)fileSizes.Average(fileSize => (long)fileSize));
		}
	}
}
