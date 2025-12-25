using System.Runtime.InteropServices;

using Pri.Essentials.DotnetProjects;

namespace Tests;

public class ExecutorShould
{
	[Fact, Trait("Category", "Integration")]
    public void ExecuteSuccessfully()
    {
        Assert.SkipUnless(RuntimeInformation.IsOSPlatform(OSPlatform.Windows), "Skipped until non-windows executor is implemented.");

        var executor = ShellExecutor.Instance;
        var r = executor.Execute("cd");
        Assert.Empty(r.StandardErrorText);
        Assert.Equal($"{Directory.GetCurrentDirectory()}{Environment.NewLine}", r.StandardOutputText);
    }

	[Fact, Trait("Category", "Integration")]
	public void RedirectStandardError()
	{
		Assert.SkipUnless(RuntimeInformation.IsOSPlatform(OSPlatform.Windows), "Skipped until non-windows executor is implemented.");
        var executor = ShellExecutor.Instance;
        var r = executor.Execute("echo to error 1>&2");
        Assert.Empty(r.StandardOutputText);
		Assert.Equal($"to error {Environment.NewLine}", r.StandardErrorText);
	}
}