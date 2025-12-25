using System.IO;

namespace Pri.Essentials.DotnetProjects.Commands;

public class CreateSolutionCommand(IShellExecutor shellExecutor, string outputDirectory, string outputName)
	: CommandBase(shellExecutor)
{
	/// <inheritdoc />
	public override Result Execute()
    {
		var result = shellExecutor.Execute(BuildCommandLine());
        return new ShellOperationResult(result.ExitCode, result.StandardOutputText, result.StandardErrorText);
    }

	private string BuildCommandLine()
    {
        var outputDirectoryOption = string.IsNullOrWhiteSpace(outputDirectory) ? string.Empty : $" -o {outputDirectory}";
        var outputNameOption = string.IsNullOrWhiteSpace(outputDirectory) ? string.Empty : $" -n {outputName}";
		var otherOptions = " --in-root"; // Avoid create solution folders for each project.

        return $"dotnet new sln{otherOptions}{outputDirectoryOption}{outputNameOption}";
    }

	/// <inheritdoc />
	public override string Target => Path.Combine(outputDirectory, outputName);
	/// <inheritdoc />
	public override string ActionName => "Create .NET solution file";
}