using System.Runtime.InteropServices;
using System.Xml.Linq;
using System.Xml.XPath;

using Pri.Essentials.DotnetProjects;

namespace Tests;

public class Assumptions
{
	[Fact]
	void XPathAssumptions()
	{
		var xmlText = """
			<Project Sdk="Microsoft.NET.Sdk">

			  <PropertyGroup>
			    <TargetFramework>netstandard2.0</TargetFramework>
			    <LangVersion>latest</LangVersion>
			    <Nullable>enable</Nullable>
			    <AssemblyName>Pri.Essentials.DotnetPsCmds</AssemblyName>
			    <GenerateDocumentationFile>True</GenerateDocumentationFile>
			    <RestorePackagesWithLockFile>true</RestorePackagesWithLockFile>
			  </PropertyGroup>

			  <ItemGroup>
			    <PackageReference Include="PowerShellStandard.Library" Version="5.1.1">
			      <PrivateAssets>All</PrivateAssets>
			    </PackageReference>
			  </ItemGroup>

			  <ItemGroup>
					<AssemblyAttribute Include="System.Diagnostics.CodeAnalysis.SuppressMessageAttribute">
					  <_Parameter1>Style</_Parameter1>
					  <_Parameter2>IDE1006:Naming Styles</_Parameter2>
					  <Justification>&lt;Pending&gt;</Justification>
					  <Scope>module</Scope>
					</AssemblyAttribute>
			  </ItemGroup>
			</Project>
			""";
		var doc = XDocument.Parse(xmlText);
		var projectElement = doc.Root!;

		var category = "Style";
		var checkId = "IDE1006:Naming Styles";
		var justification = "<Pending>";
		var scope = ProjectElementNames.ScopeName.Module;
		var existingElement = projectElement.XPathSelectElement(
			$"./ItemGroup[./AssemblyAttribute]");
		existingElement = projectElement.XPathSelectElement(
			$"./ItemGroup[./AssemblyAttribute[ @Include='System.Diagnostics.CodeAnalysis.SuppressMessageAttribute' and _Parameter1='{category}' and _Parameter2 = '{checkId}' and Justification = '{justification}' and Scope = '{scope}']]");
		// /Project/ItemGroup[./AssemblyAttribute[ @Include='System.Diagnostics.CodeAnalysis.SuppressMessage' and _Parameter1='{category}' and _Parameter2 = '{checkId' and Justification = '{justification}' and Scope = '{scope']]
		// /Project/ItemGroup[./AssemblyAttribute[ @Include='System.Diagnostics.CodeAnalysis.SuppressMessage']]
		// /Project/ItemGroup[./AssemblyAttribute[ @Include='System.Diagnostics.CodeAnalysis.SuppressMessage' and _Parameter1='Style' and _Parameter2 = 'IDE1006:Naming Styles' and Justification = '<Pending>' and Scope = 'module']]
		//./        ItemGroup[./AssemblyAttribute[ @Include='System.Diagnostics.CodeAnalysis.SuppressMessage' and _Parameter1='Style' and _Parameter2 = 'IDE1006:Naming Styles' and Justification = '<Pending>' and Scope = 'module']]
		// /Project/ItemGroup[AssemblyAttribute/_Parameter1 = '{category}' and AssemblyAttribute/_Parameter2 = '{checkId}' and AssemblyAttribute/Justification = '{justification}' and AssemblyAttribute/Scope = '{scope}']");
		Assert.NotNull(existingElement);
	}

	[Fact]
	void CreateItemGroup()
	{
		var doc = XDocument.Parse("""
		                          <Project Sdk="Microsoft.NET.Sdk">
		                            <PropertyGroup>
		                              <TargetFramework>netstandard2.0</TargetFramework>
		                              <LangVersion>latest</LangVersion>
		                              <Nullable>enable</Nullable>
		                              <RestorePackagesWithLockFile>false</RestorePackagesWithLockFile>
		                            </PropertyGroup>
		                          </Project>
		                          """);
		var projectElement = doc.XPathSelectElement($"/{ProjectElementNames.Project}");
		Assert.NotNull(projectElement);

	}

	[Fact]
	void FindElementInItemGroup()
	{
		var doc = XDocument.Parse("""
		                          <Project Sdk="Microsoft.NET.Sdk">
		                            <PropertyGroup>
		                              <TargetFramework>netstandard2.0</TargetFramework>
		                              <LangVersion>latest</LangVersion>
		                              <Nullable>enable</Nullable>
		                              <RestorePackagesWithLockFile>false</RestorePackagesWithLockFile>
		                            </PropertyGroup>
		                            <ItemGroup>
		                          	  <InternalsVisibleTo>Tests</InternalsVisibleTo>
		                          	</ItemGroup>
		                          </Project>
		                          """);
		var element = doc.XPathSelectElement($"/{ProjectElementNames.Project}/{ProjectElementNames.ItemGroup}");
		Assert.NotNull(element);
		element = doc.XPathSelectElement($"/{ProjectElementNames.Project}/{ProjectElementNames.ItemGroup}/{ProjectElementNames.InternalsVisibleToName}");
		Assert.NotNull(element);
		element = element.Parent;
		Assert.NotNull(element);
		Assert.Equal("ItemGroup", element.Name);
	}

