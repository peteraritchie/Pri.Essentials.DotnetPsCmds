using System.Management.Automation;

using Pri.Essentials.DotnetProjects;

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
	///
	/// </summary>
	[Parameter(Mandatory = true,
		Position = 0,
		HelpMessage = "What project to add to the solution.")]
	public DotnetProject Project { get; set; }

	/// <summary>
	///
	/// </summary>
	[Parameter(Mandatory = true,
		Position = 1,
		HelpMessage = "The package ID to add to the project.")]
	public string PackageId { get; set; }

	/// <summary>
	///
	/// </summary>
	[Parameter(Mandatory = false,
		Position = 2,
		HelpMessage = "The package version to add to the project.")]
	public string PackageVersion { get; set; }

	/// <summary>
	///
	/// </summary>
	[Parameter(Mandatory = false,
		Position = 3,
		HelpMessage = "Allows prerelease packages to be installed..")]
	public bool Prerelease { get; set; }
}