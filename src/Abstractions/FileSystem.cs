using System;
using System.IO;

namespace Pri.Essentials.Abstractions;

/// <summary>
/// A file system abstraction.
/// </summary>
internal class FileSystem : IFileSystem
{
	/// <inheritdoc />
	public bool Exists(string path)
	{
		return FileExists(path) || DirectoryExists(path);
	}

	/// <inheritdoc />
	public bool FileExists(string path)
	{
		return File.Exists(path);
	}

	/// <inheritdoc />
	public bool DirectoryExists(string path)
	{
		return Directory.Exists(path);
	}

	/// <inheritdoc />
	public void DeleteFile(string path)
	{
		File.Delete(path);
	}
}

/// <summary>
/// Provides an abstraction for file system operations, including checking for the existence of files and directories
/// and deleting files.
/// </summary>
/// <remarks>Implementations of this interface allow code to interact with the file system in a
/// platform-independent manner, which is useful for testing and for supporting multiple storage backends. Methods may
/// throw exceptions if invalid paths are provided or if operations fail due to permissions or other file system
/// errors.</remarks>
public interface IFileSystem
{
	/// <summary>
	/// Determines whether the specified path refers to an existing file or directory.
	/// </summary>
	/// <param name="path">The path to the file or directory to check. This can be either an absolute or relative path. Cannot be null or
	/// empty.</param>
	/// <returns>true if the specified path exists as a file or directory; otherwise, false.</returns>
	bool Exists(string path);

	/// <summary>
	/// Determines whether the specified file exists at the given path.
	/// </summary>
	/// <remarks>This method only checks for the existence of a file, not a directory. The result may be affected
	/// by file system permissions or concurrent changes to the file system.</remarks>
	/// <param name="path">The path to the file to check for existence. This can be either a relative or absolute path. Cannot be null or an
	/// empty string.</param>
	/// <returns>true if a file exists at the specified path; otherwise, false.</returns>

	bool FileExists(string path);
	/// <summary>
	/// Determines whether the specified path refers to an existing directory.
	/// </summary>
	/// <remarks>This method does not validate whether the path is a file or a directory; it only checks for the
	/// existence of a directory at the specified path. No exception is thrown if the directory does not exist or the path
	/// is invalid.</remarks>
	/// <param name="path">The path to the directory to check. The path can be relative or absolute. Cannot be null, empty, or contain
	/// invalid characters.</param>
	/// <returns>true if the specified path refers to an existing directory; otherwise, false.</returns>

	bool DirectoryExists(string path);
	/// <summary>
	/// Deletes the specified file from the file system.
	/// </summary>
	/// <remarks>If the file does not exist, no exception is thrown. This method does not delete directories.
	/// Relative paths are interpreted relative to the current working directory.</remarks>
	/// <param name="path">The path to the file to be deleted. The path must refer to an existing file and cannot be null, empty, or contain
	/// invalid characters.</param>
	void DeleteFile(string path);
}

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