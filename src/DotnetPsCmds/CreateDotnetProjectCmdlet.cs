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
[Cmdlet(VerbsCommon.New, "DotnetProject", SupportsShouldProcess = true,
	DefaultParameterSetName = "WithoutSolution")]
[OutputType(typeof(DotnetProject))]
// ReSharper disable once UnusedType.Global
public class CreateDotnetProjectCmdlet : PSCmdlet
{
	/// <summary>
	/// The parameter to the <code>dotnet new sln --template</code> option
	/// </summary>
	[Parameter(Mandatory = true, Position = 0,
		HelpMessage = "What project template to use "
		+ "to generate the project.")]
	[ValidateSet("xunit", "xunit3", "console", "classlib", "blazor",
		"worker", "webapi", "winforms")]
	public string? TemplateName{ get; set; }

	/// <summary>
	/// The parameter to the <code>dotnet new sln --output</code> option
	/// </summary>
	[Parameter(Mandatory = false, Position = 1,
		HelpMessage =
			"What directory the project will be generated to. If omitted"
		+ " the current directory will be used.")]
	[Alias("Path", "o")]
	public string? OutputDirectory { get; set; }

	/// <summary>
	/// The parameter to the <code>dotnet new sln --name</code> option
	/// </summary>
	[Parameter(Mandatory = false,
		Position = 2,
		HelpMessage = "The name of the project file. If omitted, the"
		+ " parent directory name will be used.")]
	[Alias("Name", "n")]
	public string? OutputName{ get; set; }

	/// <summary>
	/// The parameter to the <code>dotnet new sln --name</code> option
	/// </summary>
	/// <remarks>TODO: set default based on global.json if present</remarks>
	[Parameter(Mandatory = false,
		Position = 3,
		HelpMessage = "Which .NET framework version to use. If omitted, "
		+ ".NET 10 will be used.")]
	[ValidateSet("net10.0", "net9.0","net8.0","standard2.0","standard2.1")]
	public string? FrameworkName { get; set; } = SupportedFrameworkName.Net10;

	/// <summary>
	/// Gets or sets the solution to which the project will be added.
	/// </summary>
	[Parameter(Mandatory = false,
		Position = 4,
		ValueFromPipeline = true,
		HelpMessage = "What solution to add the project to.")]
	[Parameter(Mandatory = true, ParameterSetName = "WithSolution",
		Position = 4,
		ValueFromPipeline = true)]
	// ReSharper disable once UnusedAutoPropertyAccessor.Global
	public DotnetSolution? Solution { get; set; }

