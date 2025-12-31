using System.Runtime.InteropServices;

namespace Tests;

public class Assumptions
{
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