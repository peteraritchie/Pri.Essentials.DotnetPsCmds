using System.Text.RegularExpressions;

using Pri.Essentials.Abstractions;
using Pri.Essentials.DotnetProjects;
using Pri.Essentials.DotnetProjects.Commands;

namespace Tests;

public partial class AddProjectToSolutionCommandWithShouldGenerateDocumentationFileShould
	: CommandTestingBase<AddProjectToSolutionCommand>
{
	private readonly IFileSystem spyFileSystem = Substitute.For<IFileSystem>();
	public AddProjectToSolutionCommandWithShouldGenerateDocumentationFileShould()
		: base(Substitute.For<IShellExecutor>())
	{
		spyExecutor
			.Execute(Arg.Do<string>(a => suppliedCommandLine = a))
			.Returns(new ShellResult(0, string.Empty, string.Empty));
		var fakeSolution = new DotnetSolution(directory, "MyProduct");
		var fakeProject = new DotnetProject(
			Path.Combine(directory, "testing"),
			name: "testing");
		var memoryStream = new MemoryStream();
		memoryStream.Write("""
		                   <Project Sdk="Microsoft.NET.Sdk">
		                     <PropertyGroup>
		                       <TargetFramework>netstandard2.0</TargetFramework>
		                       <LangVersion>latest</LangVersion>
		                       <Nullable>enable</Nullable>
		                     </PropertyGroup>
		                   </Project>
		                   """u8.ToArray());
		memoryStream.Position = 0;

		spyFileSystem.FileOpen(path: Arg.Any<string>(),
				fileMode: Arg.Any<FileMode>(),
				fileAccess: Arg.Any<FileAccess>(),
				fileShare: Arg.Any<FileShare>())
			.Returns(memoryStream);
		sut = new AddProjectToSolutionCommand(spyExecutor,
			fakeSolution,
			fakeProject,
			solutionFolder: null,
			fileSystem: spyFileSystem)
		{
			ShouldGenerateDocumentationFile = true
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
		Assert.Equal("dotnet sln",  match.Groups[1].Value);
		Assert.Equal("add",  match.Groups[3].Value);
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
		Assert.Equal(Path.ChangeExtension(Path.Combine(directory, "MyProduct"), ".sln"),  match.Groups[2].Value);
		Assert.Equal(Path.Combine(directory, "testing", "testing.csproj"),  match.Groups[4].Value);
	}

	[GeneratedRegex(@"^(\S+ \S+) (\S+) (\S+) (\S+) --in-root$")]
	private static partial Regex GroupNameAndDirRegex();
}