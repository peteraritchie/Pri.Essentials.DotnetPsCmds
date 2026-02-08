using System.IO;

using Pri.Essentials.Abstractions;
using Pri.Essentials.DotnetProjects.Commands.Constants;

namespace Pri.Essentials.DotnetProjects.Commands;

/// <summary>
/// Represents a command to create a new .NET project based on a
/// specified template and framework.
/// </summary>
/// <remarks>
/// This command is responsible for generating a .NET project file in
/// the specified output directory with the provided name, using the given
/// project template and framework.
/// </remarks>
/// <param name="shellExecutor">
/// The shell executor used to execute the underlying shell commands required
/// for project creation.
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
public class CreateXunit3ProjectCommand(
	IShellExecutor shellExecutor,
	string outputDirectory,
	string outputName,
	SupportedFrameworkName frameworkName,
	IFileSystem? fileSystem = null)
	: ProjectCommandBase(shellExecutor, outputDirectory, outputName, frameworkName)
{
	/// <inheritdoc />
	public override string ActionName => BuildCommandLine();
	private readonly IFileSystem fileSystem = fileSystem ?? IFileSystem.Default;

	/// <inheritdoc />
	public override Result Execute()
	{
		var commandLine = BuildCommandLine();
		var result = shellExecutor.Execute(commandLine);

		if (fileSystem.Exists(Path.Combine(outputDirectory, "UnitTest1.cs")))
		{
			try
			{
				fileSystem.DeleteFile(Path.Combine(outputDirectory, "UnitTest1.cs"));
			}
			catch (IOException)
			{
				// ignore if the file can't be deleted, it might have been
				// deleted in between checking for existence.
			}
		}
		return new ShellOperationResult(ExitCode: result.ExitCode,
			OutputText: result.StandardOutputText,
			ErrorText: result.StandardErrorText)
		{
			OperationText = commandLine
		};
	}

	/// <inheritdoc />
	protected override string BuildCommandLine()
	{
		var outputDirectoryOption = string.IsNullOrWhiteSpace(outputDirectory)
			? string.Empty
			: $" -o {outputDirectory}";
		var outputNameOption = string.IsNullOrWhiteSpace(outputDirectory)
			? string.Empty
			: $" -n {outputName}";

		return $"dotnet new {SupportedProjectTemplateName.XUnit3}{outputDirectoryOption}{outputNameOption} -f {FrameworkName}";
	}
}