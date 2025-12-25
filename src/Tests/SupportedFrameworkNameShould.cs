using Pri.Essentials.DotnetProjects.Commands.Constants;

namespace Tests;

public class SupportedFrameworkNameShould
{
	[Fact]
	void ParseCorrectly()
	{
		Assert.True(SupportedFrameworkName.TryParse("net8.0", out var result));
		Assert.Same(SupportedFrameworkName.Net8, result);
	}
}