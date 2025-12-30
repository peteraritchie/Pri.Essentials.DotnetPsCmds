using System.Text.RegularExpressions;

using Pri.Essentials.DotnetProjects;
using Pri.Essentials.DotnetProjects.Commands;

namespace Tests;

public partial class AddPackageReferenceToProjectCommandWithVersionShould
	: CommandTestingBase<AddPackageReferenceToProjectCommand>
{
	public AddPackageReferenceToProjectCommandWithVersionShould()
		: base(Substitute.For<IShellExecutor>())
	{
		spyExecutor
			.Execute(Arg.Do<string>(a => suppliedCommandLine = a))
			.Returns(new ShellResult(0, string.Empty, string.Empty));
		var fakeProject = new DotnetProject(
			Path.Combine(directory, "testing"),
			name: "testing");
		sut = new AddPackageReferenceToProjectCommand(spyExecutor, fakeProject, "NSubstitute", "5.3.0");
	}

	[Fact]
	public void HaveCorrectCommand()
	{
		var result = sut!.Execute();
		Assert.True(result.IsSuccessful);
		spyExecutor.Received().Execute(Arg.Any<string>());
		Assert.NotNull(suppliedCommandLine);
		var match = GroupNameAndDirRegex().Match(suppliedCommandLine);
		Assert.True(match.Success);
		Assert.Equal("dotnet add",  match.Groups[1].Value);
		Assert.Equal("package",  match.Groups[3].Value);
		Assert.Equal("-v",  match.Groups[5].Value);
	}

	[Fact]
	public void HaveCorrectArguments()
	{
		var result = sut!.Execute();
		Assert.True(result.IsSuccessful);
		spyExecutor.Received().Execute(Arg.Any<string>());
		Assert.NotNull(suppliedCommandLine);
		var match = GroupNameAndDirRegex().Match(suppliedCommandLine);
		Assert.True(match.Success);
		Assert.Equal("NSubstitute", match.Groups[4].Value);
		Assert.Equal(Path.Combine(directory, "testing", "testing.csproj"),  match.Groups[2].Value);
		Assert.Equal("5.3.0", match.Groups[6].Value);
	}

	[GeneratedRegex(@"^(\S+ \S+) (\S+) (\S+) (\S+) (\S+) (\d+\.\d+.\d+)$")]
	private static partial Regex GroupNameAndDirRegex();
}