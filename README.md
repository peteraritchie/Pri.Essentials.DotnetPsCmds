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

testing add project
```powershell
$p = new-dotnetproject xunit Dir Name net9.0;
$s = new-dotnetsolution;
$s.AddProject($p);
```

testing add class with namespace
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

testing create project from solution object

```powershell
$s = new-dotnetsolution;
$s.CreateProject("xunit3", "Dir", "Tests", "net10.0")
```

adding package reference to project

```powershell
$s = new-dotnetsolution;
$p = $s.CreateProject("xunit3", "Dir", "Tests", "net10.0");
$p.AddPackage("NSubstitute");
```