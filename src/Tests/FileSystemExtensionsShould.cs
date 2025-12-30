using Pri.Essentials.Abstractions;

namespace Tests;

public class FileSystemExtensionsShould
{
	[Fact]
	public void CreateSingleton()
	{
		var instance = IFileSystem.Default;
		Assert.Same(instance, IFileSystem.Default);
	}
}
