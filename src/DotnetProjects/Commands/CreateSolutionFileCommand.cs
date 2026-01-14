using System.IO;

namespace Pri.Essentials.DotnetProjects.Commands;

/// <summary>
/// Represents a command that creates a new .NET solution file using the 'dotnet new sln' command.
/// </summary>
/// <param name="shellExecutor">The shell executor used to run command-line operations.</param>
/// <param name="outputDirectory">The directory in which to create the solution file. If null or empty, the current directory is used.</param>
/// <param name="outputName">The name of the solution file to create.</param>
public class CreateSolutionFileCommand(IShellExecutor shellExecutor, string outputDirectory, string outputName)
	: CommandBase(shellExecutor)
{
	/// <inheritdoc />
	public override Result Execute()
	{
		var commandLine = BuildCommandLine();
		var result = shellExecutor.Execute(commandLine);
		return new ShellOperationResult(result.ExitCode, result.StandardOutputText, result.StandardErrorText)
		{
			OperationText = commandLine
		};
	}

	private string BuildCommandLine()
	{
		var outputDirectoryOption = string.IsNullOrWhiteSpace(outputDirectory) ? string.Empty : $" -o {outputDirectory}";
		var outputNameOption = string.IsNullOrWhiteSpace(outputDirectory) ? string.Empty : $" -n {outputName}";

		return $"dotnet new sln -f sln{outputDirectoryOption}{outputNameOption}";
	}

	/// <inheritdoc />
	public override string Target => Path.Combine(outputDirectory, outputName);

	/// <inheritdoc />
	public override string ActionName => "Create .NET solution file";
}

/// <summary>
/// Represents a command that creates a new .NET solution including a sln file,
/// global.json, etc.
/// </summary>
/// <param name="shellExecutor">The shell executor used to run command-line operations.</param>
/// <param name="outputDirectory">The directory in which to create the solution file. If null or empty, the current directory is used.</param>
/// <param name="outputName">The name of the solution file to create.</param>
public class CreateSolutionCommand(IShellExecutor shellExecutor, string outputDirectory, string outputName, string? frameworkName)
	: CommandBase(shellExecutor)
{
	/// <inheritdoc />
	public override Result Execute()
	{
		var createSolutionFileCommand = new CreateSolutionFileCommand(shellExecutor, outputDirectory, outputName);
		var firstResult = createSolutionFileCommand.Execute() as ShellOperationResult;
		return new ShellOperationResult(firstResult.ExitCode, firstResult.OutputText, firstResult.ErrorText)
		{
			OperationText = firstResult.OperationText,
		};
	}

	/// <inheritdoc />
	public override string Target => Path.Combine(outputDirectory, outputName);

	/// <inheritdoc />
	public override string ActionName => "Create .NET solution";
}