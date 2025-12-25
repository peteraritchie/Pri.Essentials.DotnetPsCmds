using System.Text.RegularExpressions;

using Pri.Essentials.Abstractions;
using Pri.Essentials.DotnetProjects;
using Pri.Essentials.DotnetProjects.Commands;
using Pri.Essentials.DotnetProjects.Commands.Constants;

namespace Tests;

public partial class CreateXunit3ProjectCommandShould
	: CommandTestingBase<CreateXunit3ProjectCommand>
{
	private readonly IFileSystem spyFileSystem = Substitute.For<IFileSystem>();

	public CreateXunit3ProjectCommandShould()
		: base(Substitute.For<IShellExecutor>())
	{
		spyExecutor
			.Execute(Arg.Do<string>(a => suppliedCommandLine = a))
			.Returns(new ShellResult(0, string.Empty, string.Empty));
		sut = InitializeSut(directory, spyExecutor, spyFileSystem);
	}

	private static CreateXunit3ProjectCommand InitializeSut(string projectDirectory, IShellExecutor executor, IFileSystem fileSystem)
	{
		return new CreateXunit3ProjectCommand(shellExecutor: executor,
			outputDirectory: projectDirectory,
			outputName: "Tests",
			frameworkName: SupportedFrameworkName.Net10,
			fileSystem: fileSystem);
	}

	[Fact]
	void HaveCorrectCommand()
	{
		var result = sut.Execute();
		Assert.True(result.IsSuccessful);
		spyExecutor.Received().Execute(Arg.Any<string>());
		Assert.NotNull(suppliedCommandLine);
		var match = GroupNameAndDirRegex().Match(suppliedCommandLine);
		Assert.True(match.Success);
		Assert.Equal("dotnet new", match.Groups[1].Value);
		Assert.Equal("xunit3", match.Groups[2].Value);
	}

	[Fact]
	void HaveCorrectArguments()
	{
		var result = sut.Execute();
		Assert.True(result.IsSuccessful);
		spyExecutor.Received().Execute(Arg.Any<string>());
		Assert.NotNull(suppliedCommandLine);
		var match = GroupNameAndDirRegex().Match(suppliedCommandLine);
		Assert.True(match.Success);
		Assert.Equal(directory, match.Groups[3].Value);
		Assert.Equal("Tests", match.Groups[4].Value);
		Assert.Equal("net10.0", match.Groups[5].Value);
	}

	[Fact]
	void HaveDeletedExtraFile()
	{
		spyFileSystem.Exists(Arg.Any<string>()).Returns(true);
		var result = sut.Execute();
		Assert.True(result.IsSuccessful);
		spyFileSystem.Received().Exists(Path.Combine(directory, "UnitTest1.cs"));
		spyFileSystem.Received().DeleteFile(Path.Combine(directory, "UnitTest1.cs"));
	}

	[Fact]
	void HaveIgnoredExceptionWhenDeletedExtraFile()
	{
		spyFileSystem
			.Exists(Arg.Any<string>())
			.Returns(true);
		spyFileSystem
			.When(x => x.DeleteFile(Arg.Any<string>()))
			.Do(x => throw new IOException());
		var result = sut.Execute();
		Assert.True(result.IsSuccessful);
		spyFileSystem
			.Received()
			.Exists(Path.Combine(directory, "UnitTest1.cs"));
		spyFileSystem
			.Received()
			.DeleteFile(Path.Combine(directory, "UnitTest1.cs"));
	}

	[Fact]
	void HaveNotDeletedExtraFileIfNonexistent()
	{
		spyFileSystem.Exists(Arg.Any<string>()).Returns(false);
		var result = sut.Execute();
		Assert.True(result.IsSuccessful);
		spyFileSystem.Received().Exists(Path.Combine(directory, "UnitTest1.cs"));
		spyFileSystem.DidNotReceive().DeleteFile(Path.Combine(directory, "UnitTest1.cs"));
	}

	[GeneratedRegex(@"^(\S+ \S+) (\S+) -o (\S+) -n (\S+) -f (\S+)$")]
	private static partial Regex GroupNameAndDirRegex();
}