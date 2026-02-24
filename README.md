# Dotnet PowerShell Cmdlets

[![GitHub Release](https://img.shields.io/github/v/release/peteraritchie/Pri.Essentials.DotnetPsCmds)](https://github.com/peteraritchie/Pri.Essentials.DotnetPsCmds/releases "# of times GitHub release was downloaded.")
[![PowerShell Gallery Version](https://img.shields.io/powershellgallery/v/Pri.Essentials.DotnetPsCmds)](https://www.powershellgallery.com/packages/Pri.Essentials.DotnetPsCmds "Current version of Pri.Essentials.DotnetPsCmds on PowerShell Gallery")
[![PowerShell Gallery Downloads](https://img.shields.io/powershellgallery/dt/Pri.Essentials.DotnetPsCmds)](https://www.powershellgallery.com/packages/Pri.Essentials.DotnetPsCmds "# of times Pri.Essentials.DotnetPsCmds has been downloaded from the PowerShell Gallery")
[![CI](https://github.com/peteraritchie/Pri.Essentials.DotnetPsCmds/actions/workflows/ci.yaml/badge.svg)](https://github.com/peteraritchie/Pri.Essentials.DotnetPsCmds/actions/workflows/ci.yaml "Status of the last workflow run.")
[![CodeQL](https://github.com/peteraritchie/Pri.Essentials.DotnetPsCmds/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/peteraritchie/Pri.Essentials.DotnetPsCmds/actions/workflows/github-code-scanning/codeql "Status of the last CodeQL scan.")
[![GitHub License](https://img.shields.io/github/license/peteraritchie/Pri.Essentials.DotnetPsCmds)](https://github.com/peteraritchie/Pri.Essentials.DotnetPsCmds?tab=MIT-1-ov-file#readme "This project is license under the MIT license.")
[![GitHub Repo stars](https://img.shields.io/github/stars/peteraritchie/Pri.Essentials.DotnetPsCmds?style=flat)](https://github.com/peteraritchie/Pri.Essentials.DotnetPsCmds/stargazers "# of people who have starred this repo.")
[![GitHub Repo forks](https://img.shields.io/github/forks/peteraritchie/Pri.Essentials.DotnetPsCmds?style=flat)](https://github.com/peteraritchie/Pri.Essentials.DotnetPsCmds/forks "# of forks of this repo.")
[![Dynamic JSON Badge](https://img.shields.io/badge/dynamic/json?url=https%3A%2F%2Fapi.codetabs.com%2Fv1%2Floc%2F%3Fgithub%3Dpeteraritchie%2FPri.Essentials.DotnetPsCmds&query=%24%5B0%5D.linesOfCode.&label=LOC)](https://github.com/peteraritchie/Pri.ProductivityExtensions.Source)

## Contents
- [Getting Started](#getting-started)
- [New-DotnetSolution](#create-a-net-solution)
- [New-DotnetProject](#create-a-net-project)
- [Add-DotnetProjectReference](#adding-project-references)
- [Add-DotnetPackages](#adding-nuget-package-references)
- [Add-DotnetProject](#adding-project-references)
- [Modifying project properties](#modifying-project-properties) &#x1F195;
- [Scaffolding complete solutions](#scaffolding-complete-solutions)
 
## Getting Started

Install the Dotnet PowerShell Cmdlets

```pwsh
Import-Module Pri.Essentials.DotnetPsCmds;
```

## Using the Dotnet PowerShell Cmdlets

### Create a .NET solution

```pwsh
New-DotnetSolution;
```

This creates a solution in the current directory, using the current directory
name as the solution name (`dotnet new sln`).

To create a solution in a specific directory with a specific name:
```pwsh
New-DotnetSolution -OutputDirectory directory-name -Name solution-name;
```
OR
```pwsh
New-DotnetSolution MyProduct MySolution;
```

This creates a solution with the relative path of `MyProduct/MySolution.sln`
(`dotnet new sln -o MyProduct -n MySolution`).

The cmdlets return objects that can be used with other cmdlets. For example,
to create a solution and store it in a variable:

```pwsh
$solution = New-DotnetSolution MyProduct;
```

### Create a .NET project

```pwsh
New-DotnetProject 'classlib' Domain MyProduct.Domain;
```

Creates a class library project with the name "MyProduct.Domain" in the
"Domain" directory (`dotnet new classlib -o Domain -n MyProduct.Domain`).

At this point, you might be thinking, "How is this different than using
the `dotnet` command line tool"? The objects returned by the cmdlets can be
built upon the results of executed cmdlets. For example, in complex solutions,
you're going to want to add projects to the solution file. You can do that
by using a solution object:

```pwsh
$solution = New-DotnetSolution MyProduct; # MyProduct/MyProduct.sln
New-DotnetProject 'classlib' MyProduct/Domain MyProduct.Domain -Solution $solution;
```
New-DotnetProject 'classlib' $solution.Directory/Domain $solution.Name.Domain -Solution $solution;

Creates the solution `MyProduct/MyProduct.sln`, then creates the project
`MyProduct/Domain/MyProduct.Domain.csproj` and adds the project to the
solution.

A nice feature of PowerShell is the ability to pipe objects. So, instead of
passing the solution object to the `New-DotnetProject` cmdlet, you can pipe
the result of `New-DotnetSolution` to `New-DotnetProject` to realize the same
outcome:

```pwsh
New-DotnetSolution MyProduct `
    | New-DotnetProject 'classlib' MyProduct/Domain MyProduct.Domain;
```
Or the solution result can be stored in an object and piped to more than one
`New-DotnetProject`:

```pwsh
$s = New-DotnetSolution MyProduct;
$s | New-DotnetProject 'classlib' MyProduct/Domain MyProduct.Domain;
$s | New-DotnetProject 'xunit' MyProduct/Tests MyProduct.Tests;
```

### Adding project references

```pwsh
$c = New-DotnetProject 'classlib' MyProduct/Domain MyProduct.Domain;
$t = New-DotnetProject 'xunit' MyProduct/Tests MyProduct.Tests;
Add-DotnetProjectReference -Project $c -TargetProject $t;
$s = New-DotnetSolution MyProduct;
Add-DotnetProject -Solution $s -Project $c;
Add-DotnetProject -Solution $s -Project $t;
```

This creates a class library and a unit test project, and adds a reference
to the class library to the tests project.

Of course, pipes can be used here:

```pwsh
$c = New-DotnetProject 'classlib' MyProduct/Domain MyProduct.Domain;
$t = New-DotnetProject 'xunit' MyProduct/Tests MyProduct.Tests `
    | Add-DotnetProjectReference -Project $c;
New-DotnetSolution MyProduct `
    | Add-DotnetProject -Project $c `
    | Add-DotnetProject -Project $t;
```

### Adding NuGet package references

```pwsh
$t = New-DotnetProject 'xunit' MyProduct/Tests MyProduct.Tests;
Add-DotnetPackages -PackageIds NSubstitute -Project $t;
```

Piping is supported here as well:

```pwsh
New-DotnetProject 'xunit' MyProduct/Tests MyProduct.Tests `
    | Add-DotnetPackages NSubstitute;
```

Specific package versions are also supported via the `PackageVersion` positional parameter:

```pwsh
$t = New-DotnetProject 'xunit' MyProduct/Tests MyProduct.Tests;
$t | Add-DotnetPackage NSubstitute 5.2.0
```
Or
```pwsh
$t = New-DotnetProject 'xunit' MyProduct/Tests MyProduct.Tests;
$t | Add-DotnetPackage -PackageId NSubstitute -PackageVersion 5.2.0
```

### Modifying project properties

The `Edit-DotnetProject` cmdlet is used to edit project properties, further
enabling scaffolding-as-code. There are two types of properties: project group
properties and item group properties. Both are handled differently by
Edit-DotnetProject. To support modifying multiple project group properties
at once, there is a `-Properties` parameter that accepts either a single 
name/value pair or an array of name/value pairs. Item group properties have
explicit parameters because their value can sometimes be complex and not just
a single value.

Some examples of project group properties follow:

Set the language version of a project to the latest version:

```pwsh
Edit-DotnetProject -path src\TheProgram.csproj -Properties "LangVersion:latest"
```

Set the language version of a project to the latest version and enable
nullable reference types:

```pwsh
Edit-DotnetProject -path src\TheProgram.csproj -Properties @("LangVersion:latest", "Nullable:true")
```

Some examples of item group properties follow:

To make internal types visible to the Tests assembly&mdash;setting the
`InternalsVisibleTo` property:

```pwsh
Edit-DotnetProject -path src\TheProgram.csproj -InternalsVisibleTo Tests
```

To globally suppress a code analysis rule violation for the entire module:

```pwsh
Edit-DotnetProject -path src\TheProgram.csprojroj -SuppressMessage @{Category="Compiler"; CheckId="CS1591:MissingXmlCommentForPubliclyVisibleTypeOrMember"; Justification:"Generated Code"}

```

Supported project group properties:

| **Property** | **Brief explanation** |
|---|---|
| **AppendTargetFrameworkToOutputPath** | When `true`, appends the target framework folder (e.g., `net6.0`) to the output path to avoid multi-targeting collisions. |
| **AssemblyName** | The base name of the compiled assembly (DLL or EXE) produced by the build. |
| **GenerateDocumentationFile** | When `true`, emits an XML documentation file from triple-slash comments alongside the assembly. |
| **IntermediateOutputPath** | Directory where intermediate build artifacts are written (typically the `obj` folder). |
| **LangVersion** | Specifies the C# language version to use (e.g., `latest`, `9.0`, `preview`). Example argument text: `"LangVersion:latest"` |
| **Nullable** | Controls nullable reference types behavior; common values: `enable`, `disable`, `annotations`, `warnings`. Example argument text: `"Nullable:true"` |
| **OutputType** | Type of output produced by the project, e.g., `Library`, `Exe`, `WinExe`, `Module`. |
| **OutputPath** | Final directory where compiled outputs are placed (typically under `bin\<Configuration>\`). |
| **RestorePackagesWithLockFile** | When `true`, enables package restore using a lock file (`packages.lock.json`) to ensure repeatable restores. |
| **TargetFramework** | The target framework moniker the project builds for (e.g., `net7.0`, `netstandard2.0`). |
| **Version** | The full assembly/package version string used for the build and NuGet package (overrides prefix/suffix if set). |
| **VersionPrefix** | The base version portion (e.g., `1.2.3`) used when composing package/assembly versions. |
| **VersionSuffix** | A prerelease or build metadata suffix appended to the version (e.g., `beta1`, `ci-1234`). |

More detail on these properties can be found in [Microsoft Learn](https://learn.microsoft.com/en-us/visualstudio/msbuild/common-msbuild-project-properties?view=visualstudio) 

Supported item group properties:

- InternalsVisibleTo
- AssemblyAttribute/SuppressMessageAttribute

### Scaffolding complete solutions

```pwsh
$s = New-DotnetSolution MyProduct;
$d = New-DotnetProject 'classlib' "$($s.Directory)/Domain" "$($s.Name).Domain";
$a = New-DotnetProject 'webapi' "$($s.Directory)/WebApi" "$($s.Name).WebApi" `
    | Add-DotnetProjectReference -Project $d;
$t = New-DotnetProject 'xunit' "$($s.Directory)/Tests" "$($s.Name).Tests" `
    | Add-DotnetPackages NSubstitute `
    | Add-DotnetProjectReference -Project $a;
```

## Reference

### New-DotnetSolution Cmdlet

```text
New-DotnetSolution [[-OutputDirectory] <string>] [[-OutputName] <string>] [-WhatIf] [-Confirm] [<CommonParameters>]
```

Create a new solution in a specific directory, using the name of the
specified directory for the name of the solution
(`dotnet new sln -o ./MyProduct`):

```pwsh
New-DotnetSolution MyProduct;
```
Or:
```pwsh
New-DotnetSolution -o MyProduct;
```
Or:
```pwsh
New-DotnetSolution -OutputDirectory MyProduct;
```
Or:
```pwsh
New-DotnetSolution -Path MyProduct;
```

Create a new solution in a specific directory, with a specific name
of the solution (`dotnet new sln -o ./MyProduct -n MySolution`):

```pwsh
New-DotnetSolution MyProduct MySolution;
```
Or:
```pwsh
New-DotnetSolution -OutputDirectory MyProduct -OutputName MySolution;
```
Or:
```pwsh
New-DotnetSolution -o MyProduct -n MySolution;
```
Or:
```pwsh
New-DotnetSolution -Path MyProduct -Name MySolution;
```

### Add-DotnetPackages Cmdlet

```text
Add-DotnetPackages [-PackageIds] <string[]> [-Project] <DotnetProject> [[-Prerelease] <bool>] [-WhatIf] [-Confirm] [<CommonParameters>]
```

Create a project and add a NuGet package reference:
```pwsh
New-DotnetProject 'xunit3' Tests `
    | Add-DotnetPackages -PackageId "NSubstitute";
```

Create a project and add multiple NuGet package references

```pwsh
New-DotnetProject 'xunit3' MyLibrary.Tests `
    | Add-DotnetPackages 'NSubstitute', 'FluentAssertions';
```

Equivalent to:
```pwsh
dotnet new xunit3 -o MyLibrary.Tests
dotnet add Tests package NSubstitute
dotnet add Tests package FluentAssertions
```### Add-DotnetProject Cmdlet

```text
Add-DotnetProject [-Solution] <DotnetSolution> [-Project] <DotnetProject> [-SolutionFolder <string>] [-WhatIf] [-Confirm] [<CommonParameters>]
```

### Add-DotnetPackage Cmdlet

Add-DotnetPackage supports package versions.

```text
Add-DotnetProject [-Solution] <DotnetSolution> [-Project] <DotnetProject> [-SolutionFolder <string>] [-WhatIf]
[-Confirm] [<CommonParameters>]
```

Add a specific version of a NuGet package to a project:

```pwsh
$t = New-DotnetProject 'xunit' MyProduct/Tests MyProduct.Tests;
$t | Add-DotnetPackage NSubstitute 5.2.0
```

### Add-DotnetProjectReference Cmdlet

```text
Add-DotnetProjectReference [-Project] <DotnetProject> [-TargetProject] <DotnetProject> [-WhatIf] [-Confirm][<CommonParameters>]
```

Create a new solution in the current directory, create a new class library,
and create a tests project in a Tests directory within the solution:

```pwsh
$s = New-DotnetSolution;
$lib = $s | New-DotnetProject 'classlib';
$s | New-DotnetProject 'xunit3' Tests | Add-DotnetProjectReference $lib;
```

Or:
```pwsh
$s = New-DotnetSolution;
$lib = $s | New-DotnetProject -TemplateName 'classlib';
$s | New-DotnetProject -TemplateName 'xunit3' -o Tests `
    | Add-DotnetProjectReference -Project $lib;
```

Or:
```pwsh
$s = New-DotnetSolution;
$lib = $s | New-DotnetProject -TemplateName 'classlib';
$s | New-DotnetProject -TemplateName 'xunit3' -o Tests | Add-DotnetProjectReference -Project $lib;
```
Equivalent to (in current directory `MyProduct`):

```pwsh
dotnet new sln;
dotnet new classlib;
dotnet sln add MyProduct.csproj --in-root;
dotnet new xunit3 -o Tests;
dotnet add Tests reference MyProduct.csproj
dotnet sln add Tests --in-root;
```

### New-DotnetProject Cmdlet

```text
    New-DotnetProject [-TemplateName] {xunit | xunit3 | console | classlib | blazor | worker | webapi | winforms}
    [[-OutputDirectory] <string>] [[-OutputName] <string>] [[-FrameworkName] {net10.0 | net9.0 | net8.0 | standard2.0
    | standard2.1}] [[-Solution] <DotnetSolution>] [-ShouldGenerateDocumentationFile] [-WhatIf] [-Confirm]
    [<CommonParameters>]

    New-DotnetProject [-TemplateName] {xunit | xunit3 | console | classlib | blazor | worker | webapi | winforms}
    [[-OutputDirectory] <string>] [[-OutputName] <string>] [[-FrameworkName] {net10.0 | net9.0 | net8.0 | standard2.0
    | standard2.1}] [-Solution] <DotnetSolution> [-SolutionFolder <string>] [-ShouldGenerateDocumentationFile]
    [-WhatIf] [-Confirm] [<CommonParameters>]
```
Create a new solution in the current directory and create a new class
library project within the solution:
```pwsh
New-DotnetSolution | New-DotnetProject 'classlib'
```
```pwsh
$s = New-DotnetSolution;
$s | New-DotnetProject 'classlib';
```
Or:
```pwsh
New-DotnetSolution | New-DotnetProject -TemplateName 'classlib';
```

Equivalent to (in the current directory `MyProduct`):
```pwsh
dotnet new sln;
dotnet new classlib;
dotnet sln add MyProduct.csproj --in-root;
```


### DotnetSolution Class

```text
   TypeName: Pri.Essentials.DotnetProjects.DotnetSolution

Name      MemberType Definition
----      ---------- ----------
Directory Property   string Directory {get;}
FullPath  Property   string FullPath {get;}
Name      Property   string Name {get;}
```

### DotnetProject Class

```text
   TypeName: Pri.Essentials.DotnetProjects.DotnetProject

Name      MemberType Definition
----      ---------- ----------
Directory Property   string Directory {get;}
FullPath  Property   string FullPath {get;}
Name      Property   string Name {get;}
Solution  Property   Pri.Essentials.DotnetProjects.DotnetSolution Solution {get;set;}
```

---
[![Repo Size](https://img.shields.io/github/repo-size/peteraritchie/Pri.Essentials.DotnetPsCmds)](https://github.com/peteraritchie/Pri.Essentials.DotnetPsCmds)
[![Languages](https://img.shields.io/github/languages/count/peteraritchie/Pri.Essentials.DotnetPsCmds)](https://github.com/peteraritchie/Pri.Essentials.DotnetPsCmds)
[![Primary Language](https://img.shields.io/github/languages/top/peteraritchie/Pri.Essentials.DotnetPsCmds)](https://github.com/peteraritchie/Pri.Essentials.DotnetPsCmds)
[![Code Size](https://img.shields.io/github/languages/code-size/peteraritchie/Pri.Essentials.DotnetPsCmds)](https://github.com/peteraritchie/Pri.Essentials.DotnetPsCmds)
[![downloads](https://img.shields.io/github/downloads/peteraritchie/Pri.Essentials.DotnetPsCmds/total)](https://github.com/peteraritchie/Pri.Essentials.DotnetPsCmds)