	[Fact]
	void CreateCollectionOfDelegatesToExecute()
	{
		var doc = XDocument.Parse("""
		                          <Project Sdk="Microsoft.NET.Sdk">
		                            <PropertyGroup>
		                              <TargetFramework>netstandard2.0</TargetFramework>
		                              <LangVersion>latest</LangVersion>
		                              <Nullable>enable</Nullable>
		                              <RestorePackagesWithLockFile>false</RestorePackagesWithLockFile>
		                            </PropertyGroup>
		                          </Project>
		                          """);
		var service = new VisualStudioProjectConfigurationService(doc);
		Dictionary<string, Action> actions = [];
		actions["name"]= () => service.SetAssemblyName("something");
		foreach (var (name, action) in actions) {
			action();
		}

		var element = doc.XPathSelectElement("/Project/PropertyGroup/AssemblyName");
		Assert.NotNull(element);
		Assert.Equal("something", element.Value);
	}

	/// <summary>
	/// Searches parent directories from the current working directory upward until a file named "global.json" is found or
	/// the root directory is reached. Asserts that the file exists in one of the directories traversed.
	/// </summary>
	/// <remarks>This test is typically used to verify that a solution-level configuration file, such as
	/// "global.json", is present in the directory hierarchy above the current working directory. The assertion fails if
	/// the file cannot be found in any parent directory up to the root.</remarks>
	[Fact]
	void RecurseUpUntil()
	{
		var fileName = "global.json";
		var currentDir = Environment.CurrentDirectory;

		while (currentDir != null
		       && !File.Exists(Path.Combine(currentDir, fileName))
		       && Path.GetPathRoot(currentDir) != currentDir)
		{
			currentDir = Directory.GetParent(currentDir)?.FullName;
		}
		Assert.NotNull(currentDir);
	}

	[Fact]
	void MultipleDirCombine()
	{
		var dirs = "Namespace.ClassName".Split(".");
		var path = Path.Combine(["RootDir",.. dirs]);
		Assert.Equal($"RootDir{Path.DirectorySeparatorChar}Namespace{Path.DirectorySeparatorChar}ClassName", path);
	}

	[Fact]
	void MultipleDirCombineButOne()
	{
		var dirs = "Namespace.ClassName".Split(".");
		var path = Path.Combine(["RootDir",.. dirs[..^1]]);
		var name = dirs[^1];
		Assert.Equal("ClassName", name);
		Assert.Equal($"RootDir{Path.DirectorySeparatorChar}Namespace", path);
	}

	[Fact]
	void SingleDirCombine()
	{
		var dirs = "ClassName".Split(".");
		var name = dirs[^1];
		Assert.Equal("ClassName", name);
		var path = Path.Combine(["RootDir",.. dirs[..^1], name]);
		Assert.Equal($"RootDir{Path.DirectorySeparatorChar}ClassName", path);
	}

    [Fact]
    void ProcessingLeafPath()
    {
	    Assert.SkipUnless(RuntimeInformation.IsOSPlatform(OSPlatform.Windows), "Test that requires Windows environment.");
        var path = @"C:\dir1\dir2\dir3";
        Assert.Equal("dir3", Path.GetFileName(path));
    }

    [Fact]
    void PathCombine()
    {
	    Assert.SkipUnless(RuntimeInformation.IsOSPlatform(OSPlatform.Windows), "Test that requires Windows environment.");
        var path = @"C:\dir1\dir2\dir3";
        Assert.Equal(@"C:\dir1\dir2\dir3\file.ext", Path.Combine(path, Path.ChangeExtension("file", ".ext")));
    }

    [Fact]
    void PathSplit()
    {
	    Assert.SkipUnless(RuntimeInformation.IsOSPlatform(OSPlatform.Windows), "Test that requires Windows environment.");
		var path = @"C:\dir1\dir2\dir3\file.ext";
		Assert.Equal("file", Path.GetFileNameWithoutExtension(path));
		Assert.Equal(@"C:\dir1\dir2\dir3", Path.GetDirectoryName(path));
    }

	[Fact]
	public void C()
	{
		Assert.SkipUnless(RuntimeInformation.IsOSPlatform(OSPlatform.Windows), "Test that requires Windows environment.");
		var dir = "dir";
		Assert.False(Path.IsPathRooted(dir));
		var currentDir = @"c:\sub";
		dir = Path.Combine(currentDir, dir);
		Assert.Equal(@"c:\sub\dir", dir);
	}
}