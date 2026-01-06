BeforeAll {
	Import-Module "$PSScriptRoot/../../Pri.Essentials.DotnetPsCmds/Pri.Essentials.DotnetPsCmds.psd1";
	$productName = 'Pri.TheProduct';
	$outputPath = "$PSScriptRoot/TestProjects/$productName";
}

AfterAll {
	Remove-Module Pri.Essentials.DotnetPsCmds -Force
}

Describe 'New-DotnetSolution' {
	AfterEach {
		Remove-Item "$PSScriptRoot/TestProjects" -Recurse -Force -ErrorAction SilentlyContinue -ProgressAction SilentlyContinue;
	}

	It 'Given OutputPath, creates solution correctly' {

		New-DotnetSolution -OutputDirectory $outputPath;

		Test-Path "$outputPath/$productName.sln" | Should -Be $true;
	}

	It 'Given OutputPath and OutputName, creates solution correctly' {

		New-DotnetSolution -OutputDirectory $outputPath -OutputName $productName;

		Test-Path "$outputPath/$productName.sln" | Should -Be $true;
	}

	It 'Pipes solution to Add-DotnetProject correctly' {

		$p = New-DotnetProject 'classlib' -OutputDirectory "$outputPath/MyClassLib" -OutputName  'MyClassLib';

		New-DotnetSolution -OutputDirectory $outputPath | Add-DotnetProject -Project $p;

		Test-Path "$outputPath/$productName.sln" | Should -Be $true;
		select-string `
			'"MyClassLib", "MyClassLib(\\|/)MyClassLib.csproj"' `
			-Path "$outputPath/$productName.sln" | Should -Be $true;
	}
}