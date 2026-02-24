using System.Runtime.InteropServices;

using Pri.Essentials.Abstractions;

namespace Tests.Integration;

public class FileSystemShould
{
	[Fact]
	[Trait("Category", "Integration")]
	void OpenFileCorrectly()
	{
		var tempFile = Path.GetTempFileName();
		Assert.True(File.Exists(tempFile));
		try
		{
			File.WriteAllText(tempFile, "867-5309");
			Assert.True(File.Exists(tempFile));
			IFileSystem fs = IFileSystem.Default;
			var stream = fs.FileOpen(tempFile, FileMode.Open, FileAccess.Read, FileShare.Read);
			using var reader = new StreamReader(stream);
			Assert.Equal("867-5309", reader.ReadToEnd());
		}
		finally
		{
			File.Delete(tempFile);
		}
	}

	[Fact]
	[Trait("Category", "Integration")]
	public void DeleteFileCorrectly()
	{
		var tempFile = Path.GetTempFileName();
		Assert.True(File.Exists(tempFile));
		try
		{
			IFileSystem.Default.DeleteFile(tempFile);
			Assert.False(File.Exists(tempFile));
		}
		catch
		{
			if(File.Exists(tempFile))
			{
				File.Delete(tempFile);
			}
		}
	}

	[Fact]
	public void CheckDirectoryExistsCorrectly()
	{
		Assert.SkipUnless(RuntimeInformation.IsOSPlatform(OSPlatform.Windows),
			"Test that requires Windows environment.");
		var tempDir = Path.GetTempPath();
		Assert.True(IFileSystem.Default.DirectoryExists(tempDir));
	}

	[Fact]
	[Trait("Category", "Integration")]
	public void CheckExistsFileCorrectly()
	{
		var tempFile = Path.GetTempFileName();
		try
		{
			Assert.True(File.Exists(tempFile));
			Assert.True(IFileSystem.Default.Exists(tempFile));
		}
		finally
		{
			File.Delete(tempFile);
		}
	}

	[Fact]
	public void CheckExistsDirectoryCorrectly()
	{
		Assert.SkipUnless(RuntimeInformation.IsOSPlatform(OSPlatform.Windows),
			"Test that requires Windows environment.");
		var tempDir = Path.GetTempPath();
		Assert.True(IFileSystem.Default.Exists(tempDir));
	}

	[Fact]
	[Trait("Category", "Integration")]
	public void CheckFileExistsCorrectly()
	{
		var tempFile = Path.GetTempFileName();
		try
		{
			Assert.True(File.Exists(tempFile));
			Assert.True(IFileSystem.Default.FileExists(tempFile));
		}
		finally
		{
			File.Delete(tempFile);
		}
	}
}
