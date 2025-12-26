using System;
using System.Management.Automation;

using Pri.Essentials.DotnetProjects;
using Pri.Essentials.DotnetProjects.Commands;

namespace Pri.Essentials.DotnetPsCmds;

/// <summary>
/// Defines a cmdlet that adds a NuGet package reference to a .NET project within a solution.
/// </summary>
/// <remarks>This cmdlet is intended for use in PowerShell scripts or interactive sessions to automate the process
/// of adding package dependencies to .NET projects. The cmdlet supports confirmation prompts and can be used with
/// PowerShell's ShouldProcess pattern for safe execution. The output is the updated project with the new package
/// reference applied.</remarks>
[Cmdlet(VerbsCommon.Add, "DotnetPackage", SupportsShouldProcess = true)]
[OutputType(typeof(DotnetProject))]
public class AddDotnetPackageCmdlet : PSCmdlet
{
	/// <summary>
	/// Gets or sets the project to add to the solution.
	/// </summary>
	[Parameter(Mandatory = true,
		Position = 0,
		ValueFromPipeline = true,
		HelpMessage = "What project to add to the solution.")]
	public DotnetProject? Project { get; set; }

	/// <summary>
	/// Gets or sets the identifier of the package to add to the project.
	/// </summary>
	[Parameter(Mandatory = true,
		Position = 1,
		HelpMessage = "The package ID to add to the project.")]
	public string? PackageId { get; set; }

	/// <summary>
	/// Gets or sets the version of the package to add to the project.
	/// </summary>
	[Parameter(Mandatory = false,
		Position = 2,
		HelpMessage = "The package version to add to the project.")]
	public string? PackageVersion { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether prerelease packages are allowed to be installed.
	/// </summary>
	/// <remarks>If set to <see langword="true"/>, prerelease versions of packages may be included during
	/// installation. If <see langword="false"/>, only stable package versions are considered. If <see langword="null"/>,
	/// the default behavior is used.</remarks>
	[Parameter(Mandatory = false,
		Position = 3,
		HelpMessage = "Allows prerelease packages to be installed..")]
	public bool? Prerelease { get; set; }

	/// <inheritdoc />
	protected override void BeginProcessing()
	{
		if (string.IsNullOrWhiteSpace(PackageId))
		{
			throw new PSArgumentNullException(nameof(PackageId));
		}
		base.BeginProcessing();
	}

	/// <inheritdoc />
	protected override void ProcessRecord()
	{
		if (Project == null)
		{
			throw new PSArgumentNullException(nameof(Project));
		}
		var executor = ShellExecutor.Instance;
		var command = DetermineCommand(executor, Project!, PackageId!, PackageVersion, Prerelease ?? false);
		if (ShouldProcess(command.Target, command.ActionName))
		{
			var r = command.Execute() as ShellOperationResult;
			WriteDebug($"Exit code: {r!.ExitCode}");
			WriteDebug($"Output:{Environment.NewLine}{r.OutputText}");
			if (!string.IsNullOrWhiteSpace(r.ErrorText))
			{
				WriteError(new ErrorRecord(new Exception(r.ErrorText), "AddPackageFailed", ErrorCategory.NotSpecified, Project));
			}
			else
			{
				WriteObject(Project);
			}
		}
	}

	private CommandBase DetermineCommand(IShellExecutor executor, DotnetProject project, string packageId, string? packageVersion, bool isPrerelease)
	{
		var command = string.IsNullOrWhiteSpace(packageVersion)
			? new AddPackageReferenceToProjectCommand(executor, project, packageId) { IsPrerelease = isPrerelease }
			: new AddPackageReferenceToProjectCommand(executor, project, packageId, packageVersion!) { IsPrerelease = isPrerelease };
		return command;
	}
}