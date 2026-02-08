using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Text.RegularExpressions;

using Pri.Essentials.DotnetProjects;
using Pri.Essentials.DotnetProjects.Commands.Constants;

namespace Pri.Essentials.DotnetPsCmds;

/// <summary>
/// A service class that encapsulates support functionality needed when
/// executing the Edit-DotnetProject cmdlet
/// </summary>
internal static class EditDotnetProjectService
{
	/// <summary>
	/// A service method to process properties details
	/// </summary>
	/// <param name="cmdlet"></param>
	/// <param name="properties"></param>
	/// <param name="projectConfigurationService"></param>
	/// <returns></returns>
	public static Dictionary<string, Action> ProcessPropertyRequests(Cmdlet cmdlet, Dictionary<string, object> properties,
		VisualStudioProjectConfigurationService projectConfigurationService)
	{
		var actions = new Dictionary<string, Action>();
		foreach ((string? key, object? value) in properties)
		{
			switch (key)
			{
				case ProjectElementNames.AppendTargetFrameworkToOutputPath when (value is bool enable):
					AddActionToPerform(actions,
						key,
						() => projectConfigurationService.SetAppendTargetFrameworkToOutputPath(enable));
					break;
				case ProjectElementNames.AppendTargetFrameworkToOutputPath when (value is string valueText && bool.TryParse(valueText, out var enable)):
					AddActionToPerform(actions,
						key,
						()=> projectConfigurationService.SetAppendTargetFrameworkToOutputPath(enable));
					break;
				case ProjectElementNames.AppendTargetFrameworkToOutputPath:
					cmdlet.WriteWarning($"The value '{value}' is not supported for property {key}.");
					break;
				case ProjectElementNames.AssemblyName when (value is string valueText):
					AddActionToPerform(actions,
						key,
						() => projectConfigurationService.SetAssemblyName(valueText));
					break;
				case ProjectElementNames.GenerateDocumentationFile when (value is bool enable):
					AddActionToPerform(actions,
						key,
						() => projectConfigurationService.SetGenerateDocumentationFile(enable));
					break;
				case ProjectElementNames.GenerateDocumentationFile when (value is string valueText && bool.TryParse(valueText, out var enable)):
					AddActionToPerform(actions,
						key,
						() => projectConfigurationService.SetGenerateDocumentationFile(enable));
					break;
				case ProjectElementNames.GenerateDocumentationFile:
					cmdlet.WriteWarning($"The value '{value}' is not supported for property {key}.");
					break;
				case ProjectElementNames.IntermediateOutputPath when (value is string valueText):
					AddActionToPerform(actions,
						key,
						() => projectConfigurationService.SetIntermediateOutputPath(valueText));
					break;
				case ProjectElementNames.LangVersion when (value is string valueText):
					AddActionToPerform(actions,
						key,
						() => projectConfigurationService.SetLangVersion(valueText));
					break;
				case ProjectElementNames.Nullable when (value is bool enable):
					AddActionToPerform(actions,
						key,
						() => projectConfigurationService.SetNullable(enable));
					break;
				case ProjectElementNames.Nullable when (value is string valueText && bool.TryParse(valueText, out var enable)):
					AddActionToPerform(actions,
						key,
						() => projectConfigurationService.SetNullable(enable));
					break;
				case ProjectElementNames.Nullable when (value is string valueText && Regex.Match(valueText, @"\s*(disable|enable)\s*").Success):
					AddActionToPerform(actions,
						key,
						() => projectConfigurationService.SetNullable(valueText == "enable"));
					break;
				case ProjectElementNames.Nullable:
					cmdlet.WriteWarning($"The value '{value}' is not supported for property {key}.");
					break;
				case ProjectElementNames.OutputPath when (value is string valueText):
					AddActionToPerform(actions,
						key,
						() => projectConfigurationService.SetOutputPath(valueText));
					break;
				case ProjectElementNames.OutputType when (value is string valueText && AssemblyOutputType.TryFind(valueText, out var outputType)):
					AddActionToPerform(actions,
						key,
						() => projectConfigurationService.SetOutputType(outputType));
					break;
				case ProjectElementNames.OutputType:
					cmdlet.WriteWarning($"The value '{value}' is not supported for property {key}.");
					break;
				case ProjectElementNames.RestorePackagesWithLockFile when (value is bool enable):
					AddActionToPerform(actions,
						key,
						() => projectConfigurationService.SetRestorePackagesWithLockFile(enable));
					break;
				case ProjectElementNames.RestorePackagesWithLockFile when (value is string valueText && bool.TryParse(valueText, out var enable)):
					AddActionToPerform(actions,
						key,
						() => projectConfigurationService.SetRestorePackagesWithLockFile(enable));
					break;
				case ProjectElementNames.RestorePackagesWithLockFile:
					cmdlet.WriteWarning($"The value '{value}' is not supported for property {key}.");
					break;
				case ProjectElementNames.TargetFramework
					when (value is string valueText
					      && SupportedFrameworkName.TryFind(valueText, out var name)):
					AddActionToPerform(actions,
						key,
						() => projectConfigurationService.SetTargetFramework(name));
					break;
				case ProjectElementNames.Version when (value is string valueText):
					AddActionToPerform(actions,
						key,
						() => projectConfigurationService.SetVersion(valueText));
					break;
				case ProjectElementNames.VersionPrefix when (value is string valueText):
					AddActionToPerform(actions,
						key,
						() => projectConfigurationService.SetVersionPrefix(valueText));
					break;
				case ProjectElementNames.VersionSuffix when (value is string valueText):
					AddActionToPerform(actions,
						key,
						() => projectConfigurationService.SetVersionSuffix(valueText));
					break;
				default:
					cmdlet.WriteWarning($"Property '{key}' or value '{value}' is not supported.");
					break;
			}
		}

		return actions;

		static void AddActionToPerform(IDictionary<string, Action> actionsToPerform,
			string text,
			Action action)
		{
			actionsToPerform[text] = action;
		}
	}

	/// <summary>
	/// A service method to determine the description displayed for the action
	/// based on the properties passed in.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="internalsVisibleTo"></param>
	/// <param name="properties"></param>
	/// <returns></returns>
	public static StringBuilder DetermineActionDescription<T>(string[]? internalsVisibleTo, IReadOnlyDictionary<string, T> properties)
	{
		var actionDescriptionBuilder = new StringBuilder($"Add/Update {properties.Count} properties");
		if (internalsVisibleTo is not null && internalsVisibleTo.Any())
		{
			actionDescriptionBuilder.Append($" and{string.Join(",", internalsVisibleTo.Select(e => $" add InternalsVisibleTo {e}"))}");
		}
		actionDescriptionBuilder.Append('.');
		return actionDescriptionBuilder;
	}

	/// <summary>
	/// A service method to determine the full path of the project
	/// based on the current session directory and provided project or path.
	/// </summary>
	/// <param name="project"></param>
	/// <param name="path"></param>
	/// <param name="sessionState"></param>
	/// <returns></returns>
	public static string DetermineProjectFileFullPath(DotnetProject? project, string? path, SessionState sessionState)
	{
		var fullPath = project != null ? project.FullPath
			: path!;
		if (string.IsNullOrWhiteSpace(fullPath))
		{
			fullPath = sessionState.Path.CurrentFileSystemLocation.Path;
		}
		if (!System.IO.Path.IsPathRooted(fullPath))
		{
			fullPath = System.IO.Path.Combine(
				sessionState.Path.CurrentFileSystemLocation.Path,
				fullPath);
		}

		return fullPath;
	}
}