# Architecture

Dotnet PowerShell Cmdlets uses the project template Microsoft.PowerShell.Standard.Module.Template (i.e. created with `dotnet new psmodule`).

The PowerShell module project is Pri.Essentials.DotnetPsCmds.

The project creates various PowerShell CmdLets by implementing the abstract class `PSCmdlet`. Various CmdLets return complex objects that are defined in the Pri.Essentials.DotnetProjects project/assembly.

Each CmdLet imlementation is a minimal entry point translate into the creation and execution of Command pattern objects defined in the Pri.Essentials.DotnetProjects project/assembly.  For example `New-DotnetSolution` is implemented in `CreateDotnetSolutionCmdLet` that validates inputs and creates a `CreateSolutionCommand` object and calls its Execute method.

Most command implementations are mostly thin wrapper around a .NET CLI command. For example `CreateSolutionCommand` is a thin wrapper around the `dotnet new sln` command. Each command implementation users a IShellExecutor instance to invoke the .NET CLI commands and organize standard output, error, and result code.

Command implementations may do more than simply invoke a .NET CLI command. For example `CreateXunitProjectCommand` deletes the UnitTest1.cs file that is created by default when creating an xunit project. At some point in the future command implementations may avoid involving CLI commands and perform operations in-process.
