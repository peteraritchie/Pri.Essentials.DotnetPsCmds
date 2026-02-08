using System.IO;
using System.Text;
using System.Xml.Linq;

using Pri.Essentials.Abstractions;
using Pri.Essentials.DotnetProjects.Commands.Constants;

namespace Pri.Essentials.DotnetProjects.Commands;

/// <summary>
/// Represents a command that creates a new .NET class library project using
/// the specified parameters.
/// </summary>
/// <remarks>This command automates the creation of a .NET class library
/// project by invoking the appropriate 'dotnet new classlib' command.
/// After project creation, it attempts to remove the default 'Class1.cs'
/// file if it exists. This class is typically used in build automation or
/// tooling scenarios where programmatic project creation is
/// required.</remarks>
/// <param name="shellExecutor">
/// The shell executor used to run the project creation command.
/// </param>
/// <param name="outputDirectory">
/// The directory in which to create the new class library project. If empty
/// or null, the current directory is used.
/// </param>
/// <param name="outputName">
/// The name to assign to the new project. If empty or null, the default
/// project name is used.
/// </param>
/// <param name="frameworkName">
/// The target framework for the new class library project.
/// </param>
/// <param name="fileSystem">
/// An optional file system abstraction used for file operations. If null,
/// the default file system is used.
/// </param>
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

		if (result.IsSuccess)
		{
			var project = new DotnetProject(outputDirectory, outputName);
			using var stream = fileSystem.FileOpen(project.FullPath,
				FileMode.Open,
				FileAccess.ReadWrite,
				FileShare.None);

			if (fileSystem.Exists(Path.Combine(outputDirectory, "Class1.cs")))
			{
				TryDelete(Path.Combine(outputDirectory, "Class1.cs"));
			}

			// TODO: test this again with use of instance
			// VisualStudioProjectConfigurationService

			// add GenerateDocumentationFile true to the csproj file if needed
			if (ShouldGenerateDocumentationFile is true)
			{
				var doc = XDocument.Load(stream);
				var projectConfigurationService =
					new VisualStudioProjectConfigurationService(doc);

				projectConfigurationService.SetGenerateDocumentationFile();

				// Reset position before saving
				stream.Position = 0;
				doc.Save(stream);
			}
		}

		return new ShellOperationResult(ExitCode: result.ExitCode,
			OutputText: result.StandardOutputText,
			ErrorText: result.StandardErrorText)
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
		StringBuilder stringBuilder = new();
		stringBuilder.Append("dotnet new ")
			.Append(SupportedProjectTemplateName.ClassLib);

		if(!string.IsNullOrWhiteSpace(outputDirectory))
		{
			stringBuilder.Append($" -o {outputDirectory}");
		}

		if(!string.IsNullOrWhiteSpace(outputDirectory))
		{
			stringBuilder.Append($" -n {outputName}");
		}

		stringBuilder.Append(" -f ")
			.Append(FrameworkName);

		return stringBuilder.ToString();
	}
}