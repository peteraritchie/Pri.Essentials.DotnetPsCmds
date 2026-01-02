using System.Text.RegularExpressions;

using Pri.Essentials.Abstractions;
using Pri.Essentials.DotnetProjects;
using Pri.Essentials.DotnetProjects.Commands;
using Pri.Essentials.DotnetProjects.Commands.Constants;

using Tests.Mocks;

namespace Tests;

public partial class CreateClassLibProjectCommandWithShouldGenerateDocumentationFileShould
	: CommandTestingBase<CreateClassLibProjectCommand>
{
	private readonly SpyMemoryStream memoryStream = new();

	public CreateClassLibProjectCommandWithShouldGenerateDocumentationFileShould()
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
		mockFileSystem.FileOpen(path: Arg.Any<string>(),
				fileMode: Arg.Any<FileMode>(),
				fileAccess: Arg.Any<FileAccess>(),
				fileShare: Arg.Any<FileShare>())
			.Returns(memoryStream);

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
			mockFileSystem)
		{
			ShouldGenerateDocumentationFile = true
		};
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

	[Fact]
	public void HaveCorrectContent()
	{
		var result = sut!.Execute();

		// assert that it is closed
		Assert.False(memoryStream.CanRead);

		Assert.True(result.IsSuccessful);
		Assert.Contains("<GenerateDocumentationFile>true</GenerateDocumentationFile>", memoryStream.ToString());
	}

	[GeneratedRegex(@"^(\S+ \S+) (\S+) -o (\S+) -n (\S+) -f (\S+)$")]
	private static partial Regex GroupNameAndDirRegex();
}