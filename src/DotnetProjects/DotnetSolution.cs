using System;
using System.IO;

using Pri.Essentials.DotnetProjects.Commands;
using Pri.Essentials.DotnetProjects.Commands.Constants;

namespace Pri.Essentials.DotnetProjects;

/// <summary>
/// An encapsulation of a .NET .sln file and the actions available via `dotnet sln`
/// </summary>
public class DotnetSolution : DotnetFile
{
	/// <summary>
	/// Initializes a new instance of the DotnetSolution class with the specified directory and solution name.
	/// </summary>
	/// <param name="directory">The path to the directory containing the solution. Can be null to use the current directory.</param>
	/// <param name="name">The name of the solution. Can be null to use a default or inferred name.</param>
	public DotnetSolution(string? directory, string? name) : base(directory, name)
	{
	}

	/// <summary>
	/// Initializes a new instance of the DotnetSolution class for the specified solution file path.
	/// </summary>
	/// <param name="path">The full file system path to the .NET solution file. Cannot be null or empty.</param>
	public DotnetSolution(string path) : base(path)
	{
	}

	/// <summary>
	/// Gets the full file system path to the solution file, including the ".sln" extension.
	/// </summary>
	public override string FullPath => Path.Combine(Directory, Path.ChangeExtension(Name, ".sln"));

	/// <summary>
	/// Adds the specified project to the current solution.
	/// </summary>
	/// <param name="project">The project to add to the solution. Cannot be null.</param>
	/// <returns>The project that was added to the solution.</returns>
	public DotnetProject AddProject(DotnetProject project)
	{
		var executor = ShellExecutor.Instance;
		var command = new AddProjectToSolutionCommand(executor, this, project);
		var r = command.Execute() as ShellOperationResult;
		return project;
	}

	/// <summary>
	///
	/// </summary>
	/// <remarks>if the directory is not rooted, assume the current directory
	/// is the solution directory.</remarks>
	/// <param name="templateName"></param>
	/// <param name="outputDirectory"></param>
	/// <param name="outputName"></param>
	/// <param name="frameworkName"></param>
	/// <returns></returns>
	public DotnetProject CreateProject(string templateName, string outputDirectory, string outputName, string frameworkName)
	{
		if (!SupportedProjectTemplateName.TryParse(templateName, out var template)) {
			throw new ArgumentOutOfRangeException(
				paramName: nameof(templateName),
				actualValue: templateName,
				message: $"Expected one of: {string.Join(", ", SupportedProjectTemplateName.All.Keys)}");
		}

		if (!SupportedFrameworkName.TryParse(frameworkName, out var framework)) {
			throw new ArgumentOutOfRangeException(
				paramName: nameof(frameworkName),
				actualValue: frameworkName,
				message: $"Expected one of: {string.Join(", ", SupportedFrameworkName.All.Keys)}");
		}
		if (string.IsNullOrWhiteSpace(outputDirectory))
		{
			outputDirectory = Directory;
		}
		if (!Path.IsPathRooted(outputDirectory))
		{
			outputDirectory = Path.Combine(Directory, outputDirectory);
		}

		if (string.IsNullOrWhiteSpace(outputName))
		{
			outputName = Path.GetFileName(outputDirectory);
		}
		var executor = ShellExecutor.Instance;
		var createCommand = ProjectTemplateNameMapping.CommandMap[template](
			executor,
			template,
			outputDirectory,
			outputName,
			framework);
		var r = createCommand.Execute() as ShellOperationResult;
		if (!r.IsSuccessful)
		{
			ThrowCreateProjectError(outputDirectory, outputName, r.OutputText, r.ErrorText);
		}
		var project = new DotnetProject(outputDirectory, outputName);
		var addCommand = new AddProjectToSolutionCommand(executor, this, project);
		r = addCommand.Execute() as ShellOperationResult;

		if (!r!.IsSuccessful)
		{
			ThrowAddProjectError(project, r.OutputText, r.ErrorText);
		}

		return project;
	}

	/// <summary>
	/// Throw an error, using standard error/output if present.
	/// </summary>
	/// <param name="outputName"></param>
	/// <param name="standardOutput"></param>
	/// <param name="standardError"></param>
	/// <param name="outputDirectory"></param>
	/// <exception cref="InvalidOperationException"></exception>
	[System.Diagnostics.CodeAnalysis.DoesNotReturn]
	private static void ThrowCreateProjectError(string outputDirectory, string outputName, string standardOutput, string standardError)
	{
		if (string.IsNullOrWhiteSpace(standardError))
		{
			throw new InvalidOperationException(
				$"Error creating project name '{outputName}' in directory '{outputDirectory}':{Environment.NewLine}{standardError}");
		}

		if (string.IsNullOrWhiteSpace(standardOutput))
		{
			throw new InvalidOperationException(
				$"Error creating project name '{outputName}' in directory '{outputDirectory}':{Environment.NewLine}{standardOutput}");
		}

		throw new InvalidOperationException($"Error creating project name '{outputName}' in directory '{outputDirectory}'");
	}

	/// <summary>
	/// Throw an error, using standard error/output if present.
	/// </summary>
	/// <param name="project"></param>
	/// <param name="standardOutput"></param>
	/// <param name="standardError"></param>
	/// <exception cref="InvalidOperationException"></exception>
	[System.Diagnostics.CodeAnalysis.DoesNotReturn]
	private void ThrowAddProjectError(DotnetProject project, string standardOutput, string standardError)
	{
		if (string.IsNullOrWhiteSpace(standardError))
		{
			throw new InvalidOperationException(
				$"Error adding project '{project.FullPath}' to solution '{FullPath}':{Environment.NewLine}{standardError}");
		}

		if (string.IsNullOrWhiteSpace(standardOutput))
		{
			throw new InvalidOperationException(
				$"Error adding project '{project.FullPath}' to solution '{FullPath}':{Environment.NewLine}{standardOutput}");
		}

		throw new InvalidOperationException(
			$"Error adding project '{project.FullPath}' to solution '{FullPath}'.");
	}
}