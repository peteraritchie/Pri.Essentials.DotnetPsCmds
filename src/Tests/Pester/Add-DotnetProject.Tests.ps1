BeforeAll {
	Import-Module "$PSScriptRoot/../../Pri.Essentials.DotnetPsCmds/Pri.Essentials.DotnetPsCmds.psd1";
	$productName = 'Pri.TheProduct';
	$outputPath = "$PSScriptRoot/TestProjects/$productName";
}

AfterAll {
	Remove-Module Pri.Essentials.DotnetPsCmds -Force
}

Describe 'Add-DotnetProject' {
	AfterEach {
		Remove-Item "$PSScriptRoot/TestProjects" -Recurse -Force -ErrorAction SilentlyContinue -ProgressAction SilentlyContinue;
	}

	It 'Given Solution, adds project correctly' {

		$p = New-DotnetProject 'classlib' -OutputDirectory "$outputPath/MyClassLib" -OutputName  'MyClassLib';

		$s = New-DotnetSolution -OutputDirectory $outputPath;

		Add-DotnetProject -Solution $s -Project $p;

		Test-Path "$outputPath/$productName.sln" | Should -Be $true;
		select-string `
			'"MyClassLib", "MyClassLib(\\|/)MyClassLib.csproj"' `
			-Path "$outputPath/$productName.sln" | Should -Be $true;

	}
}