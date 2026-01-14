BeforeAll {
	$projectName = 'MyClassLib';
	$outputPath = "$PSScriptRoot/TestProjects/Pri.TheProduct/$projectName";
}

Describe 'Add-DotnetPackages' {
	AfterEach {
		Remove-Item "$PSScriptRoot/TestProjects" -Recurse -Force -ErrorAction SilentlyContinue -ProgressAction SilentlyContinue;
	}

	It 'Package references added correctly.' {
		# Act
		$p = New-DotnetProject 'classlib' $outputPath $projectName;
		$p | Add-DotnetPackages Pri.LongPath, ProductivityExtensions;

		# Assert
		Test-Path "$outputPath/$projectName.csproj" | Should -Be $true;

		select-string `
			'<PackageReference Include="Pri.LongPath" Version="\d+\.\d+.\d+"' `
			-Path "$outputPath/$projectName.csproj" | Should -Be $true;
		select-string `
			'<PackageReference Include="ProductivityExtensions" Version="\d+\.\d+.\d+"' `
			-Path "$outputPath/$projectName.csproj" | Should -Be $true;
	}

	It 'Given PackageId, package references added correctly.' {
		# Act
		$p = New-DotnetProject 'classlib' $outputPath $projectName;
		$p | Add-DotnetPackages -PackageIds Pri.LongPath, ProductivityExtensions;

		# Assert
		Test-Path "$outputPath/$projectName.csproj" | Should -Be $true;

		select-string `
			'<PackageReference Include="Pri.LongPath" Version="\d+\.\d+.\d+"' `
			-Path "$outputPath/$projectName.csproj" | Should -Be $true;
		select-string `
			'<PackageReference Include="ProductivityExtensions" Version="\d+\.\d+.\d+"' `
			-Path "$outputPath/$projectName.csproj" | Should -Be $true;
	}

	It 'Given Project, package references added correctly.' {
		# Act
		$p = New-DotnetProject 'classlib' $outputPath $projectName;
		Add-DotnetPackages Pri.LongPath, ProductivityExtensions -Project $p;

		# Assert
		Test-Path "$outputPath/$projectName.csproj" | Should -Be $true;

		select-string `
			'<PackageReference Include="Pri.LongPath" Version="\d+\.\d+.\d+"' `
			-Path "$outputPath/$projectName.csproj" | Should -Be $true;
		select-string `
			'<PackageReference Include="ProductivityExtensions" Version="\d+\.\d+.\d+"' `
			-Path "$outputPath/$projectName.csproj" | Should -Be $true;
	}

	It 'Given PackageId and Project, package references added correctly.' {
		# Act
		$p = New-DotnetProject 'classlib' $outputPath $projectName;
		Add-DotnetPackages -PackageIds Pri.LongPath, ProductivityExtensions -Project $p;

		# Assert
		Test-Path "$outputPath/$projectName.csproj" | Should -Be $true;

		select-string `
			'<PackageReference Include="Pri.LongPath" Version="\d+\.\d+.\d+"' `
			-Path "$outputPath/$projectName.csproj" | Should -Be $true;
		select-string `
			'<PackageReference Include="ProductivityExtensions" Version="\d+\.\d+.\d+"' `
			-Path "$outputPath/$projectName.csproj" | Should -Be $true;
	}
}
