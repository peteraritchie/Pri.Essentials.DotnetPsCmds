using System;
using System.Management.Automation;

using Pri.Essentials.DotnetProjects;
using Pri.Essentials.DotnetProjects.Commands;

namespace Pri.Essentials.DotnetPsCmds;

[Cmdlet(VerbsCommon.Add, nameof(DotnetProject), SupportsShouldProcess = true)]
[OutputType(typeof(DotnetSolution))]
public class AddDotnetProjectCmdlet : PSCmdlet
{
	[Parameter(Mandatory = true,
		Position = 0,
		ValueFromPipeline = true,
		HelpMessage = "What solution to add the project to.")]
	public DotnetSolution Solution { get; set; }

	[Parameter(Mandatory = true,
		Position = 1,
		HelpMessage = "What project to add to the solution.")]
	public DotnetProject Project { get; set; }

	protected override void BeginProcessing()
	{
		// TODO: parameter checks?
		base.BeginProcessing();
	}
	protected override void ProcessRecord()
	{
		var executor = ShellExecutor.Instance;
		var command = DetermineCommand(executor, Solution, Project);
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

	private CommandBase DetermineCommand(IShellExecutor executor, DotnetSolution solution, DotnetProject project)
	{
		return new AddProjectToSolutionCommand(executor, solution, project);
	}

	protected override void EndProcessing()
	{
		base.EndProcessing();
	}
}