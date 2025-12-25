using System;
using System.Diagnostics;
using System.IO;
using System.Management.Automation;

using Pri.Essentials.DotnetProjects;
using Pri.Essentials.DotnetProjects.Commands;
using Pri.Essentials.DotnetProjects.Commands.Constants;

namespace Pri.Essentials.DotnetPsCmds;

/// <summary>
/// Implements the New_DotnetProject Cmdlet.
/// </summary>
[Cmdlet(VerbsCommon.New, "DotnetProject", SupportsShouldProcess = true)]
[OutputType(typeof(DotnetProject))]
public class CreateDotnetProjectCmdlet : PSCmdlet
{
	[Parameter(Mandatory = true,
		Position = 0,
		HelpMessage = "What project template to use to generate the project.")]
	[ValidateSet("xunit", "xunit3", "console", "classlib", "blazor", "worker", "webapi", "winforms")]
	public string? TemplateName{ get; set; }

	/// <summary>The parameter to the <code>dotnet new sln --output</code> option</summary>
	[Parameter(Mandatory = false,
		Position = 1,
		HelpMessage =
			"What directory the project will be generated to. If omitted the current directory will be used.")]
	public string? OutputDirectory { get; set; }

	/// <summary>The parameter to the <code>dotnet new sln --name</code> option</summary>
	[Parameter(Mandatory = false,
		Position = 2,
		HelpMessage = "The name of the project file. If omitted, the parent directory name will be used.")]
	public string? OutputName{ get; set; }

	/// <summary>The parameter to the <code>dotnet new sln --name</code> option</summary>
	[Parameter(Mandatory = false,
		Position = 3,
		HelpMessage = "Which .NET framework version to use. If omitted, .NET 10 will be used.")]
	[ValidateSet("net10.0", "net9.0","net8.0","standard2.0","standard2.1")]
	public string? FrameworkName { get; set; } = SupportedFrameworkName.Net10;

	protected override void BeginProcessing()
	{
		if (string.IsNullOrWhiteSpace(FrameworkName) || !SupportedFrameworkName.TryParse(FrameworkName!, out frameworkName))
		{
			ThrowTerminatingError(errorRecord: new ErrorRecord(exception: new ArgumentException($"Invalid FrameworkName '{FrameworkName}'",
					paramName: nameof(FrameworkName)),
				errorId: "Invalid FrameworkName",
				errorCategory: ErrorCategory.CloseError,
				targetObject: null));
		}
		if (string.IsNullOrWhiteSpace(TemplateName) || !SupportedProjectTemplateName.TryParse(TemplateName!, out supportedTemplateName))
		{
			ThrowTerminatingError(errorRecord: new ErrorRecord(exception: new ArgumentException("Invalid TemplateName",
					paramName: nameof(TemplateName)),
				errorId: "Invalid TemplateName",
				errorCategory: ErrorCategory.CloseError,
				targetObject: null));
		}
		if (string.IsNullOrWhiteSpace(OutputDirectory))
		{
			OutputDirectory = SessionState.Path.CurrentFileSystemLocation.Path;
		}
		if (!Path.IsPathRooted(OutputDirectory))
		{
			OutputDirectory = Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, OutputDirectory!);
		}

		if (string.IsNullOrWhiteSpace(OutputName))
		{
			OutputName = Path.GetFileName(OutputDirectory);
		}
	}

	// This method will be called for each input received from the pipeline to this cmdlet; if no input is received, this method is not called
	protected override void ProcessRecord()
	{
		var executor = ShellExecutor.Instance;
		Debug.Assert(supportedTemplateName != null);
		var command = DetermineCommand(executor, supportedTemplateName!);

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
			WriteObject(new DotnetProject(OutputDirectory, OutputName));
		}
	}

	private CommandBase DetermineCommand(IShellExecutor executor, SupportedProjectTemplateName template)
	{
		return ProjectTemplateNameMapping.CommandMap[template](
			executor,
			template,
			OutputDirectory!,
			OutputName!,
			frameworkName ?? SupportedFrameworkName.Net10);
	}

	private SupportedProjectTemplateName? supportedTemplateName;
	private SupportedFrameworkName? frameworkName;

	// This method will be called once at the end of pipeline execution; if no input is received, this method is not called
	protected override void EndProcessing()
	{
	}
}