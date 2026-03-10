BeforeAll {
	$productName = 'Pri.TheProduct';
	$outputPath = "$PSScriptRoot/TestProjects/$productName";
}

Describe 'Add-DotnetProject' {
	AfterEach {
		Remove-Item "$PSScriptRoot/TestProjects" -Recurse -Force -ErrorAction SilentlyContinue -ProgressAction SilentlyContinue;
	}

	It 'Given Solution, adds classlib project correctly' {

		$p = New-DotnetProject 'classlib' -OutputDirectory "$outputPath/MyClassLib" -OutputName  'MyClassLib';

		$s = New-DotnetSolution -OutputDirectory $outputPath;

		Add-DotnetProject -Solution $s -Project $p;

		Test-Path "$outputPath/$productName.sln" | Should -Be $true;
		select-string `
			'"MyClassLib", "MyClassLib(\\|/)MyClassLib.csproj"' `
			-Path "$outputPath/$productName.sln" -Quiet | Should -Be $true;

	}

	It 'Given Solution, adds .NET 10 classlib project correctly' {

		$p = New-DotnetProject 'classlib' -OutputDirectory "$outputPath/MyClassLib" -OutputName  'MyClassLib' -FrameworkName 'net10.0';

		$s = New-DotnetSolution -OutputDirectory $outputPath;

		Add-DotnetProject -Solution $s -Project $p;

		Test-Path "$outputPath/$productName.sln" | Should -Be $true;
		select-string `
			'"MyClassLib", "MyClassLib(\\|/)MyClassLib.csproj"' `
			-Path "$outputPath/$productName.sln" -Quiet | Should -Be $true;

		select-string '<TargetFramework>net10.0</TargetFramework>' `
			-SimpleMatch -Path $p.FullPath `
			-Quiet | Should -Be $true;
	}

	It 'Given Solution, adds .NET 9 classlib project correctly' {

		$p = New-DotnetProject 'classlib' -OutputDirectory "$outputPath/MyClassLib" -OutputName  'MyClassLib' -FrameworkName 'net9.0';

		$s = New-DotnetSolution -OutputDirectory $outputPath;

		Add-DotnetProject -Solution $s -Project $p;

		Test-Path "$outputPath/$productName.sln" | Should -Be $true;
		select-string `
			'"MyClassLib", "MyClassLib(\\|/)MyClassLib.csproj"' `
			-Path "$outputPath/$productName.sln" -Quiet | Should -Be $true;

		select-string '<TargetFramework>net9.0</TargetFramework>' `
			-SimpleMatch -Path $p.FullPath `
			-Quiet | Should -Be $true;
	}

	It 'Given Solution, adds .NET 8 classlib project correctly' {

		$p = New-DotnetProject 'classlib' -OutputDirectory "$outputPath/MyClassLib" -OutputName  'MyClassLib' -FrameworkName 'net8.0';

		$s = New-DotnetSolution -OutputDirectory $outputPath;

		Add-DotnetProject -Solution $s -Project $p;

		Test-Path "$outputPath/$productName.sln" | Should -Be $true;
		select-string `
			'"MyClassLib", "MyClassLib(\\|/)MyClassLib.csproj"' `
			-Path "$outputPath/$productName.sln" -Quiet | Should -Be $true;

		select-string '<TargetFramework>net8.0</TargetFramework>' `
			-SimpleMatch -Path $p.FullPath `
			-Quiet | Should -Be $true;
	}

	It 'Given Solution, adds .NET Standard 2.0 classlib project correctly' {

		$p = New-DotnetProject 'classlib' -OutputDirectory "$outputPath/MyClassLib" -OutputName  'MyClassLib' -FrameworkName 'netstandard2.0';

		$s = New-DotnetSolution -OutputDirectory $outputPath;

		Add-DotnetProject -Solution $s -Project $p;

		Test-Path "$outputPath/$productName.sln" | Should -Be $true;
		select-string `
			'"MyClassLib", "MyClassLib(\\|/)MyClassLib.csproj"' `
			-Path "$outputPath/$productName.sln" -Quiet | Should -Be $true;

		select-string '<TargetFramework>netstandard2.0</TargetFramework>' `
			-SimpleMatch -Path $p.FullPath `
			-Quiet | Should -Be $true;
	}

	It 'Given Solution, adds .NET Standard 2.1 classlib project correctly' {

		$p = New-DotnetProject 'classlib' -OutputDirectory "$outputPath/MyClassLib" -OutputName  'MyClassLib' -FrameworkName 'netstandard2.1';

		$s = New-DotnetSolution -OutputDirectory $outputPath;

		Add-DotnetProject -Solution $s -Project $p;

		Test-Path "$outputPath/$productName.sln" | Should -Be $true;
		select-string `
			'"MyClassLib", "MyClassLib(\\|/)MyClassLib.csproj"' `
			-Path "$outputPath/$productName.sln" -Quiet | Should -Be $true;

		select-string '<TargetFramework>netstandard2.1</TargetFramework>' `
			-SimpleMatch -Path $p.FullPath `
			-Quiet | Should -Be $true;
	}
}