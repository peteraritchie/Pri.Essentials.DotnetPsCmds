namespace Pri.Essentials.DotnetProjects;

/// <summary>
/// A command execution result abstraction to encapsulate exit code,
/// standard output, and standard error.
/// </summary>
/// <param name="exitCode">The exit code from the shell.</param>
/// <param name="standardOutputText">
/// The standard output from the command.
/// </param>
/// <param name="standardErrorText">
/// The standard error, if any, from the command.
/// </param>
public sealed class ShellResult(
	int exitCode,
	string standardOutputText,
	string standardErrorText)
{
    /// <summary>The exit code returned from the command.</summary>
    public int ExitCode { get; private set; } = exitCode;

    /// <summary>The standard output from the command.</summary>
    public string StandardOutputText { get; private set; }
		= standardOutputText;

    /// <summary>The standard error from the command.</summary>
    public string StandardErrorText { get; private set; } = standardErrorText;

	/// <summary>
	/// <code>true</code> if the ExitCode is 0, <code>false</code> otherwise.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public bool IsSuccess => ExitCode == 0;

	/// <summary>
	/// <code>true</code> if the ExitCode is not 0, <code>false</code>
	/// otherwise.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public bool IsFailure => ExitCode != 0;
}