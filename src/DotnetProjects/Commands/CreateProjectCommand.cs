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
public class CreateProjectCommand(
	IShellExecutor shellExecutor,
	SupportedProjectTemplateName templateName,
	string outputDirectory,
	string outputName,
	SupportedFrameworkName frameworkName)
	: ProjectCommandBase(shellExecutor, outputDirectory, outputName, frameworkName)
{
	/// <inheritdoc />
	public override string ActionName => BuildCommandLine();

	/// <inheritdoc />
	public override Result Execute()
	{
		var result = shellExecutor.Execute(BuildCommandLine());
		return new ShellOperationResult(result.ExitCode, result.StandardOutputText, result.StandardErrorText);
	}

	protected override string BuildCommandLine()
	{
		var outputDirectoryOption = string.IsNullOrWhiteSpace(outputDirectory) ? string.Empty : $" -o {outputDirectory}";
		var outputNameOption = string.IsNullOrWhiteSpace(outputDirectory) ? string.Empty : $" -n {outputName}";

		// TODO: add something to delete unittest1.cs with xunit projects?
		return $"dotnet new {templateName}{outputDirectoryOption}{outputNameOption} -f {FrameworkName}";
	}
}