using System;

namespace Pri.Essentials.Abstractions;

/// <summary>
/// Extension members for IFileSystem
/// </summary>
public static class FileSystemExtensions
{
	private static readonly Lazy<IFileSystem> LazyFileSystem = new(() => new FileSystem());

	extension(IFileSystem)
	{
		/// <summary>
		/// A singleton instance of a default IFileSystem implementation
		/// </summary>
		public static IFileSystem Default => LazyFileSystem.Value;
	}
}