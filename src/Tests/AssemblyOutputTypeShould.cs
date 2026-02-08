using Pri.Essentials.DotnetProjects.Commands.Constants;

namespace Tests;

public class AssemblyOutputTypeShould
{
	[Fact]
	void ParseCorrectly()
	{
		Assert.True(AssemblyOutputType.TryParse("exe", out var result));
		Assert.Same(AssemblyOutputType.Exe, result);
	}

	[Fact]
	void FindCorrectly()
	{
		Assert.True(AssemblyOutputType.TryFind("exe", out var result));
		Assert.Same(AssemblyOutputType.Exe, result);
	}
}