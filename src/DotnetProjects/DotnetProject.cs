using System;
using System.IO;

using Pri.Essentials.DotnetProjects.Commands;

namespace Pri.Essentials.DotnetProjects;

/// <summary>
/// An encapsulation of a .NET .csproj file and the actions available via `dotnet sln`
/// </summary>

public class DotnetProject : DotnetFile
{
	public DotnetProject(string? directory, string? name) : base(directory, name)
	{
	}

	public DotnetProject(string path) : base(path)
	{
	}

	/// <summary>
	/// TODO: handle different types of project besides CS
	/// </summary>
	public override string FullPath => Path.Combine(Directory, Path.ChangeExtension(Name, ".csproj"));

	public DotnetProject AddProjectReference(DotnetProject project)
	{
		var executor = ShellExecutor.Instance;
		var command = new AddProjectReferenceToProjectCommand(executor, this, project);
		var r = command.Execute() as ShellOperationResult;
		return project;
	}

	public DotnetProject AddClass(string className)
	{
		var executor = ShellExecutor.Instance;
		var command = new AddClassToProjectCommand(executor, this, className);
		var r = command.Execute() as ShellOperationResult;
		return this;

	}

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