BeforeAll {
	$projectName = 'MyClassLib';
	$outputPath = "$PSScriptRoot/TestProjects/Pri.TheProduct/$projectName";
}

Describe 'Add-DotnetPackage' {
	AfterEach {
		Remove-Item "$PSScriptRoot/TestProjects" -Recurse -Force -ErrorAction SilentlyContinue -ProgressAction SilentlyContinue;
	}

	It 'Package reference added correctly.' {
		# Act
		$p = New-DotnetProject 'classlib' $outputPath $projectName;
		$p | Add-DotnetPackage Pri.LongPath;

		# Assert
		Test-Path "$outputPath/$projectName.csproj" | Should -Be $true;

		select-string `
			'<PackageReference Include="Pri.LongPath" Version="\d+\.\d+.\d+"' `
			-Path "$outputPath/$projectName.csproj" | Should -Be $true;
	}

	It 'Given PackageId, package reference added correctly.' {
		# Act
		$p = New-DotnetProject 'classlib' $outputPath $projectName;
		$p | Add-DotnetPackage -PackageId Pri.LongPath;

		# Assert
		Test-Path "$outputPath/$projectName.csproj" | Should -Be $true;

		select-string `
			'<PackageReference Include="Pri.LongPath" Version="\d+\.\d+.\d+"' `
			-Path "$outputPath/$projectName.csproj" | Should -Be $true;
	}

	It 'Given Project, package reference added correctly.' {
		# Act
		$p = New-DotnetProject 'classlib' $outputPath $projectName;
		Add-DotnetPackage Pri.LongPath -Project $p;

		# Assert
		Test-Path "$outputPath/$projectName.csproj" | Should -Be $true;

		select-string `
			'<PackageReference Include="Pri.LongPath" Version="\d+\.\d+.\d+"' `
			-Path "$outputPath/$projectName.csproj" | Should -Be $true;
	}

	It 'Given PackageId and Project, package reference added correctly.' {
		# Act
		$p = New-DotnetProject 'classlib' $outputPath $projectName;
		Add-DotnetPackage -PackageId Pri.LongPath -Project $p;

		# Assert
		Test-Path "$outputPath/$projectName.csproj" | Should -Be $true;

		select-string `
			'<PackageReference Include="Pri.LongPath" Version="\d+\.\d+.\d+"' `
			-Path "$outputPath/$projectName.csproj" | Should -Be $true;
	}
}
