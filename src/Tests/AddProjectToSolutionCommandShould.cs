using System.Text.RegularExpressions;

using Pri.Essentials.DotnetProjects;
using Pri.Essentials.DotnetProjects.Commands;

namespace Tests;

public partial class AddProjectToSolutionCommandShould
	: CommandTestingBase<AddProjectToSolutionCommand>
{
	public AddProjectToSolutionCommandShould()
		: base(Substitute.For<IShellExecutor>())
	{
		spyExecutor
			.Execute(Arg.Do<string>(a => suppliedCommandLine = a))
			.Returns(new ShellResult(0, string.Empty, string.Empty));
		var fakeSolution = new DotnetSolution(directory);
		var fakeProject = new DotnetProject(
			Path.Combine(directory, "testing"),
			name: "testing");
		sut = new AddProjectToSolutionCommand(spyExecutor, fakeSolution, fakeProject);
	}

	[Fact]
	public void HaveCorrectCommand()
	{
		var result = sut.Execute();
		Assert.True(result.IsSuccessful);
		spyExecutor.Received().Execute(Arg.Any<string>());
		Assert.NotNull(suppliedCommandLine);
		var match = GroupNameAndDirRegex().Match(suppliedCommandLine);
		Assert.True(match.Success);
		Assert.Equal("dotnet sln",  match.Groups[1].Value);
		Assert.Equal("add",  match.Groups[3].Value);
	}

	[Fact]
	public void HaveCorrectArguments()
	{
		var result = sut.Execute();
		Assert.True(result.IsSuccessful);
		spyExecutor.Received().Execute(Arg.Any<string>());
		Assert.NotNull(suppliedCommandLine);
		var match = GroupNameAndDirRegex().Match(suppliedCommandLine);
		Assert.True(match.Success);
		Assert.Equal(Path.ChangeExtension(directory, ".sln"),  match.Groups[2].Value);
		Assert.Equal(Path.Combine(directory, "testing", "testing.csproj"),  match.Groups[4].Value);
	}

	[GeneratedRegex(@"^(\S+ \S+) (\S+) (\S+) (\S+)$")]
	private static partial Regex GroupNameAndDirRegex();
}