namespace Pri.Essentials.DotnetProjects.Commands;

public class AddProjectReferenceToProjectCommand(
	IShellExecutor shellExecutor,
	DotnetProject targetProject,
	DotnetProject referencedProject)
	: CommandBase(shellExecutor)
{
	public override string Target => targetProject.FullPath;
	public override string ActionName => $"Add {referencedProject.FullPath} to {Target}";
	public override Result Execute()
	{
		var result = shellExecutor.Execute(BuildCommandLine());
		return new ShellOperationResult(result.ExitCode, result.StandardOutputText, result.StandardErrorText);
	}
	private string BuildCommandLine()
	{
		return $"dotnet add reference --project {Target} {referencedProject.FullPath}";
	}
}