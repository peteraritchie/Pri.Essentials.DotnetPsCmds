BeforeAll {
	# $VerbosePreference="Continue";
	. "$PSScriptRoot/TestHelpers.ps1"
	$projectType = 'classlib';
	$projectName = 'MyClassLib';
	$testDir = 'TestProjects2';
	$outputPath = "$PSScriptRoot/$testDir/Pri.TheProduct/$projectName";
}

Describe 'Edit-DotnetProject' {
	BeforeEach {
		New-DotnetProject $projectType $outputPath $projectName;
	}

	AfterEach {
		Remove-Item "$PSScriptRoot/$testDir" -Recurse -Force -ErrorAction SilentlyContinue -ProgressAction SilentlyContinue;
	}

	It "Given project, sets InternalsVisibleTo correctly." {
		Edit-DotnetProject -path "$outputPath/$projectName.csproj" -InternalsVisibleTo Tests -WarningVariable warnings;

		$warnings | Should -Be $null

		Get-ItemProperty "$outputPath/$projectName.csproj" InternalsVisibleTo | Should -Be "Tests";
	}

	# TODO: tests to ensure warnings for invalid values are produced.
	It "Given project, sets property (<name>) correctly." -ForEach @(
		@{ Name = "AppendTargetFrameworkToOutputPath"; Value = "false"; Expected = "false" }
		@{ Name = "AssemblyName"; Value = "TheAssemblyName"; Expected = "TheAssemblyName" }
		@{ Name = "GenerateDocumentationFile"; Value = "true"; Expected = "true" }
		@{ Name = "IntermediateOutputPath"; Value = "Int"; Expected = "Int" }
		@{ Name = "LangVersion"; Value = "latest"; Expected = "latest" }
		@{ Name = "Nullable"; Value = "true"; Expected = "enable" }
		@{ Name = "Nullable"; Value = "enable"; Expected = "enable" }
		@{ Name = "Nullable"; Value = "false"; Expected = "disable" }
		@{ Name = "OutputType"; Value = "exe"; Expected = "exe" }
		@{ Name = "OutputPath"; Value = "Release"; Expected = "Release" }
		@{ Name = "RestorePackagesWithLockFile"; Value = "true"; Expected = "true" }
		@{ Name = "TargetFramework"; Value = "net10.0"; Expected = "net10.0" }
		@{ Name = "Version"; Value = "1.0.1"; Expected = "1.0.1" }
		@{ Name = "VersionPrefix"; Value = "2.1.3"; Expected = "2.1.3" }
		@{ Name = "VersionSuffix"; Value = "beta"; Expected = "beta" }
	) {
		Write-Verbose "name is: $name";
		Edit-DotnetProject -path "$outputPath/$projectName.csproj" -Properties "$($name):$($value)" -WarningVariable warnings;

		$warnings | Should -Be $null

		Test-ProjectPropertyExists "$outputPath/$projectName.csproj" $name | Should -Be $true;
		Get-ProjectProperty "$outputPath/$projectName.csproj" $name | Should -Be $expected;
	}
}
