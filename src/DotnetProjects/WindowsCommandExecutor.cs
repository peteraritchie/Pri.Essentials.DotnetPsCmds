using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Pri.Essentials.DotnetProjects;

/// <summary>
/// The base interface for a shell executor
/// </summary>
public interface IShellExecutor
{
    /// <summary>
    /// Executes the command <paramref name="commandLine"/>.
    /// </summary>
    /// <param name="commandLine">The command to execute</param>
    /// <returns>A <seealso cref="ShellResult"/> with information about the execution.</returns>
    ShellResult Execute(string commandLine);
}

/// <summary>
/// An abstraction of a shell proxy to execute commands
/// </summary>
public abstract class ShellExecutor
    : IShellExecutor
{
    private static readonly Lazy<IShellExecutor> LazyShellExecutor
        = new(DetermineShellExecutor);

    /// <summary>
    /// Private method to figure out and create the environment-specific shell proxy.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage/*(Justification = "Text execution limited to one OS at a time.")*/]
    private static IShellExecutor DetermineShellExecutor()
    {
        return RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? new WindowsShellExecutor()
            : RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                ? new LinuxShellExecutor()
                : throw new InvalidOperationException("Current operating system not supported");
    }

    /// <summary>The environment-specific executor.</summary>
    public static IShellExecutor Instance => LazyShellExecutor.Value;

    /// <inheritdoc />
    public abstract ShellResult Execute(string commandLine);
}

/// <summary>
/// A proxy to the Windows command interpreter (shell)
/// </summary>
public sealed class WindowsShellExecutor : ShellExecutor
{
    /// <inheritdoc />
    public override ShellResult Execute(string commandLine)
    {
        var startInfo = new ProcessStartInfo("cmd.exe", $"/c {commandLine}")
        {
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        };
        using Process p = new();
        p.StartInfo = startInfo;

        p.Start();
        p.WaitForExit();
        ShellResult shellResult = new(p.ExitCode, p.StandardOutput.ReadToEnd(), p.StandardError.ReadToEnd());

        return shellResult;
    }
}

/// <summary> A proxy to the Linux shell </summary>
/// <remarks>WIP</remarks>
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
public class LinuxShellExecutor : ShellExecutor
{
    /// <inheritdoc />
    public override ShellResult Execute(string commandLine)
    {
        throw new NotImplementedException();
    }
}
