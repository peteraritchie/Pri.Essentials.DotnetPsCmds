using System.IO;

namespace Pri.Essentials.DotnetProjects;

/// <summary>
/// An encapsulation of a .NET .file (.sln, .csproj, etc.). Subclasses will
/// implement actions.
/// </summary>
public abstract class DotnetFile
{
	/// <summary>
	/// Initializes a new instance of the DotnetFile class with the specified
	/// directory and file name.
	/// </summary>
	/// <param name="directory">
	/// The path to the directory containing the file. If null, the current
	/// working directory is used.
	/// </param>
	/// <param name="name">
	/// The name of the file. If null, the file name is derived from the
	/// specified directory path.
	/// </param>
	protected DotnetFile(string? directory, string? name)
	{
		Directory = directory ?? System.IO.Directory.GetCurrentDirectory();
		Name = name ?? Path.GetFileName(Directory);
	}

	/// <summary>
	/// The directory where the .sln exists or would have been created.
	/// The parameter to the <code>dotnet new sln --output</code> option
	/// </summary>
	public string Directory { get; private set; }

	/// <summary>
	/// The name of the solution (filename). The parameter to the
	/// <code>dotnet new sln --name</code> option
	/// </summary>
	public string Name { get; private set; }

	/// <summary>The full path to the .sln file</summary>
	public abstract string FullPath { get; }
}