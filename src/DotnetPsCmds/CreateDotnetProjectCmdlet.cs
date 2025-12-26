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
/// <param type="synopsis">Creates a new .NET project.</param>
/// <param type="description">Creates a new .NET project.</param>
/// </summary>
[Cmdlet(VerbsCommon.New, "DotnetProject", SupportsShouldProcess = true)]
[OutputType(typeof(DotnetProject))]
// ReSharper disable once UnusedType.Global
public class CreateDotnetProjectCmdlet : PSCmdlet
{
	/// <summary>
	/// The parameter to the <code>dotnet new sln --template</code> option
	/// </summary>
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

	/// <summary>
	/// Gets or sets the solution to which the project will be added.
	/// </summary>
	[Parameter(Mandatory = false,
		Position = 4,
		ValueFromPipeline = true,
		HelpMessage = "What solution to add the project to.")]
	// ReSharper disable once UnusedAutoPropertyAccessor.Global
	public DotnetSolution? Solution { get; set; }

	/// <inheritdoc />
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

	/// <inheritdoc />
	protected override void ProcessRecord()
	{
		var executor = ShellExecutor.Instance;
		Debug.Assert(supportedTemplateName != null);
		var createCommand = DetermineCreateCommand(executor, supportedTemplateName!);

		if (ShouldProcess(createCommand.Target, createCommand.ActionName))
		{
			#region to move somewhere else as one or more operations
			var createResult = createCommand.Execute() as ShellOperationResult;
			#endregion

			// TODO: Error if exit code is non-zero?
			if(createResult!.ExitCode != 0)
			{
				ThrowTerminatingError(errorRecord: new ErrorRecord(exception: new InvalidOperationException(
						$"Failed to create project. Exit code {createResult.ExitCode}{Environment.NewLine}Error output:{Environment.NewLine}{createResult.ErrorText}"),
					errorId: "CreateProjectFailed",
					errorCategory: ErrorCategory.OperationStopped,
					targetObject: null));
			}
			WriteDebug($"Exit code: {createResult.ExitCode}");
			WriteDebug($"Output:{Environment.NewLine}{createResult.OutputText}");
			if (!string.IsNullOrWhiteSpace(createResult.ErrorText))
			{
				WriteDebug($"Error:{Environment.NewLine}{createResult.ErrorText}");
			}
			var createdProject = new DotnetProject(OutputDirectory, OutputName);
			if (Solution != null)
			{
				var addCommand = DetermineAddCommand(executor, Solution, createdProject);
				if (ShouldProcess(addCommand.Target, addCommand.ActionName))
				{
					var addResult = addCommand.Execute() as ShellOperationResult;
					if(addResult!.ExitCode != 0)
					{
						ThrowTerminatingError(errorRecord: new ErrorRecord(exception: new InvalidOperationException(
								$"Failed to add project to solution. Exit code {addResult.ExitCode}{Environment.NewLine}Error output:{Environment.NewLine}{addResult.ErrorText}"),
							errorId: "AddProjectToSolutionFailed",
							errorCategory: ErrorCategory.OperationStopped,
							targetObject: null));
					}
					WriteDebug($"Exit code: {addResult.ExitCode}");
					WriteDebug($"Output:{Environment.NewLine}{addResult.OutputText}");
					if (!string.IsNullOrWhiteSpace(addResult.ErrorText))
					{
						WriteDebug($"Error:{Environment.NewLine}{addResult.ErrorText}");
					}
				}
			}

			WriteObject(createdProject);
		}
	}

	private CommandBase DetermineCreateCommand(IShellExecutor executor, SupportedProjectTemplateName template)
	{
		return ProjectTemplateNameMapping.CommandMap[template](
			executor,
			template,
			OutputDirectory!,
			OutputName!,
			frameworkName ?? SupportedFrameworkName.Net10);
	}

	private CommandBase DetermineAddCommand(IShellExecutor executor, DotnetSolution solution, DotnetProject project)
	{
		return new AddProjectToSolutionCommand(executor, solution, project);
	}


	private SupportedProjectTemplateName? supportedTemplateName;
	private SupportedFrameworkName? frameworkName;

	/// <inheritdoc />
	protected override void EndProcessing()
	{
	}
}