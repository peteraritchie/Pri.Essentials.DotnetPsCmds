using System.IO;

namespace Pri.Essentials.DotnetProjects;

/// <summary>
/// An encapsulation of a .NET .csproj file and the actions available via
/// `dotnet sln`
/// </summary>
/// <remarks>
/// We don't have methods on this type to add files or references because
/// if methods on this type are called from PowerShell, the context is
/// not known, and we won't know the real current directory.
/// </remarks>
public class DotnetProject : DotnetFile
{
	/// <summary>
	/// Initializes a new instance of the DotnetProject class with the specified project directory and name.
	/// </summary>
	/// <param name="directory">The path to the project directory. Can be null to use the current directory.</param>
	/// <param name="name">The name of the project. Can be null if the project name should be inferred.</param>
	public DotnetProject(string? directory, string? name) : base(directory, name)
	{
	}

	/// <summary>
	/// TODO: handle different types of project besides CS
	/// </summary>
	public override string FullPath => Path.Combine(Directory, Path.ChangeExtension(Name, ".csproj"));

	public DotnetSolution? Solution { get; set; }
}