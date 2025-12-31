using System.IO;

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
public class AddProjectToSolutionCommand(IShellExecutor shellExecutor, DotnetSolution solution, DotnetProject project)
	: CommandBase(shellExecutor)
{
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
				using FileStream fs = new(project.FullPath,
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
		var otherOptions = " --in-root"; // Avoid create solution folders for each project.
		return $"dotnet sln {solution.FullPath} add {project.FullPath}{otherOptions}";
	}

	public bool? ShouldGenerateDocumentationFile { get; init; }
}