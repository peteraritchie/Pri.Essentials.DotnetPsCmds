using System;
using System.IO;

using Pri.Essentials.DotnetProjects.Commands;

namespace Pri.Essentials.DotnetProjects;

/// <summary>
/// An encapsulation of a .NET .csproj file and the actions available via `dotnet sln`
/// </summary>

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
	/// Initializes a new instance of the DotnetProject class for the specified project file path.
	/// </summary>
	/// <param name="path">The full file system path to the .NET project file. Cannot be null or empty.</param>
	public DotnetProject(string path) : base(path)
	{
	}

	/// <summary>
	/// TODO: handle different types of project besides CS
	/// </summary>
	public override string FullPath => Path.Combine(Directory, Path.ChangeExtension(Name, ".csproj"));

	/// <summary>
	/// Adds a reference to the specified project in the current project.
	/// </summary>
	/// <param name="project">The project to add as a reference. Cannot be null.</param>
	/// <returns>The project that was added as a reference.</returns>
	public DotnetProject AddProjectReference(DotnetProject project)
	{
		var executor = ShellExecutor.Instance;
		var command = new AddProjectReferenceToProjectCommand(executor, this, project);
		var r = command.Execute() as ShellOperationResult;
		return project;
	}

	/// <summary>
	/// Adds a new class with the specified name to the project.
	/// </summary>
	/// <param name="className">The name of the class to add to the project. Cannot be null or empty.</param>
	/// <returns>The current <see cref="DotnetProject"/> instance, enabling method chaining.</returns>
	public DotnetProject AddClass(string className)
	{
		var executor = ShellExecutor.Instance;
		var command = new AddClassToProjectCommand(executor, this, className);
		var r = command.Execute() as ShellOperationResult;
		return this;

	}

	/// <summary>
	/// Adds a NuGet package reference to the project.
	/// </summary>
	/// <param name="packageName">The name of the NuGet package to add. Cannot be null or empty.</param>
	/// <param name="packageVersion">The version of the NuGet package to add. If null or empty, the latest available version is used.</param>
	/// <returns>The current <see cref="DotnetProject"/> instance, enabling method chaining.</returns>
	public DotnetProject AddPackageReference(string packageName, string? packageVersion = null)
	{
		var executor = ShellExecutor.Instance;

		var command
			= string.IsNullOrWhiteSpace(packageVersion)
				? new AddPackageReferenceToProjectCommand(executor, this, packageName)
				: new AddPackageReferenceToProjectCommand(executor, this, packageName, packageVersion!);

		var r = command.Execute() as ShellOperationResult;

		if (!r!.IsSuccessful)
		{
			ThrowAddPackageError(packageName, r.OutputText, r.ErrorText);
		}

		return this;
	}

	/// <summary>
	/// Throw an error, using standard error/output if present.
	/// </summary>
	/// <param name="packageName"></param>
	/// <param name="standardOutput"></param>
	/// <param name="standardError"></param>
	/// <exception cref="InvalidOperationException"></exception>
	[System.Diagnostics.CodeAnalysis.DoesNotReturn]
	private void ThrowAddPackageError(string packageName, string standardOutput, string standardError)
	{
		if (string.IsNullOrWhiteSpace(standardError))
		{
			throw new InvalidOperationException(
				$"Error occurred adding package '{packageName}' to project '{FullPath}':{Environment.NewLine}{standardError}");
		}

		if (string.IsNullOrWhiteSpace(standardOutput))
		{
			throw new InvalidOperationException(
				$"Error occurred adding package '{packageName}' to project '{FullPath}':{Environment.NewLine}{standardOutput}");
		}

		throw new InvalidOperationException(
			$"Error occurred adding package '{packageName}' to project '{FullPath}'");
	}
}