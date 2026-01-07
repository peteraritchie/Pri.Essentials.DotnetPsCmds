BeforeAll {
	Import-Module "$PSScriptRoot/../../Pri.Essentials.DotnetPsCmds/Pri.Essentials.DotnetPsCmds.psd1";
	$productName = 'Pri.TheProduct';
	$outputPath = "$PSScriptRoot/TestProjects/$productName";
}

AfterAll {
	Remove-Module Pri.Essentials.DotnetPsCmds -Force
}

Describe 'Scaffolding' {
	AfterEach {
		Remove-Item "$PSScriptRoot/TestProjects" -Recurse -Force -ErrorAction SilentlyContinue -ProgressAction SilentlyContinue;
	}

	It 'Creates Web API solution correctly' {

		$s = New-DotnetSolution $outputPath;
		$d = New-DotnetProject 'classlib' "$($s.Directory)/Domain" "$($s.Name).Domain";
		$a = New-DotnetProject 'webapi' "$($s.Directory)/WebApi" "$($s.Name).WebApi" `
			| Add-DotnetProjectReference -Project $d;
		$t = New-DotnetProject 'xunit' "$($s.Directory)/Tests" "$($s.Name).Tests" `
			| Add-DotnetPackages NSubstitute `
			| Add-DotnetProjectReference -Project $a;

		Test-Path "$outputPath/Domain/$($s.Name).Domain.csproj" | Should -Be $true;
		Test-Path "$outputPath/WebApi/$($s.Name).WebApi.csproj" | Should -Be $true;
		Test-Path "$outputPath/Tests/$($s.Name).Tests.csproj" | Should -Be $true;

		select-string `
			'<PackageReference Include="NSubstitute" Version="\d+\.\d+.\d+"' `
			-Path "$($t.FullPath)" | Should -Be $true;
		select-string `
			"<ProjectReference Include=""\.\..+(\\|/)$($s.Name).WebApi.csproj"" />" `
			-Path "$outputPath/Tests/$($s.Name).Tests.csproj" | Should -Be $true;
		select-string `
			"<ProjectReference Include=""\.\..+(\\|/)$($s.Name).Domain.csproj"" />" `
			-Path "$outputPath/WebApi/$($s.Name).WebApi.csproj" | Should -Be $true;
	}
}