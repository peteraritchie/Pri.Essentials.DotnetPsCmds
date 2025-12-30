using System.Text.RegularExpressions;

using Pri.Essentials.Abstractions;
using Pri.Essentials.DotnetProjects;
using Pri.Essentials.DotnetProjects.Commands;
using Pri.Essentials.DotnetProjects.Commands.Constants;

namespace Tests;

public partial class CreateClassLibProjectCommandShould
	: CommandTestingBase<CreateClassLibProjectCommand>
{
	public CreateClassLibProjectCommandShould()
		: base(Substitute.For<IShellExecutor>())
	{
		spyExecutor
			.Execute(Arg.Do<string>(a => suppliedCommandLine = a))
			.Returns(new ShellResult(0, string.Empty, string.Empty));
		IFileSystem mockFileSystem = Substitute.For<IFileSystem>();
		mockFileSystem
			.Exists(Arg.Is(Path.Combine(directory, "Class1.cs")))
			.Returns(true);
		mockFileSystem
			.When(x => x.DeleteFile(Arg.Is<string>(a => a != Path.Combine(directory, "Class1.cs"))))
			.Do(_ => throw new IOException());
		sut = InitializeSut(directory, spyExecutor, mockFileSystem);
	}

	private static CreateClassLibProjectCommand InitializeSut(string projectDirectory,
		IShellExecutor shellExecutor,
		IFileSystem mockFileSystem)
	{
		return new CreateClassLibProjectCommand(shellExecutor: shellExecutor,
			outputDirectory: projectDirectory,
			outputName: "Library",
			frameworkName: SupportedFrameworkName.Net9,
			mockFileSystem);
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
		Assert.Equal("classlib", match.Groups[2].Value);
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
		Assert.Equal("Library", match.Groups[4].Value);
		Assert.Equal("net9.0", match.Groups[5].Value);
	}

	[GeneratedRegex(@"^(\S+ \S+) (\S+) -o (\S+) -n (\S+) -f (\S+)$")]
	private static partial Regex GroupNameAndDirRegex();
}