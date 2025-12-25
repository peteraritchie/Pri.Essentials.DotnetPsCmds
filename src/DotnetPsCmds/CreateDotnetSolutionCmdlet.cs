using System;
using System.IO;
using System.Management.Automation;

using Pri.Essentials.DotnetProjects;
using Pri.Essentials.DotnetProjects.Commands;

namespace Pri.Essentials.DotnetPsCmds;

/// <summary>
/// Implements the New_DotnetSolution Cmdlet.
/// </summary>
[Cmdlet(VerbsCommon.New, "DotnetSolution", SupportsShouldProcess = true)]
[OutputType(typeof(DotnetSolution))]
public class CreateDotnetSolutionCmdlet : PSCmdlet
{
    /// <summary>The parameter to the <code>dotnet new sln --output</code> option</summary>
    [Parameter(Mandatory=false)]
    public string? OutputDirectory { get; set; }

    /// <summary>The parameter to the <code>dotnet new sln --name</code> option</summary>
    [Parameter(Mandatory = false)]
    public string? OutputName{ get; set; }

    protected override void BeginProcessing()
    {
        if (string.IsNullOrWhiteSpace(OutputDirectory)) OutputDirectory = SessionState.Path.CurrentFileSystemLocation.Path;
        if (string.IsNullOrWhiteSpace(OutputName)) OutputName = Path.GetFileName(OutputDirectory);
    }

    // This method will be called for each input received from the pipeline to this cmdlet; if no input is received, this method is not called
    protected override void ProcessRecord()
    {
	    var executor = ShellExecutor.Instance;
		var command = new CreateSolutionCommand(executor, OutputDirectory!, OutputName!);

		if (ShouldProcess(command.Target, command.ActionName))
        {
            #region to move somewhere else as one or more operations
			var r = command.Execute() as ShellOperationResult;
            #endregion

            WriteDebug($"Exit code: {r!.ExitCode}");
            WriteDebug($"Output:{Environment.NewLine}{r.OutputText}");
            if (!string.IsNullOrWhiteSpace(r.ErrorText))
            {
                WriteDebug($"Error:{Environment.NewLine}{r.ErrorText}");
            }
            WriteObject(new DotnetSolution(OutputDirectory, OutputName));
        }
    }

    // This method will be called once at the end of pipeline execution; if no input is received, this method is not called
    protected override void EndProcessing()
    {
    }
}