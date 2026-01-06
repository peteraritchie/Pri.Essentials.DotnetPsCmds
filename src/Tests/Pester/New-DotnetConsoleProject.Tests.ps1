BeforeAll {
	Import-Module "$PSScriptRoot/../../Pri.Essentials.DotnetPsCmds/Pri.Essentials.DotnetPsCmds.psd1";
	$projectType = 'console';
	$projectName = 'MyConsole';
	$outputPath = "$PSScriptRoot/TestProjects/Pri.TheProduct/$projectName";
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

	It 'Given console, creates project in correct location.' {
		# Act
		New-DotnetProject $projectType $outputPath $projectName;

		# Assert
		Test-Path "$outputPath/$projectName.csproj" | Should -Be $true
	}

	It 'Given console and GenerateDocumentationFile, creates correct project' {
		# Act
		New-DotnetProject $projectType $outputPath $projectName -ShouldGenerateDocumentationFile;

		# Assert
		Test-Path "$outputPath/$projectName.csproj" | Should -Be $true;
		select-string `
			"<GenerateDocumentationFile>true</GenerateDocumentationFile>" `
			-Path "$outputPath/$projectName.csproj" | Should -Be $true;
	}
}
