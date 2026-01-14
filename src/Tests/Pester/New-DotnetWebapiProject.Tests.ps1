BeforeAll {
	$projectType = 'webapi';
	$projectName = 'MyApi';
	$outputPath = "$PSScriptRoot/TestProjects/Pri.TheProduct/$projectName";
}

Describe 'New-DotnetProject' {
	AfterEach {
		Remove-Item "$PSScriptRoot/TestProjects" -Recurse -Force -ErrorAction SilentlyContinue -ProgressAction SilentlyContinue;
	}

	It 'Installed correctly' {
		Get-Module Pri.Essentials.DotnetPsCmds | Should -Not -Be $null
	}

	It 'Given blazor, creates project in correct location with default files' {
		# Act
		New-DotnetProject $projectType $outputPath $projectName;

		# Assert
		Test-Path "$outputPath/$projectName.csproj" | Should -Be $true
		Test-Path "$outputPath/Properties/launchSettings.json" | Should -Be $true
		Test-Path "$outputPath/Program.cs" | Should -Be $true
		Test-Path "$outputPath/appsettings.json" | Should -Be $true
		Test-Path "$outputPath/appsettings.Development.json" | Should -Be $true
	}

	It 'Given blazor and GenerateDocumentationFile, creates correct project' {
		# Act
		New-DotnetProject $projectType $outputPath $projectName -ShouldGenerateDocumentationFile;

		# Assert
		Test-Path "$outputPath/$projectName.csproj" | Should -Be $true;
		select-string `
			"<GenerateDocumentationFile>true</GenerateDocumentationFile>" `
			-Path "$outputPath/$projectName.csproj" | Should -Be $true;
	}
}
