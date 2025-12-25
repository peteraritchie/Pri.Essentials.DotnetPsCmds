using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Tests;

public class ProcessStartShould
{
    [Fact]
    public void ExecuteSuccessfully()
    {
        Assert.SkipUnless(RuntimeInformation.IsOSPlatform(OSPlatform.Windows), "Test that requires Windows environment.");

        var startInfo = new ProcessStartInfo("cmd.exe", "/c cd")
        {
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        };
        Process p = new() { StartInfo = startInfo };
        p.Start();
        p.WaitForExit();
        var outputText = p.StandardOutput.ReadToEnd();
        Assert.Equal($"{Directory.GetCurrentDirectory()}{Environment.NewLine}", outputText);
        var errorText = p.StandardError.ReadToEnd();
        Assert.Empty(errorText);
    }
}