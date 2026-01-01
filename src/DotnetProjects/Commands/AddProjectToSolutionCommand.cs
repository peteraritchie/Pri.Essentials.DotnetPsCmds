using System.IO;

using Pri.Essentials.Abstractions;

namespace Pri.Essentials.DotnetProjects.Commands;

/// <summary>
/// WIP
/// </summary>
/// <remarks>
/// dotnet reference add --project .\UI\ .\Lib\ .NET 10+
/// dotnet add reference --project .\UI\ .\Lib2\ .NET 0 and earlier
/// </remarks>
/// <param name="shellExecutor"></param>
/// <param name="solution"></param>
/// <param name="project"></param>
/// <param name="solutionFolder"></param>
/// <param name="fileSystem"></param>
public class AddProjectToSolutionCommand(
	IShellExecutor shellExecutor,
	DotnetSolution solution,
	DotnetProject project,
	string? solutionFolder,
	IFileSystem? fileSystem = null)
	: CommandBase(shellExecutor)
{
	private readonly IFileSystem fileSystem = fileSystem ?? IFileSystem.Default;
	/// <inheritdoc />
	public override string Target => solution.FullPath;

	/// <inheritdoc />
	public override string ActionName => BuildCommandLine();//$"Add {project.FullPath} to {Target}";

	/// <inheritdoc />
	public override Result Execute()
	{
		var commandLine = BuildCommandLine();
		var result = shellExecutor.Execute(commandLine);
		if (result.IsSuccess)
		{
			// Update file options
			if (ShouldGenerateDocumentationFile is not null && ShouldGenerateDocumentationFile == true)
			{
				using Stream fs = fileSystem.FileOpen(project.FullPath,
					FileMode.Open,
					FileAccess.ReadWrite,
					FileShare.None);
				VisualStudioProjectConfigurationService.EnableGenerateDocumentationFile(fs);
			}
		}
		return new ShellOperationResult(result.ExitCode, result.StandardOutputText, result.StandardErrorText)
		{
			OperationText = commandLine
		};
	}

	private string BuildCommandLine()
	{
		string otherOptions = string.IsNullOrWhiteSpace(solutionFolder)
			? " --in-root" // Avoid create solution folders for each project.
			: $" --solution-folder \"{solutionFolder}\"";

		return $"dotnet sln {solution.FullPath} add {project.FullPath}{otherOptions}";
	}

	/// <summary>
	/// Gets or sets whether to generate the xml doc file
	/// </summary>
	public bool? ShouldGenerateDocumentationFile { get; init; }
}