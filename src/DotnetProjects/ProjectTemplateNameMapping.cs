using System;
using System.Collections.Generic;

using Pri.Essentials.DotnetProjects.Commands;
using Pri.Essentials.DotnetProjects.Commands.Constants;

namespace Pri.Essentials.DotnetProjects;

/// <summary>
/// Provides a mapping between supported project template names and factory functions that create the corresponding
/// project command instances.
/// </summary>
/// <remarks>This class is intended to centralize the association of project template types with their respective
/// command creation logic. It enables consumers to retrieve a command factory for a given template, facilitating
/// consistent project creation workflows. The class is static and cannot be instantiated.</remarks>
public static class ProjectTemplateNameMapping
{
	/// <summary>
	/// A dictionary mapping supported project template names to factory functions that create the corresponding
	/// </summary>
	public static readonly Dictionary<SupportedProjectTemplateName, Func<IShellExecutor, SupportedProjectTemplateName, string, string, SupportedFrameworkName, CommandBase>>
		CommandMap = new()
		{
			{ SupportedProjectTemplateName.Blazor, (e,t,d,n,f) => new CreateProjectCommand (e,t,d,n,f) },
			{ SupportedProjectTemplateName.ClassLib, (e,t,d,n,f) => new CreateClassLibProjectCommand (e,d,n,f) },
			{ SupportedProjectTemplateName.Console, (e,t,d,n,f) => new CreateProjectCommand (e,t,d,n,f) },
			{ SupportedProjectTemplateName.WebApi, (e,t,d,n,f) => new CreateProjectCommand (e,t,d,n,f) },
			{ SupportedProjectTemplateName.WinForms, (e,t,d,n,f) => new CreateProjectCommand (e,t,d,n,f) },
			{ SupportedProjectTemplateName.Worker, (e,t,d,n,f) => new CreateProjectCommand (e,t,d,n,f) },
			{ SupportedProjectTemplateName.XUnit, (e,t,d,n,f) => new CreateXunitProjectCommand (e,d,n,f) },
			{ SupportedProjectTemplateName.XUnit3, (e,t,d,n,f) => new CreateXunit3ProjectCommand (e,d,n,f) },
		};
}