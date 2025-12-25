using System.IO;

using Pri.Essentials.Abstractions;
using Pri.Essentials.DotnetProjects.Commands.Constants;

namespace Pri.Essentials.DotnetProjects.Commands;

public class CreateClassLibProjectCommand(
	IShellExecutor shellExecutor,
	string outputDirectory,
	string outputName,
	SupportedFrameworkName frameworkName,
	IFileSystem? fileSystem = null)
	: ProjectCommandBase(shellExecutor, outputDirectory, outputName, frameworkName)
{
	private readonly IFileSystem fileSystem = fileSystem ?? IFileSystem.Default;

	public override string ActionName => BuildCommandLine();

	public override Result Execute()
	{
		var result = shellExecutor.Execute(BuildCommandLine());

		if (fileSystem.Exists(Path.Combine(outputDirectory, "Class1.cs")))
		{
			try
			{
				fileSystem.DeleteFile(Path.Combine(outputDirectory, "Class1.cs"));
			}
			catch (IOException)
			{
				// ignore if the file can't be deleted, it might have been
				// deleted in between checking for existence.
			}
		}
		return new ShellOperationResult(result.ExitCode, result.StandardOutputText, result.StandardErrorText);
	}

	protected override string BuildCommandLine()
	{
		var outputDirectoryOption = string.IsNullOrWhiteSpace(outputDirectory) ? string.Empty : $" -o {outputDirectory}";
		var outputNameOption = string.IsNullOrWhiteSpace(outputDirectory) ? string.Empty : $" -n {outputName}";

		return $"dotnet new {SupportedProjectTemplateName.ClassLib}{outputDirectoryOption}{outputNameOption} -f {FrameworkName}";
	}
}