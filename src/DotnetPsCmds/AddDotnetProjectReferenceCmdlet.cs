using System;
using System.Management.Automation;

using Pri.Essentials.DotnetProjects;
using Pri.Essentials.DotnetProjects.Commands;

namespace Pri.Essentials.DotnetPsCmds;

/// <summary>
/// Represents a PowerShell cmdlet that adds a .NET project reference to an
/// existing .NET project using the dotnet CLI.
/// </summary>
/// <remarks>This cmdlet supports ShouldProcess, enabling confirmation prompts
/// and WhatIf support. Use this cmdlet to programmatically add a project to
/// a solution file within automation scripts or interactive PowerShell
/// sessions. The cmdlet writes the updated solution object to the output
/// pipeline upon successful completion.</remarks>
[Cmdlet(VerbsCommon.Add, "DotnetProjectReference", SupportsShouldProcess = true)]
[OutputType(typeof(DotnetSolution))]
// ReSharper disable once UnusedType.Global
public class AddDotnetProjectReferenceCmdlet : PSCmdlet
{
	/// <summary>
	///
	/// </summary>
	[Parameter(Mandatory = true,
		Position = 0,
		ValueFromPipeline = true,
		HelpMessage = "What project to add the reference to.")]
	public DotnetProject? TargetProject { get; set; }

	/// <summary>
	///
	/// </summary>
	[Parameter(Mandatory = true,
		Position = 1,
		HelpMessage = "What project to reference in the target project.")]
	public DotnetProject? Project { get; set; }

	/// <inheritdoc />
	protected override void BeginProcessing()
	{
		if(Project == null)
		{
			throw new PSArgumentNullException(nameof(Project));
		}

		base.BeginProcessing();
	}

	/// <inheritdoc />
	protected override void ProcessRecord()
	{
		if(TargetProject == null)
		{
			throw new PSArgumentNullException(nameof(TargetProject));
		}

		var executor = ShellExecutor.Instance;
		var command = DetermineCommand(executor, TargetProject!, Project!);
		if (ShouldProcess(command.Target, command.ActionName))
		{
			var r = command.Execute() as ShellOperationResult;

			WriteDebug($"Exit code: {r!.ExitCode}");
			WriteDebug($"Output:{Environment.NewLine}{r.OutputText}");
			if (!string.IsNullOrWhiteSpace(r.ErrorText))
			{
				WriteDebug($"Error:{Environment.NewLine}{r.ErrorText}");
			}
			WriteObject(TargetProject);
		}
	}

	private static CommandBase DetermineCommand(IShellExecutor executor,
		DotnetProject targetProject,
		DotnetProject project)
	{
		return new AddProjectReferenceToProjectCommand(executor,
			targetProject,
			project);
	}

	/// <inheritdoc />
	protected override void EndProcessing()
	{
		base.EndProcessing();
	}
}