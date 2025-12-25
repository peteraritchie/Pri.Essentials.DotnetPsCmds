using System.IO;

using Pri.Essentials.Abstractions;
using Pri.Essentials.DotnetProjects.Commands.Constants;

namespace Pri.Essentials.DotnetProjects.Commands;

/// <summary>
/// Represents a command that creates a new .NET class library project using the specified parameters.
/// </summary>
/// <remarks>This command automates the creation of a .NET class library project by invoking the appropriate
/// 'dotnet new classlib' command. After project creation, it attempts to remove the default 'Class1.cs' file if it
/// exists. This class is typically used in build automation or tooling scenarios where programmatic project creation is
/// required.</remarks>
/// <param name="shellExecutor">The shell executor used to run the project creation command.</param>
/// <param name="outputDirectory">The directory in which to create the new class library project. If empty or null, the current directory is used.</param>
/// <param name="outputName">The name to assign to the new project. If empty or null, the default project name is used.</param>
/// <param name="frameworkName">The target framework for the new class library project.</param>
/// <param name="fileSystem">An optional file system abstraction used for file operations. If null, the default file system is used.</param>
public class CreateClassLibProjectCommand(
	IShellExecutor shellExecutor,
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
		var result = shellExecutor.Execute(BuildCommandLine());

		if (fileSystem.Exists(Path.Combine(outputDirectory, "Class1.cs")))
		{
			try
			{
				fileSystem.DeleteFile(Path.Combine(outputDirectory, "Class1.cs"));
			}
			catch (IOException)
			{
				// ignore if the file can't be deleted, it might have been
				// deleted in between checking for existence.
			}
		}
		return new ShellOperationResult(result.ExitCode, result.StandardOutputText, result.StandardErrorText);
	}

	/// <inheritdoc />
	protected override string BuildCommandLine()
	{
		var outputDirectoryOption = string.IsNullOrWhiteSpace(outputDirectory) ? string.Empty : $" -o {outputDirectory}";
		var outputNameOption = string.IsNullOrWhiteSpace(outputDirectory) ? string.Empty : $" -n {outputName}";

		return $"dotnet new {SupportedProjectTemplateName.ClassLib}{outputDirectoryOption}{outputNameOption} -f {FrameworkName}";
	}
}