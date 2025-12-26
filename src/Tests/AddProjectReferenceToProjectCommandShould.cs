using System.Text.RegularExpressions;

using Pri.Essentials.DotnetProjects;
using Pri.Essentials.DotnetProjects.Commands;

namespace Tests;

/// <remarks>dotnet add reference --project C:\Users\peter\AppData\Local\Temp\testing\Tests\Tests.csproj C:\Users\peter\AppData\Local\Temp\testing\Library\Library.csproj</remarks>
public partial class AddProjectReferenceToProjectCommandShould
	: CommandTestingBase<AddProjectReferenceToProjectCommand>
{
	public AddProjectReferenceToProjectCommandShould()
		:base(Substitute.For<IShellExecutor>())
	{
		spyExecutor
			.Execute(Arg.Do<string>(a => suppliedCommandLine = a))
			.Returns(new ShellResult(0, string.Empty, string.Empty));
		var targetProject = new DotnetProject(Path.Combine(directory, "Tests"), "Tests");
		var referencedProject = new DotnetProject(Path.Combine(directory, "Library"), "Library");
		sut = new AddProjectReferenceToProjectCommand(shellExecutor: spyExecutor,
			targetProject: targetProject,
			referencedProject: referencedProject);
	}

	[Fact]
	void HaveCorrectDotnetCommand()
	{
		var result = sut!.Execute();
		Assert.True(result.IsSuccessful);
		spyExecutor.Received().Execute(Arg.Any<string>());

		Assert.NotNull(suppliedCommandLine);
		var match = ParseCommandArgumentsRegex().Match(suppliedCommandLine);
		Assert.True(match.Success);
		Assert.Equal("dotnet add reference", match.Groups[1].Value);
	}

	[Fact]
	void HaveCorrectDirectories()
	{
		var result = sut!.Execute();
		Assert.True(result.IsSuccessful);
		spyExecutor.Received().Execute(Arg.Any<string>());

		Assert.NotNull(suppliedCommandLine);
		var match = ParseCommandArgumentsRegex().Match(suppliedCommandLine);
		Assert.True(match.Success);
		Assert.Equal(Path.Combine(directory, "Tests", "Tests.csproj"), match.Groups[2].Value);
		Assert.Equal(Path.Combine(directory, "Library", "Library.csproj"), match.Groups[3].Value);
	}

	[GeneratedRegex(@"^(\w+ \w+ \w+) --project (\S+) (\S+)$")]
	private static partial Regex ParseCommandArgumentsRegex();
}