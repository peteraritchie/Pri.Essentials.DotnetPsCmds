using System.Text;

namespace Pri.Essentials.DotnetProjects.Commands;

/// <summary>
/// Represents a command to add a package reference to a .NET project file.
/// </summary>
/// <remarks>
/// This command utilizes a shell executor to execute the necessary `dotnet` CLI commands
/// to add a package reference to the specified project. It supports specifying an optional
/// package version and can handle pre-release packages.
/// </remarks>
/// <param name="shellExecutor">The shell executor used to execute the command.</param>
/// <param name="targetProject">The target .NET project to which the package reference will be added.</param>
/// <param name="packageName">The name of the package to add as a reference.</param>
public class AddPackageReferenceToProjectCommand(
	IShellExecutor shellExecutor,
	DotnetProject targetProject,
	string packageName)
	: CommandBase(shellExecutor)
{
	private readonly string? packageVersion;

	/// <summary>
	/// Initializes a new instance of the <see cref="AddPackageReferenceToProjectCommand"/> class with a specified package version.
	/// </summary>
	/// <param name="shellExecutor">The shell executor used to execute the command.</param>
	/// <param name="targetProject">The target .NET project to which the package reference will be added.</param>
	/// <param name="packageName">The name of the package to add as a reference.</param>
	/// <param name="packageVersion">The version of the package to add as a reference.</param>
	public AddPackageReferenceToProjectCommand(IShellExecutor shellExecutor,
		DotnetProject targetProject,
		string packageName,
		string packageVersion) : this(shellExecutor, targetProject, packageName)
	{
		this.packageVersion = packageVersion;
	}

	/// <inheritdoc />
	public override string Target => targetProject.FullPath;

	/// <inheritdoc />
	public override string ActionName => BuildCommandLine();//$"Add package '{packageName}' to {Target}";

	/// <summary>
	/// Whether it is a prerelease or not
	/// </summary>
	public bool IsPrerelease { get; init; }

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
		StringBuilder sb = new();
		sb.Append($"dotnet add {Target} package {packageName}");

		if (!string.IsNullOrWhiteSpace(packageVersion))
		{
			sb.Append($" -v {packageVersion}");
		}

		if (IsPrerelease)
		{
			sb.Append(" --prerelease");
		}

		return sb.ToString();
	}
}