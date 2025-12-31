# DotnetPsCmds

Go to the right directory to test and load the module and list the commands:

```powershell
cd .\src\experiment\DotnetPsCmds\t && Import-Module ..\src\DotnetPsCmds\bin\Debug\netstandard2.0\Pri.Essentials.DotnetPsCmds.dll && get-command -module Pri.Essentials.DotnetPsCmds
```

```powershell
Import-Module .\src\DotnetPsCmds\bin\Debug\netstandard2.0\Pri.Essentials.DotnetPsCmds.dll
```

testing sln
```powershell
cd .\src\experiment\DotnetPsCmds\t && Import-Module ..\src\DotnetPsCmds\bin\Debug\netstandard2.0\Pri.Essentials.DotnetPsCmds.dll && ($s = New-DotnetSolution);
```

testing module
```powershell
cd .\src\experiment\DotnetPsCmds\t && Import-Module ..\src\DotnetPsCmds\bin\Debug\netstandard2.0\Pri.Essentials.DotnetPsCmds.dll && get-command -module Pri.Essentials.DotnetPsCmds
```

~~testing add project~~
```powershell
$p = new-dotnetproject xunit Dir Name net9.0;
$s = new-dotnetsolution;
$s.AddProject($p);
```

~~testing add class with namespace~~
```powershell
$p1 = new-dotnetproject xunit3 Tests Tests
$p1.AddClass("NS.OtherClass") 
```

testing add project to solution cmdlet

```powershell
$p = new-dotnetproject xunit Dir Name net9.0;
$s = new-dotnetsolution;
add-dotnetproject $s $p;
```
```powershell
$p = new-dotnetproject xunit Dir Name net9.0;
$s = new-dotnetsolution | add-dotnetproject -Project $p;
```
```powershell
$p = new-dotnetproject xunit Dir Name net9.0;
$s = new-dotnetsolution;
$s | add-dotnetproject -Project $p;
```

~~testing create project from solution object~~

```powershell
$s = new-dotnetsolution;
$s.CreateProject("xunit3", "Dir", "Tests", "net10.0")
```

~~adding package reference to project~~

```powershell
$s = new-dotnetsolution;
$p = $s.CreateProject("xunit3", "Dir", "Tests", "net10.0");
$p.AddPackage("NSubstitute");
```

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