using System.Text.RegularExpressions;

using Pri.Essentials.DotnetProjects;
using Pri.Essentials.DotnetProjects.Commands;

namespace Tests;

public partial class AddClassToProjectCommandShould
	: CommandTestingBase<AddClassToProjectCommand>
{
	public AddClassToProjectCommandShould()
		: base(Substitute.For<IShellExecutor>())
	{
		spyExecutor
			.Execute(Arg.Do<string>(a => suppliedCommandLine = a))
			.Returns(new ShellResult(0, string.Empty, string.Empty));
		sut = InitializeSut(directory, spyExecutor);
	}

	private static AddClassToProjectCommand InitializeSut(string projectDirectory, IShellExecutor executor)
	{
		return new AddClassToProjectCommand(executor,
			new DotnetProject(
				projectDirectory,
				name: "testing"),
			"TheClass");
	}

	[Fact]
	void HaveCorrectLanguageParameter()
	{
		var result = sut.Execute();
		Assert.True(result.IsSuccessful);
		spyExecutor.Received().Execute(Arg.Any<string>());
		Assert.NotNull(suppliedCommandLine);
		Assert.Contains(" --language C#", suppliedCommandLine);
	}

	[Fact]
	void HaveCorrectClassFilePath()
	{
		var result = sut.Execute();
		Assert.True(result.IsSuccessful);
		spyExecutor.Received().Execute(Arg.Any<string>());
		Assert.NotNull(suppliedCommandLine);
		var match = GroupNameAndDirRegex().Match(suppliedCommandLine);
		Assert.True(match.Success);
		Assert.Equal(3, match.Groups.Count);
		Assert.Equal("TheClass", match.Groups[2].Value);
		Assert.Equal(directory, match.Groups[1].Value);
	}

	[Fact]
	void HaveCorrectClassFilePathWhenNamespaceIncluded()
	{
		var fakeProject = new DotnetProject(
			directory,
			name: "testing");
		var command = new AddClassToProjectCommand(spyExecutor, fakeProject, "Capability.TheClass");

		var result = command.Execute();
		Assert.True(result.IsSuccessful);
		spyExecutor.Received().Execute(Arg.Any<string>());
		Assert.NotNull(suppliedCommandLine);
		var match = GroupNameAndDirRegex().Match(suppliedCommandLine);
		Assert.True(match.Success);
		Assert.Equal(3, match.Groups.Count);
		Assert.Equal("TheClass", match.Groups[2].Value);
		Assert.Equal(Path.Combine(directory, "Capability"), match.Groups[1].Value);
	}

	[GeneratedRegex("^.+-o (.+) -n (.+) --language C#$")]
	private static partial Regex GroupNameAndDirRegex();
}