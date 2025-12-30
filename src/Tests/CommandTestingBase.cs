using Pri.Essentials.DotnetProjects;
using Pri.Essentials.DotnetProjects.Commands;

namespace Tests;

public abstract class CommandTestingBase<T>(IShellExecutor spyExecutor)
	where T : CommandBase
{
	protected readonly IShellExecutor spyExecutor = spyExecutor;
	protected readonly string directory = Environment.ExpandEnvironmentVariables(@"%TMP%\MyProduct");
	protected T? sut;
	protected string? suppliedCommandLine;

	[Fact]
	public void HaveNonNullSut()
	{
		Assert.NotNull(sut);
	}

	[Fact]
	public void HaveNonNullActionName()
	{
		Assert.NotNull(sut!.ActionName);
	}

	[Fact]
	public void HaveNonNullTarget()
	{
		Assert.NotNull(sut!.Target);
	}
}