namespace Pri.Essentials.DotnetProjects.Commands;

/// <summary>
/// Represents a command that adds a project reference to a target .NET project using the dotnet CLI.
/// </summary>
/// <remarks>This command automates the process of adding a project reference by invoking the 'dotnet add
/// reference' command. It is typically used in build automation or tooling scenarios to programmatically manage project
/// dependencies.</remarks>
/// <param name="shellExecutor">The shell executor used to run the underlying dotnet CLI command.</param>
/// <param name="targetProject">The target .NET project to which the reference will be added. Cannot be null.</param>
/// <param name="referencedProject">The .NET project to add as a reference to the target project. Cannot be null.</param>
public class AddProjectReferenceToProjectCommand(
	IShellExecutor shellExecutor,
	DotnetProject targetProject,
	DotnetProject referencedProject)
	: CommandBase(shellExecutor)
{
	/// <inheritdoc />
	public override string Target => targetProject.FullPath;

	/// <inheritdoc />
	public override string ActionName => $"Add {referencedProject.FullPath} to {Target}";

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
		return $"dotnet add reference --project {Target} {referencedProject.FullPath}";
	}
}