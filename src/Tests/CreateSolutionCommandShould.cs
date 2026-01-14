using System.Text.RegularExpressions;

using Pri.Essentials.DotnetProjects;
using Pri.Essentials.DotnetProjects.Commands;

namespace Tests;

public partial class CreateSolutionCommandShould
	: CommandTestingBase<CreateSolutionFileCommand>
{
	public CreateSolutionCommandShould()
		: base(Substitute.For<IShellExecutor>())
	{
		spyExecutor
			.Execute(Arg.Do<string>(a => suppliedCommandLine = a))
			.Returns(new ShellResult(0, string.Empty, string.Empty));
		sut = InitializeSut(directory, spyExecutor);
	}

	private static CreateSolutionFileCommand InitializeSut(string solutionDirectory, IShellExecutor shellExecutor)
	{
		return new CreateSolutionFileCommand(shellExecutor: shellExecutor,
			outputDirectory: solutionDirectory,
			outputName: "MySolution");
	}

	[Fact]
	void HaveNotNullTarget()
	{
		Assert.NotNull(sut!.Target);
	}

	[Fact]
	void HaveCorrectCommand()
	{
		var result = sut!.Execute();
		Assert.True(result.IsSuccessful);
		spyExecutor.Received().Execute(Arg.Any<string>());
		Assert.NotNull(suppliedCommandLine);
		var match = GroupNameAndDirRegex().Match(suppliedCommandLine);
		Assert.True(match.Success);
		Assert.Equal("dotnet new", match.Groups[1].Value);
		Assert.Equal("sln", match.Groups[2].Value);
	}

	[Fact]
	void HaveCorrectArguments()
	{
		var result = sut!.Execute();
		Assert.True(result.IsSuccessful);
		spyExecutor.Received().Execute(Arg.Any<string>());
		Assert.NotNull(suppliedCommandLine);
		var match = GroupNameAndDirRegex().Match(suppliedCommandLine);
		Assert.True(match.Success);
		Assert.Equal(directory, match.Groups[3].Value);
		Assert.Equal("MySolution", match.Groups[4].Value);
	}

	[GeneratedRegex(@"^(\S+ \S+) (\S+) -f sln -o (\S+) -n (\S+)$")]
	private static partial Regex GroupNameAndDirRegex();
}