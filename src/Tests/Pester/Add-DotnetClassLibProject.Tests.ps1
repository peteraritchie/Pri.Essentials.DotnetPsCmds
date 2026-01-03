BeforeAll {
	Import-Module "$PSScriptRoot/../../DotnetPsCmds/bin/Release/netstandard2.0/Pri.Essentials.DotnetPsCmds.dll";
}

AfterAll {
	Remove-Module Pri.Essentials.DotnetPsCmds -Force
}

Describe 'New-DotnetProject' {
	AfterEach {
		Remove-Item "$PSScriptRoot/TestProjects" -Recurse -Force -ErrorAction SilentlyContinue;
	}

	It 'Installed correctly' {
		Get-Module Pri.Essentials.DotnetPsCmds | Should -Not -Be $null
	}

	It 'Given classlib, creates project in correct location without default files' {
		# Arrange
		$projectType = 'classlib'
		$projectName = 'MyClassLib'
		$outputPath = "$PSScriptRoot/TestProjects/Pri.TheProduct/$projectName"

		# Act
		New-DotnetProject $projectType $outputPath $projectName;

		# Assert
		Test-Path "$outputPath/$projectName.csproj" | Should -Be $true
		Test-Path "$outputPath/Class1.cs" | Should -Be $false
	}

	It 'Given classlib and GenerateDocumentationFile, creates correct project' {
		# Arrange
		$projectType = 'classlib'
		$projectName = 'MyClassLib'
		$outputPath = "$PSScriptRoot/TestProjects/Pri.TheProduct.$projectName"

		# Act
		New-DotnetProject $projectType $outputPath $projectName -ShouldGenerateDocumentationFile;

		# Assert
		Test-Path "$outputPath/$projectName.csproj" | Should -Be $true;
		select-string `
			"<GenerateDocumentationFile>true</GenerateDocumentationFile>" `
			-Path "$outputPath/$projectName.csproj" | Should -Be $true;
	}
}
