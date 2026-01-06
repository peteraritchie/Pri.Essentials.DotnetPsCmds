BeforeAll {
	dotnet new install xunit.v3.templates;
	Import-Module "$PSScriptRoot/../../Pri.Essentials.DotnetPsCmds/Pri.Essentials.DotnetPsCmds.psd1";
	$projectType = 'xunit3';
	$projectName = 'Tests';
	$outputPath = "$PSScriptRoot/TestProjects/Tests/$projectName";
}

AfterAll {
	Remove-Module Pri.Essentials.DotnetPsCmds -Force
}

Describe 'New-DotnetProject' {
	AfterEach {
		Remove-Item "$PSScriptRoot/TestProjects" -Recurse -Force -ErrorAction SilentlyContinue -ProgressAction SilentlyContinue;
	}

	It 'Installed correctly' {
		Get-Module Pri.Essentials.DotnetPsCmds | Should -Not -Be $null
	}

	It 'Given xunit3, creates project in correct location without default files' {
		# Act
		New-DotnetProject $projectType $outputPath $projectName;

		# Assert
		Test-Path "$outputPath/$projectName.csproj" | Should -Be $true
		Test-Path "$outputPath/UnitTest1.cs" | Should -Be $false
	}
}
