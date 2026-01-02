namespace Pri.Essentials.DotnetProjects.Commands;

/// <summary>
/// An abstraction of a command pattern.
/// </summary>
/// <param name="shellExecutor"></param>
public abstract class CommandBase(IShellExecutor shellExecutor)
{
	/// <summary>
	/// The shell executor used to run shell commands.
	/// </summary>
	protected readonly IShellExecutor shellExecutor = shellExecutor;

	/// <summary>
	/// The target that the command will affect. Typically, file or a directory.
	/// </summary>
	public abstract string Target { get; }

	/// <summary>
	/// The name of that action that will affect <seealso cref="Target"/>
	/// </summary>
	public abstract string ActionName { get; }

	/// <summary>
	/// Gets or sets whether to generate the xml doc file
	/// </summary>
	public bool? ShouldGenerateDocumentationFile { get; set; }

	/// <summary>
	/// Execute the command (and perform that action, affecting the target).
	/// </summary>
	/// <returns></returns>
	public abstract Result Execute();
}