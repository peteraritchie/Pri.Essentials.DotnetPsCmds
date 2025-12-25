namespace Pri.Essentials.DotnetProjects;

/// <summary>
/// Represents the result of an operation, indicating whether it was successful or not.
/// </summary>
public abstract record Result
{
	/// <summary>
	/// Indicates whether the operation was successful.
	/// </summary>
	public abstract bool IsSuccessful { get; }
}

/// <summary>
/// Represents the result of a shell command execution, including the exit code, standard output, and standard error
/// output.
/// </summary>
/// <remarks>Use this record to capture and inspect the outcome of executing a shell or command-line process. The
/// properties provide access to the process's exit code and any output or error messages generated during
/// execution.</remarks>
/// <param name="ExitCode">The exit code returned by the shell process. A value of 0 typically indicates success; nonzero values indicate an
/// error or abnormal termination.</param>
/// <param name="OutputText">The text written to the standard output stream by the shell command. May be empty if the command produced no output.</param>
/// <param name="ErrorText">The text written to the standard error stream by the shell command. May be empty if no errors were reported.</param>
public record ShellOperationResult(int ExitCode, string OutputText, string ErrorText) : Result
{
    /// <summary>
    /// 
    /// </summary>
    public int ExitCode { get; private set; } = ExitCode;
    /// <summary>
    /// 
    /// </summary>
    public string OutputText { get; private set; } = OutputText;
    /// <summary>
    /// 
    /// </summary>
    public string ErrorText { get; private set; } = ErrorText;
	/// <summary>
	/// 
	/// </summary>
	public override bool IsSuccessful => ExitCode == 0;
}
