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

	[Fact]
	void FindCorrectly()
	{
		Assert.True(SupportedFrameworkName.TryFind("net10.0", out var result));
		Assert.Same(SupportedFrameworkName.Net10, result);
	}
}