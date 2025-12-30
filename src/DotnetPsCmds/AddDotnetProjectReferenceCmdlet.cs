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
[Cmdlet(VerbsCommon.Add, "DotnetProjectReference",
	SupportsShouldProcess = true)]
[OutputType(typeof(DotnetSolution))]
// ReSharper disable once UnusedType.Global
public class AddDotnetProjectReferenceCmdlet : PSCmdlet
{
	/// <summary>
	///
	/// </summary>
	[Parameter(Mandatory = true,
		Position = 0,
		HelpMessage = "What project to reference in the target project.")]
	public DotnetProject? Project { get; set; }

	/// <summary>
	///
	/// </summary>
	[Parameter(Mandatory = true,
		Position = 1,
		ValueFromPipeline = true,
		HelpMessage = "What project to add the reference to.")]
	public DotnetProject? TargetProject { get; set; }

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
			var addResult = command.Execute() as ShellOperationResult;
			if (addResult!.IsFailure)
			{
				ThrowTerminatingError(new ErrorRecord(
					new InvalidOperationException(
						$"Failed to add reference to project. Exit code " +
						$"{addResult.ExitCode}{Environment.NewLine}" +
						$"Error output:" +
						$"{Environment.NewLine}{addResult.ErrorText}"),
					errorId: "AddProjectReferenceToProjectFailed",
					errorCategory: ErrorCategory.OperationStopped,
					targetObject: null));
			}

			WriteDebug($"Operation: {addResult!.OperationText}");
			WriteDebug($"Exit code: {addResult.ExitCode}");
			WriteDebug($"Output:{Environment.NewLine}{addResult.OutputText}");
			if (!string.IsNullOrWhiteSpace(addResult.ErrorText))
			{
				WriteDebug(
					$"Error:{Environment.NewLine}{addResult.ErrorText}");
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