using System;
using System.IO;
using System.Management.Automation;

using Pri.Essentials.DotnetProjects;
using Pri.Essentials.DotnetProjects.Commands;

namespace Pri.Essentials.DotnetPsCmds;

/// <summary>
/// Implements the New_DotnetSolution Cmdlet.
/// <para type="synopsis">Creates a new .NET solution.</para>
/// </summary>
[Cmdlet(VerbsCommon.New, "DotnetSolution", SupportsShouldProcess = true)]
[OutputType(typeof(DotnetSolution))]
public class CreateDotnetSolutionCmdlet : PSCmdlet
{
	/// <summary>
	/// The parameter to the <code>dotnet new sln --output</code> option
	/// </summary>
	[Parameter(Mandatory = false, Position = 0,
		HelpMessage = "What directory the project will be generated to. "
		+ "If omitted the current directory will be used.")]
	[Alias("Path", "o")]
	public string? OutputDirectory { get; set; }

	/// <summary>
	/// The parameter to the <code>dotnet new sln --name</code> option
	/// </summary>
	[Parameter(Mandatory = false, Position = 1,
		HelpMessage = "The name of the project file. If omitted, "
		+ "the parent directory name will be used.")]
	[Alias("Name", "n")]
	public string? OutputName { get; set; }

	/// <inheritdoc />
	protected override void BeginProcessing()
	{
		if (string.IsNullOrWhiteSpace(OutputDirectory))
		{
			OutputDirectory
				= SessionState.Path.CurrentFileSystemLocation.Path;
		}

		if (!Path.IsPathRooted(OutputDirectory))
		{
			OutputDirectory = Path.Combine(
				SessionState.Path.CurrentFileSystemLocation.Path,
				OutputDirectory!);
		}

		if (string.IsNullOrWhiteSpace(OutputName))
		{
			OutputName = Path.GetFileName(OutputDirectory);
		}
	}

	/// <inheritdoc />
	protected override void ProcessRecord()
	{
		var executor = ShellExecutor.Instance;
		var command = new CreateSolutionCommand(executor,
			OutputDirectory!,
			OutputName!);

		if (ShouldProcess(command.Target, command.ActionName))
		{
			#region to move somewhere else as one or more operations
			var createResult = command.Execute() as ShellOperationResult;
			#endregion
			if (createResult!.IsFailure)
			{
				ThrowTerminatingError(new ErrorRecord(
					new InvalidOperationException(
						$"Failed to create solution. Exit code "
						+ $"{createResult.ExitCode}{Environment.NewLine}"
						+ $"Error output:"
						+ $"{Environment.NewLine}{createResult.ErrorText}"),
					errorId: "CreateSolutionFailed",
					errorCategory: ErrorCategory.OperationStopped,
					targetObject: null));
			}

			WriteDebug($"Operation: {createResult!.OperationText}");
			WriteDebug($"Exit code: {createResult.ExitCode}");
			WriteDebug($"Output:{Environment.NewLine}"
				+ $"{createResult.OutputText}");
			if (!string.IsNullOrWhiteSpace(createResult.ErrorText))
			{
				WriteDebug($"Error:{Environment.NewLine}"
					+ $"{createResult.ErrorText}");
			}
			WriteObject(new DotnetSolution(OutputDirectory, OutputName));
		}
	}

	/// <summary>
	/// Performs end-of-processing tasks after all pipeline input has been
	/// received and processed.
	/// </summary>
	/// <remarks>
	/// This method is called once at the end of pipeline execution. It is
	/// not invoked if no input is received by the cmdlet. Override this
	/// method to implement any cleanup or finalization logic that should
	/// occur after all input has been processed.
	/// </remarks>
	protected override void EndProcessing()
	{
	}
}