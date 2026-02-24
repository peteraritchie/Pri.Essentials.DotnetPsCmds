BeforeAll {
	$outputPath = "$PSScriptRoot/TestProjects/Pri.TheProduct";
}

Describe 'Add-DotnetProjectReference' {
	AfterEach {
		Remove-Item "$PSScriptRoot/TestProjects" -Recurse -Force -ErrorAction SilentlyContinue -ProgressAction SilentlyContinue;
	}

	It 'Given project reference added correctly.' {
		# Act
		$classLib = New-DotnetProject 'classlib' "$outputPath/MyClassLib" MyClassLib;
		$tests = New-DotnetProject 'xunit' "$outputPath/Tests" Tests;
		$tests | Add-DotnetProjectReference -Project $classLib;

		# Assert
		Test-Path "$outputPath/MyClassLib/MyClassLib.csproj" | Should -Be $true;
		Test-Path "$outputPath/Tests/Tests.csproj" | Should -Be $true;

		select-string `
			'<ProjectReference Include="..(\\|/)MyClassLib(\\|/)MyClassLib.csproj" />' `
			-Path "$outputPath/Tests/Tests.csproj" -Quiet | Should -Be $true;
	}

	It 'Given TargetProject, project reference added correctly.' {
		# Act
		$classLib = New-DotnetProject 'classlib' "$outputPath/MyClassLib" MyClassLib;
		$tests = New-DotnetProject 'xunit' "$outputPath/Tests" Tests;
		Add-DotnetProjectReference -Project $classLib -TargetProject $tests;

		# Assert
		Test-Path "$outputPath/MyClassLib/MyClassLib.csproj" | Should -Be $true;
		Test-Path "$outputPath/Tests/Tests.csproj" | Should -Be $true;

		select-string `
			'<ProjectReference Include="..(\\|/)MyClassLib(\\|/)MyClassLib.csproj" />' `
			-Path "$outputPath/Tests/Tests.csproj" -Quiet | Should -Be $true;
	}
}
