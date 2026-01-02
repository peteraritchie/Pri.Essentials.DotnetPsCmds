namespace Tests.Mocks;

/// <summary>
/// A MemoryStream subclass that captures the final byte array
/// upon closing the stream.
/// </summary>
internal class SpyMemoryStream : MemoryStream
{
	/// <summary>
	/// The data written to the stream when closed.
	/// </summary>
	public byte[]? FinalArray { get; set; }

	public override void Close()
	{
		FinalArray = ToArray();
		base.Close();
	}

	public override string? ToString()
	{
		return FinalArray != null
			? System.Text.Encoding.UTF8.GetString(FinalArray)
			: null;
	}
}