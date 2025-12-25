using System;
using System.Collections.Generic;

using Pri.Essentials.DotnetProjects.Commands;
using Pri.Essentials.DotnetProjects.Commands.Constants;

namespace Pri.Essentials.DotnetProjects;

public static class ProjectTemplateNameMapping
{
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