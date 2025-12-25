using System.Runtime.InteropServices;

using Microsoft.TemplateEngine.Abstractions.Installer;

namespace Tests;

public class Assumptions
{
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

    [Fact]
    void GetDotnetNewTemplates()
    {
		var path = Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\.templateengine\packages.json");
        Assert.SkipUnless(Path.Exists(path), "Skip if the file doesn't exist");
        using var stream = File.OpenRead(path);
        PackagesJson packagesJson = (PackagesJson)System.Text.Json.JsonSerializer.Deserialize(stream, typeof(PackagesJson))!;
        Assert.NotNull(packagesJson);
        var packages = Packages(packagesJson).ToList();
        Assert.NotEmpty(packages);
    }

    private static IEnumerable<string> Packages(PackagesJson packagesJson)
    {
        foreach (var p in packagesJson.Packages.Where(e=>e.Details != null && e.Details.ContainsKey("PackageId")))
        {
            yield return p.Details!["PackageId"];
        }
    }

}
#nullable disable
    public class PackagesJson
    {
        public TemplatePackageData[] Packages { get; set; }
    }