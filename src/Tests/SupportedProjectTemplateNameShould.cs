using Pri.Essentials.DotnetProjects.Commands.Constants;

namespace Tests;

public class SupportedProjectTemplateNameShould
{
	[Fact]
	void ParseCorrectly()
	{
		Assert.True(SupportedProjectTemplateName.TryParse("blazor", out var result));
		Assert.Same(SupportedProjectTemplateName.Blazor, result);
	}
}