using System;
using System.Management.Automation;

using Pri.Essentials.DotnetProjects;
using Pri.Essentials.DotnetProjects.Commands;

namespace Pri.Essentials.DotnetPsCmds;

/// <summary>
/// Represents a PowerShell cmdlet that adds a .NET project to an existing
/// solution using the dotnet CLI.
/// </summary>
/// <remarks>This cmdlet supports ShouldProcess, enabling confirmation prompts
/// and WhatIf support. Use this cmdlet to programmatically add a project to a
/// solution file within automation scripts or interactive PowerShell sessions.
/// The cmdlet writes the updated solution object to the output pipeline upon
/// successful completion.</remarks>
[Cmdlet(VerbsCommon.Add, nameof(DotnetProject), SupportsShouldProcess = true)]
[OutputType(typeof(DotnetSolution))]
// ReSharper disable once UnusedType.Global
public class AddDotnetProjectCmdlet : PSCmdlet
{
	/// <summary>
	/// Gets or sets the solution to which the project will be added.
	/// </summary>
	[Parameter(Mandatory = true,
		Position = 0,
		ValueFromPipeline = true,
		HelpMessage = "What solution to add the project to.")]
	public DotnetSolution? Solution { get; set; }

	/// <summary>
	///
	/// </summary>
	[Parameter(Mandatory = true,
		Position = 1,
		HelpMessage = "What project to add to the solution.")]
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
		if(Solution == null)
		{
			throw new PSArgumentNullException(nameof(Solution));
		}

		var executor = ShellExecutor.Instance;
		var command = DetermineCommand(executor, Solution!, Project!);
		if (ShouldProcess(command.Target, command.ActionName))
		{
			var r = command.Execute() as ShellOperationResult;

			WriteDebug($"Exit code: {r!.ExitCode}");
			WriteDebug($"Output:{Environment.NewLine}{r.OutputText}");
			if (!string.IsNullOrWhiteSpace(r.ErrorText))
			{
				WriteDebug($"Error:{Environment.NewLine}{r.ErrorText}");
			}
			WriteObject(Solution);
		}
	}

	private CommandBase DetermineCommand(IShellExecutor executor,
		DotnetSolution solution,
		DotnetProject project)
	{
		return new AddProjectToSolutionCommand(executor, solution, project);
	}

	/// <inheritdoc />
	protected override void EndProcessing()
	{
		base.EndProcessing();
	}
}