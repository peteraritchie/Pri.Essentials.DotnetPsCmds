namespace Pri.Essentials.DotnetProjects;

public abstract record Result
{
	public abstract bool IsSuccessful { get; }
}

public record ShellOperationResult(int ExitCode, string OutputText, string ErrorText) : Result
{
    public int ExitCode { get; private set; } = ExitCode;
    public string OutputText { get; private set; } = OutputText;
    public string ErrorText { get; private set; } = ErrorText;
	public override bool IsSuccessful => ExitCode == 0;
}
