using System.Diagnostics;

namespace Pri.Essentials.DotnetProjects;

/// <summary> A proxy to the Linux shell </summary>
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
public class LinuxShellExecutor : ShellExecutor
{
	/// <inheritdoc />
	public override ShellResult Execute(string commandLine)
	{
		var startInfo = new ProcessStartInfo(fileName: "/bin/bash", arguments: $"-c \"{commandLine}\"")
		{
			UseShellExecute = false,
			RedirectStandardOutput = true,
			RedirectStandardError = true,
			CreateNoWindow = true
		};
		using Process p = new();
		p.StartInfo = startInfo;

		p.Start();
		p.WaitForExit();
		ShellResult shellResult = new(p.ExitCode, p.StandardOutput.ReadToEnd(), p.StandardError.ReadToEnd());

		return shellResult;
	}
}