using System.IO;

namespace Pri.Essentials.DotnetProjects.Commands;

public class AddClassToProjectCommand(IShellExecutor shellExecutor,
	DotnetProject targetProject, string className)
	:CommandBase(shellExecutor)
{
	public override string Target => targetProject.FullPath;
	public override string ActionName => $"Add class named '{className}' to {Target}";
	public override Result Execute()
	{
		var result = shellExecutor.Execute(BuildCommandLine());
		return new ShellOperationResult(result.ExitCode, result.StandardOutputText, result.StandardErrorText);
	}
	private string BuildCommandLine()
	{
		ParseQualifiedClassName(out string actualClassName, out string outputDir);
		return $"dotnet new class --project {Target} -o {outputDir} -n {actualClassName} --language C#";
	}

	private void ParseQualifiedClassName(out string actualClassName, out string outputDir)
	{
		ParseQualifiedClassName(className, targetProject.Directory, out actualClassName, out outputDir);
	}
	private static void ParseQualifiedClassName(string className, string targetProjectDirectory, out string actualClassName, out string outputDir)
	{
		var dirs = className.Split('.');
		actualClassName = dirs[^1];
		outputDir = Path.Combine([targetProjectDirectory, .. dirs[..^1]]);
	}
}