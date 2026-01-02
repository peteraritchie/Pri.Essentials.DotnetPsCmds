using System.IO;

using Pri.Essentials.Abstractions;
using Pri.Essentials.DotnetProjects.Commands.Constants;

namespace Pri.Essentials.DotnetProjects.Commands;

/// <summary>
/// Represents a command to create a new .NET project based on a specified template and framework.
/// </summary>
/// <remarks>
/// This command is responsible for generating a .NET project file in the specified output directory
/// with the provided name, using the given project template and framework.
/// </remarks>
/// <param name="shellExecutor">
/// The shell executor used to execute the underlying shell commands required for project creation.
/// </param>
/// <param name="templateName">
/// The name of the project template to use for creating the .NET project.
/// </param>
/// <param name="outputDirectory">
/// The directory where the new project will be created.
/// </param>
/// <param name="outputName">
/// The name of the output project file.
/// </param>
/// <param name="frameworkName">
/// The target framework for the new project.
/// </param>
/// <param name="fileSystem"></param>
public class CreateProjectCommand(
	IShellExecutor shellExecutor,
	SupportedProjectTemplateName templateName,
	string outputDirectory,
	string outputName,
	SupportedFrameworkName frameworkName,
	IFileSystem? fileSystem = null)
	: ProjectCommandBase(shellExecutor, outputDirectory, outputName, frameworkName)
{
	private readonly IFileSystem fileSystem = fileSystem ?? IFileSystem.Default;
	/// <inheritdoc />
	public override string ActionName => BuildCommandLine();

	/// <inheritdoc />
	public override Result Execute()
	{
		var commandLine = BuildCommandLine();
		var result = shellExecutor.Execute(commandLine);
		if (result.IsSuccess)
		{
			var project = new DotnetProject(outputDirectory, outputName);
			using var stream = fileSystem.FileOpen(project.FullPath,
				FileMode.Open,
				FileAccess.ReadWrite,
				FileShare.None);
			// add GenerateDocumentationFile true to the csproj file if needed
			if (ShouldGenerateDocumentationFile is true)
			{
				VisualStudioProjectConfigurationService.EnableGenerateDocumentationFile(stream);
			}
		}
		return new ShellOperationResult(result.ExitCode, result.StandardOutputText, result.StandardErrorText)
		{
			OperationText = commandLine
		};
	}

	/// <inheritdoc />
	protected override string BuildCommandLine()
	{
		var outputDirectoryOption = string.IsNullOrWhiteSpace(outputDirectory) ? string.Empty : $" -o {outputDirectory}";
		var outputNameOption = string.IsNullOrWhiteSpace(outputDirectory) ? string.Empty : $" -n {outputName}";

		// TODO: add something to delete unittest1.cs with xunit projects?
		return $"dotnet new {templateName}{outputDirectoryOption}{outputNameOption} -f {FrameworkName}";
	}
}