using System.IO;

namespace Pri.Essentials.DotnetProjects;

/// <summary>
/// An encapsulation of a .NET .sln file and the actions available via
/// `dotnet sln`
/// </summary>
/// <remarks>
/// We don't have methods on this type to create or add projects because
/// if methods on this type are called from PowerShell, the context is
/// not known, and we won't know the real current directory.
/// </remarks>
public class DotnetSolution : DotnetFile
{
	/// <summary>
	/// Initializes a new instance of the DotnetSolution class with the
	/// specified directory and solution name.
	/// </summary>
	/// <param name="directory">
	/// The path to the directory containing the
	/// solution. Can be null to use the current directory.
	/// </param>
	/// <param name="name">
	/// The name of the solution. Can be null to use
	/// a default or inferred name.
	/// </param>
	public DotnetSolution(string? directory, string? name) : base(directory, name)
	{
		// TODO: handle slnx
		FullPath = Path.GetExtension(Name) == ".sln"
			? Path.Combine(Directory, Name)
			: Path.Combine(Directory, Name + ".sln");
	}

	/// <summary>
	/// Gets the full file system path to the solution file, including the
	/// ".sln" extension.
	/// </summary>
	public override string FullPath { get; }
}