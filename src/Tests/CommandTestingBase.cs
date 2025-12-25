using Pri.Essentials.DotnetProjects;
using Pri.Essentials.DotnetProjects.Commands;

namespace Tests;

public abstract class CommandTestingBase<T>(IShellExecutor spyExecutor)
	where T : CommandBase
{
	protected readonly IShellExecutor spyExecutor = spyExecutor;
	protected readonly string directory = Environment.ExpandEnvironmentVariables(@"%TMP%\testing");
	protected T sut;
	protected string? suppliedCommandLine;

	[Fact]
	public void HaveNonNullActionName()
	{
		Assert.NotNull(sut.ActionName);
	}

}