namespace Tests;

public class ExceptionExtensionsShould
{
	[Fact]
	void NotThrowViaThrowIfNullOrWhiteSpace()
	{
		string? pName = "some text";
		ExceptionExtensions.ThrowIfNullOrWhiteSpace(pName);
	}

	[Fact]
	void ThrowCorrectArgumentNullExceptionViaThrowIfNullOrWhiteSpace()
	{
		string? pName = null;
		var ex = Assert.Throws<ArgumentNullException>(() => ExceptionExtensions.ThrowIfNullOrWhiteSpace(pName));
		Assert.Equal(nameof(pName), ex.ParamName);
		Assert.Equal("Value cannot be null. (Parameter 'pName')", ex.Message);
	}

	[Fact]
	void ThrowCorrectArgumentExceptionViaThrowIfNullOrWhiteSpace()
	{
		string? pName = string.Empty;
		var ex = Assert.Throws<ArgumentException>(() => ExceptionExtensions.ThrowIfNullOrWhiteSpace(pName));
		Assert.Equal(nameof(pName), ex.ParamName);
		Assert.Equal("The value cannot be an empty string or composed entirely of whitespace. (Parameter 'pName')", ex.Message);
	}

	[Fact]
	void ThrowCorrectArgumentNullExceptionViaThrowIfNull()
	{
		string? pName = null;
		var ex = Assert.Throws<ArgumentNullException>(() => ExceptionExtensions.ThrowIfNull(pName));
		Assert.Equal(nameof(pName), ex.ParamName);
		Assert.Equal("Value cannot be null. (Parameter 'pName')", ex.Message);
	}
}