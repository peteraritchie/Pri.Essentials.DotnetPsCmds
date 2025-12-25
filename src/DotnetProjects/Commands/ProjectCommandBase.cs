using System.IO;

using Pri.Essentials.DotnetProjects.Commands.Constants;

namespace Pri.Essentials.DotnetProjects.Commands;

/// <summary>
/// Provides a base class for project-related command implementations that generate output files for a specific
/// framework.
/// </summary>
/// <remarks>This class is intended to be inherited by commands that operate on project files and produce output
/// for a specified framework. Derived classes must implement the BuildCommandLine method to define the specific
/// command-line arguments required for execution.</remarks>
/// <param name="shellExecutor">The shell executor used to run external commands required by the project command.</param>
/// <param name="outputDirectory">The directory where the output file will be created. Cannot be null or empty.</param>
/// <param name="outputName">The name of the output file to generate. Cannot be null or empty.</param>
/// <param name="frameworkName">The target framework for which the command is executed.</param>
public abstract class ProjectCommandBase(IShellExecutor shellExecutor,
	string outputDirectory,
	string outputName,
	SupportedFrameworkName frameworkName) : CommandBase(shellExecutor)
{
	/// <summary>
	/// 
	/// </summary>
	protected readonly string outputDirectory = outputDirectory;
	/// <summary>
	/// 
	/// </summary>
	protected readonly string outputName = outputName;

	/// <inheritdoc />
	public override string Target => Path.Combine(outputDirectory, outputName);

	/// <summary>
	/// 
	/// </summary>
	protected string FrameworkName { get; } = frameworkName;

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	protected abstract string BuildCommandLine();
}