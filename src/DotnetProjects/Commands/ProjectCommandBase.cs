using System.IO;

using Pri.Essentials.DotnetProjects.Commands.Constants;

namespace Pri.Essentials.DotnetProjects.Commands;

public abstract class ProjectCommandBase(IShellExecutor shellExecutor,
	string outputDirectory,
	string outputName,
	SupportedFrameworkName frameworkName) : CommandBase(shellExecutor)
{
	protected readonly string outputDirectory = outputDirectory;
	protected readonly string outputName = outputName;

	/// <inheritdoc />
	public override string Target => Path.Combine(outputDirectory, outputName);

	protected string FrameworkName { get; } = frameworkName;

	protected abstract string BuildCommandLine();
}