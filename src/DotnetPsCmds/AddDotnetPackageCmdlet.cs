using System.Management.Automation;

using Pri.Essentials.DotnetProjects;

namespace Pri.Essentials.DotnetPsCmds;

[Cmdlet(VerbsCommon.Add, "DotnetPackage", SupportsShouldProcess = true)]
[OutputType(typeof(DotnetProject))]
public class AddDotnetPackageCmdlet : PSCmdlet
{
	[Parameter(Mandatory = true,
		Position = 0,
		HelpMessage = "What project to add to the solution.")]
	public DotnetProject Project { get; set; }

	[Parameter(Mandatory = true,
		Position = 1,
		HelpMessage = "The package ID to add to the project.")]
	public string PackageId { get; set; }

	[Parameter(Mandatory = false,
		Position = 2,
		HelpMessage = "The package version to add to the project.")]
	public string PackageVersion { get; set; }

	[Parameter(Mandatory = false,
		Position = 3,
		HelpMessage = "Allows prerelease packages to be installed..")]
	public bool Prerelease { get; set; }
}