	/// <summary>
	/// Gets or sets the solution folder to which the project will be added.
	/// </summary>
	[Parameter(Mandatory = false, ParameterSetName = "WithSolution",
		HelpMessage = "What solution folder to add the project to.")]
	// ReSharper disable once UnusedAutoPropertyAccessor.Global
	public string? SolutionFolder { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether to generate XML documentation
	/// </summary>
	[Parameter(Mandatory = false,
		HelpMessage = "Switch parameter to indicate whether to "
		+ "generate documentation file.")]
	public SwitchParameter ShouldGenerateDocumentationFile { get; set; }

	/// <inheritdoc />
	protected override void BeginProcessing()
	{
		if (string.IsNullOrWhiteSpace(FrameworkName)
			|| !SupportedFrameworkName.TryParse(FrameworkName!,
				out frameworkName))
		{
			ThrowTerminatingError(
				new ErrorRecord(
					new ArgumentException(
						$"Invalid FrameworkName '{FrameworkName}'",
					nameof(FrameworkName)),
				errorId: "Invalid FrameworkName",
				errorCategory: ErrorCategory.CloseError,
				targetObject: null));
		}
		if (string.IsNullOrWhiteSpace(TemplateName)
			|| !SupportedProjectTemplateName.TryParse(TemplateName!,
				out supportedTemplateName))
		{
			ThrowTerminatingError(new ErrorRecord(
				new ArgumentException("Invalid TemplateName",
					nameof(TemplateName)),
				errorId: "Invalid TemplateName",
				errorCategory: ErrorCategory.CloseError,
				targetObject: null));
		}
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
		if (this.ParameterSetName == "WithSolution"
			&& Solution == null)
		{
			ThrowTerminatingError(
				new ErrorRecord(
					new ArgumentException(
						$"Parameter Solution is required "
						+ $"when using parameter set '{this.ParameterSetName}'",
					nameof(Solution)),
				errorId: "Missing Solution Parameter",
				errorCategory: ErrorCategory.InvalidArgument,
				targetObject: null));
		}

		var executor = ShellExecutor.Instance;
		Debug.Assert(supportedTemplateName != null);
		var createCommand = DetermineCreateCommand(executor,
			supportedTemplateName!,
			ShouldGenerateDocumentationFile);

		if (ShouldProcess(createCommand.Target, createCommand.ActionName))
		{
			#region to move somewhere else as one or more operations
			var createResult = createCommand.Execute()
				as ShellOperationResult;
			#endregion

			if(createResult!.IsFailure)
			{
				ThrowTerminatingError(new ErrorRecord(
					new InvalidOperationException(
						$"Failed to create project. Exit code " +
						$"{createResult.ExitCode}{Environment.NewLine}" +
						$"Error output:" +
						$"{Environment.NewLine}{createResult.ErrorText}"),
					errorId: "CreateProjectFailed",
					errorCategory: ErrorCategory.OperationStopped,
					targetObject: null));
			}
			WriteDebug($"Operation: {createResult.OperationText}");
			WriteDebug($"Exit code: {createResult.ExitCode}");
			WriteDebug($"Output:{Environment.NewLine}"
				+ $"{createResult.OutputText}");
			if (!string.IsNullOrWhiteSpace(createResult.ErrorText))
			{
				WriteDebug($"Error:{Environment.NewLine}"
					+ $"{createResult.ErrorText}");
			}
			var createdProject = new DotnetProject(OutputDirectory,
				OutputName);
			if (Solution != null)
			{
				var addCommand = DetermineAddCommand(executor,
					Solution,
					createdProject,
					SolutionFolder);
				if (ShouldProcess(addCommand.Target, addCommand.ActionName))
				{
					var addResult = addCommand.Execute()
						as ShellOperationResult;
					if(addResult!.IsFailure)
					{
						ThrowTerminatingError(errorRecord: new ErrorRecord(
							exception: new InvalidOperationException(
								$"Failed to add project to solution. Exit code " +
								$"{addResult.ExitCode}{Environment.NewLine}" +
								$"Error output:" +
								$"{Environment.NewLine}{addResult.ErrorText}"),
							errorId: "AddProjectToSolutionFailed",
							errorCategory: ErrorCategory.OperationStopped,
							targetObject: null));
					}
					createdProject.Solution = Solution;
					WriteDebug($"Operation: {addResult.OperationText}");
					WriteDebug($"Exit code: {addResult.ExitCode}");
					WriteDebug($"Output:{Environment.NewLine}"
						+ $"{addResult.OutputText}");
					if (!string.IsNullOrWhiteSpace(addResult.ErrorText))
					{
						WriteDebug($"Error:{Environment.NewLine}"
							+ $"{addResult.ErrorText}");
					}
				}
			}

			WriteObject(createdProject);
		}
	}

	private CommandBase DetermineCreateCommand(IShellExecutor executor,
		SupportedProjectTemplateName template,
		SwitchParameter shouldGenerateDocumentationFile)
	{
		var createCommand = ProjectTemplateNameMapping.CommandMap[template](
			executor,
			template,
			OutputDirectory!,
			OutputName!,
			frameworkName ?? SupportedFrameworkName.Net10);

		createCommand.ShouldGenerateDocumentationFile
			= shouldGenerateDocumentationFile.IsPresent
				? shouldGenerateDocumentationFile
				: null;
		return createCommand;
	}

	private static CommandBase DetermineAddCommand(IShellExecutor executor,
		DotnetSolution solution,
		DotnetProject project,
		string? solutionFolder)
	{
		return new AddProjectToSolutionCommand(executor,
			solution,
			project,
			solutionFolder)
		{
			// TODO: other options?
		};
	}


	private SupportedProjectTemplateName? supportedTemplateName;
	private SupportedFrameworkName? frameworkName;

	/// <inheritdoc />
	protected override void EndProcessing()
	{
	}
}