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
		var commandLine = BuildCommandLine();
		var result = shellExecutor.Execute(commandLine);

		if (fileSystem.Exists(Path.Combine(outputDirectory, "Class1.cs")))
		{
			TryDelete(Path.Combine(outputDirectory, "Class1.cs"));
		}
		return new ShellOperationResult(result.ExitCode, result.StandardOutputText, result.StandardErrorText)
		{
			OperationText = commandLine
		};
	}

	/// <summary>
	/// Attempts to delete the file at the specified path, ignoring any I/O
	/// errors that occur during deletion.
	/// </summary>
	/// <remarks>
	/// This wraps the untestable paranoid try/catch so that it can be
	/// ignored with ExcludeFromCodeCoverageAttribute
	/// </remarks>
	/// <param name="path">
	/// The path of the file to delete. Cannot be null or empty.
	/// </param>
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	private void TryDelete(string path)
	{
		try
		{
			fileSystem.DeleteFile(path);
		}
		catch (IOException)
		{
			// ignore if the file can't be deleted, it might have been
			// deleted in between checking for existence.
		}
	}

	/// <inheritdoc />
	protected override string BuildCommandLine()
	{
		var outputDirectoryOption = string.IsNullOrWhiteSpace(outputDirectory) ? string.Empty : $" -o {outputDirectory}";
		var outputNameOption = string.IsNullOrWhiteSpace(outputDirectory) ? string.Empty : $" -n {outputName}";

		return $"dotnet new {SupportedProjectTemplateName.ClassLib}{outputDirectoryOption}{outputNameOption} -f {FrameworkName}";
	}
}