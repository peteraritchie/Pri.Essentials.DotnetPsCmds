namespace Pri.Essentials.DotnetProjects;

/// <summary>
/// A DTO for SuppressMessageAttribute information.
/// </summary>
public class SuppressMessageParameters
{
	/// <summary>
	/// the category identifying the classification of the attribute.
	/// </summary>
	public required string Category { get; init; }

	/// <summary>
	/// The identifier of the code analysis tool rule to be suppressed.
	/// </summary>
	public required string CheckId { get; init; }

	/// <summary>
	/// The justification for suppressing the message.
	/// </summary>
	public string? Justification { get; init; } = null;

	/// <summary>
	/// The scope of the code that is relevant for the attribute.
	/// </summary>
	public string? Scope { get; init; } = null;

	/// <summary>
	/// A fully qualified path that represents the code analysis target.
	/// </summary>
	public string? Target { get; init; } = null;
}