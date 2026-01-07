# Dotnet PowerShell Cmdlets

## Getting Started

Install the Dotnet PowerShell Cmdlets

```pwsh
Import-Module Pri.Essentials.DotnetPsCmds;
```

## Using the Dotnet PowerShell Cmdlets

Create a .NET solution:

```pwsh
New-DotnetSolution;
```

Which creates a solution in the current directory using the name of the current
directory for the name of the solution.

To Create a solution in a specific directory with a specific name:
```pwsh
New-DotnetSolution -OutputDirectory directory-name -Name solution-name;
```
OR
```pwsh
New-DotnetSolution MyProduct MySolution;
```

Which creates a solution with the relative path of `MyProduct/MySolution.sln`.

The cmdlets return objects that can be used with other cmdlets. For example,
to create a solution and store it in a variable:

```pwsh
$solution = New-DotnetSolution MyProduct;
```

Create a .NET project:

```pwsh
New-DotnetProject 'classlib' Domain MyProduct.Domain;
```

Creates a class library project with the name "MyProduct.Domain" in the
"Domain" directory.

At this point, you might be thinking "how is this diffent that using
the `dotnet` command line tool?" The objects returned by the cmdlets can be
built upon those executed cmdlets. For example, in complex solutions
you're going to want to add projects to the solution file. You can do that
by using a solution object:

```pwsh
$solution = New-DotnetSolution MyProduct; # MyProduct/MyProduct.sln
New-DotnetProject 'classlib' MyProduct/Domain MyProduct.Domain -Solution $solution;
```
New-DotnetProject 'classlib' $solution.Directory/Domain $solution.Name.Domain -Solution $solution;

Creates the solution `MyProduct/MyProduct.sln` then creates the project
`MyProduct/Domain/MyProduct.Domain.csproj` and adds the project to the
solution.

A nice feature of PowerShell is the ability to pipe objects. So, instead of
passing the solution object to the `New-DotnetProject` cmdlet, you can pipe
the result of `New-DotnetSolution` to `New-DotnetProject` to realize same
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

## Adding project references

```pwsh
$c = New-DotnetProject 'classlib' MyProduct/Domain MyProduct.Domain;
$t = New-DotnetProject 'xunit' MyProduct/Tests MyProduct.Tests;
Add-DotnetProjectReference -Project $c -TargetProject $t;
```

Which creates a class library, a unit tests project, and adds a reference
to the class library to the tests project.

Of course, pipes can be used here:

```pwsh
$c = New-DotnetProject 'classlib' MyProduct/Domain MyProduct.Domain;
New-DotnetProject 'xunit' MyProduct/Tests MyProduct.Tests `
    | Add-DotnetProjectReference -Project $c;
```

## Adding NuGet package references

```pwsh
$t = New-DotnetProject 'xunit' MyProduct/Tests MyProduct.Tests;
Add-DotnetPackages -PackageIds NSubstitute -Project $t;
```

Piping supported here as well:

```pwsh
New-DotnetProject 'xunit' MyProduct/Tests MyProduct.Tests `
    | Add-DotnetPackages NSubstitute;
```

## Scaffolding complete solutions

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

### Add-DotnetPackage Cmdlet

```pwsh
```

### Add-DotnetPackages Cmdlet

```pwsh
```

### Add-DotnetProject Cmdlet

```pwsh
```

### Add-DotnetProjectReference Cmdlet

```pwsh
```

### New-DotnetProject Cmdlet

```pwsh
```

### New-DotnetSolution Cmdlet

```pwsh
```

### DotnetSolution Class

```pwsh
```

### DotnetProject Class

```pwsh
```

---

### Creating a solution

Create a new solution in the current directory, using the name of the current
directory for the name of the solution (`dotnet new sln`):

```pwsh
New-DotnetSolution;
```

### Create a new solution in a specific directory, using the name of specific
direction for the name of the solution (`dotnet new sln -o ./MyProduct`):
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

### Create a new solution in a specific directory, with a specific name of 
the solution (`dotnet new sln -o ./MyProduct -n MySolution`):
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

### Create a new solution in the current directory and create a new class library
project within the solution:
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

Equivalent to (in current directory `MyProduct`):
```pwsh
dotnet new sln;
dotnet new classlib;
dotnet sln add MyProduct.csproj --in-root;
```

### Create a new solution in the current directory, create a new class library, and create a tests project in a Tests directory within the solution:

```pwsh
$s = New-DotnetSolution;
$lib = $s | New-DotnetProject 'classlib';
$s | New-DotnetProject 'xunit3' Tests | Add-DotnetProjectReference $lib;
```

Or:
```pwsh
$s = New-DotnetSolution;
$lib = $s | New-DotnetProject -TemplateName 'classlib';
$s | New-DotnetProject -TemplateName 'xunit3' -o Tests | Add-DotnetProjectReference -Project $lib;
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

### Create a project and add a NuGet package reference
```pwsh
New-DotnetProject 'xunit3' Tests | Add-DotnetPackages -PackageId "NSubstitute";
```

### Create a project and add multiple NuGet package reference

```pwsh
New-DotnetProject 'xunit3' MyLibrary.Tests | Add-DotnetPackages 'NSubstitute', 'FluentAssertions';
```

Equivalent to:
```pwsh
dotnet new xunit3 -o MyLibrary.Tests
dotnet add Tests package NSubstitute
dotnet add Tests package FluentAssertions
```