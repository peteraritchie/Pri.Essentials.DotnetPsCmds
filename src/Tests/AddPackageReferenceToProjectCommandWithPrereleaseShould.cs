using System.Text.RegularExpressions;

using Pri.Essentials.DotnetProjects;
using Pri.Essentials.DotnetProjects.Commands;

namespace Tests;

public partial class AddPackageReferenceToProjectCommandWithPrereleaseShould
	: CommandTestingBase<AddPackageReferenceToProjectCommand>
{
	public AddPackageReferenceToProjectCommandWithPrereleaseShould()
		: base(Substitute.For<IShellExecutor>())
	{
		spyExecutor
			.Execute(Arg.Do<string>(a => suppliedCommandLine = a))
			.Returns(new ShellResult(0, string.Empty, string.Empty));
		var fakeProject = new DotnetProject(
			Path.Combine(directory, "testing"),
			name: "testing");
		sut = new AddPackageReferenceToProjectCommand(spyExecutor, fakeProject, "NSubstitute")
		{
			IsPrerelease = true
		};
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
		Assert.Equal("--prerelease",  match.Groups[5].Value);
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
	}

	[GeneratedRegex(@"^(\S+ \S+) (\S+) (\S+) (\S+) (\S+)$")]
	private static partial Regex GroupNameAndDirRegex();
}