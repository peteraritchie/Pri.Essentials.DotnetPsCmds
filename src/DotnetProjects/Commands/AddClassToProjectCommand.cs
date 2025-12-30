using System.IO;

namespace Pri.Essentials.DotnetProjects.Commands;

/// <summary>
/// An add class to project command abstraction.
/// </summary>
public class AddClassToProjectCommand(IShellExecutor shellExecutor,
	DotnetProject targetProject, string className)
	:CommandBase(shellExecutor)
{
	/// <inheritdoc/>
	public override string Target => targetProject.FullPath;
	/// <inheritdoc/>
	public override string ActionName => $"Add class named '{className}' to {Target}";
	/// <inheritdoc/>
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