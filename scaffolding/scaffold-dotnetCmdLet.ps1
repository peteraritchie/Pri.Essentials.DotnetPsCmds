$productName = 'DotnetPsCmds';
$rootPath = ".\$($ProductName)";
$srcPath = Join-Path $rootPath 'src';

if(test-path -Path $productName) {
    "Removing existing directories..."
    rmdir -r -force $productName;
}
md $productName 2>&1 >NUL || $(throw "error creating $($productName) directory.");

if($false) {
    'Checking for psmodule dotnet new template...'
    dotnet new list psmodule 2>&1 >NUL || dotnet new install Microsoft.PowerShell.Standard.Module.Template;
}

dotnet new gitignore -o $rootPath 2>&1 >NUL || $(throw 'error creating .gitignore');
dotnet new editorconfig -o $srcPath 2>&1 >NUL || $(throw 'error creating .editorconfig');

dotnet new sln -o $srcPath -n $productName 2>&1 >NUL || throw "Command failed to execute";
dotnet new classlib -o .\$srcPath\DotnetProjects -n Pri.Essentials.DotnetProjects --framework netstandard2.0 2>&1 >NUL || throw "Command failed to execute";
if(test-path -Path .\$srcPath\DotnetProjects\class1.cs) { del .\$srcPath\DotnetProjects\class1.cs  2>&1 >NUL || throw "Command failed to execute"; }
dotnet sln $srcPath add .\$srcPath\DotnetProjects 2>&1 >NUL || throw "Command failed to execute";
dotnet new psmodule -o .\$srcPath\$productName -n Pri.Essentials.$productName 2>&1 >NUL || throw "Command failed to execute";
dotnet reference add .\$srcPath\DotnetProjects --project .\$srcPath\$productName 2>&1 >NUL || throw "Command failed to execute";
dotnet sln $srcPath add .\$srcPath\$productName 2>&1 >NUL || throw "Command failed to execute";
dotnet new xunit3 -o .\$srcPath\Tests -n Tests 2>&1 >NUL || throw "Command failed to execute";
if(test-path -Path .\$srcPath\Tests\UnitTest1.cs) { del .\$srcPath\Tests\UnitTest1.cs  2>&1 >NUL || throw "Command failed to execute"; }
dotnet reference add .\$srcPath\$productName --project .\$srcPath\Tests 2>&1 >NUL || throw "Command failed to execute";
dotnet sln $srcPath add .\$srcPath\Tests 2>&1 >NUL || throw "Command failed to execute";

mkdir "$($rootPath)\scaffolding" 2>&1 >NUL || $(throw 'error creating scaffolding directory.')
Copy-Item $MyInvocation.MyCommand.Path "$($rootPath)\scaffolding";

Set-Content -Path $rootPath\README.md -Value "# $productName`r`n`r`n"

git init $rootPath;
git --work-tree=$rootPath --git-dir=$rootPath/.git add .;
git --work-tree=$rootPath --git-dir=$rootPath/.git commit -m "initial commit";

# Need command like Test-DotnetNew.
