using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Pri.Essentials.DotnetProjects.Commands.Constants;

/// <summary>
/// Represents a supported project template name used to identify and work with predefined project templates.
/// </summary>
/// <remarks>This type provides named instances for each supported project template, such as xUnit, Console, or
/// Blazor. It enables type-safe handling of project template names and supports conversion to and from their string
/// representations. Use the static members to access specific templates or enumerate all supported templates via the
/// All property.</remarks>
public sealed class SupportedProjectTemplateName
{
	private readonly string name;
	private SupportedProjectTemplateName(string name)
	{
		this.name = name;
	}

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
	public static readonly SupportedProjectTemplateName XUnit = new("xunit");
	public static readonly SupportedProjectTemplateName XUnit3 = new("xunit3");
	public static readonly SupportedProjectTemplateName Console = new("console");
	public static readonly SupportedProjectTemplateName ClassLib = new("classlib");
	public static readonly SupportedProjectTemplateName Blazor = new("blazor");
	public static readonly SupportedProjectTemplateName Worker = new("worker");
	public static readonly SupportedProjectTemplateName WebApi = new("webapi");
	public static readonly SupportedProjectTemplateName WinForms = new("winforms");
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

	/// <summary>
	/// Provides a read-only dictionary containing all supported project template names, keyed by their string identifiers.
	/// </summary>
	/// <remarks>The dictionary maps each template's string name to its corresponding <see
	/// cref="SupportedProjectTemplateName"/> instance. This collection can be used to enumerate or look up all available
	/// project templates by name.</remarks>
    public static readonly IReadOnlyDictionary<string, SupportedProjectTemplateName> All =
                    new Dictionary<string, SupportedProjectTemplateName>
                    {
                        {XUnit, XUnit},
                        {XUnit3, XUnit3},
                        {Console, Console},
                        {ClassLib, ClassLib},
                        {Blazor, Blazor},
                        {Worker, Worker},
                        {WebApi, WebApi},
                        {WinForms, WinForms}
                    };
	/// <inheritdoc />
	public override string ToString() => name;

	/// <summary>
	/// Converts a SupportedProjectTemplateName instance to its string representation.
	/// </summary>
	/// <remarks>This operator enables implicit conversion of a SupportedProjectTemplateName to a string, allowing
	/// instances to be used wherever a string is expected. The conversion returns the underlying name value of the
	/// SupportedProjectTemplateName.</remarks>
	/// <param name="supportedFrameworkName">The SupportedProjectTemplateName instance to convert.</param>
	public static implicit operator string(SupportedProjectTemplateName supportedFrameworkName) =>
		supportedFrameworkName.name;

	/// <summary>Converts the string representation of the name of one of the constants to an equivalent object.
	/// The return value indicates whether the conversion succeeded.</summary>
	/// <param name="potentialName">The case-sensitive string representation of the SupportedProjectTemplateName name
	/// or underlying value to convert.</param>
	/// <param name="result">When this method returns, contains an object of type <c>SupportedProjectTemplateName</c> whose value
	/// is represented by <paramref name="potentialName" /> if the parse operation succeeds.
	/// If the parse operation fails, contains the default value of the underlying type of <c>TEnum</c>.
	/// This parameter is passed uninitialized.</param>
	/// <exception cref="T:System.ArgumentNullException">
	/// <paramref name="potentialName" /> is <see langword="null" />.</exception>
	/// /// <returns>
	/// <see langword="true" /> if the <paramref name="potentialName" /> parameter was converted successfully; otherwise, <see langword="false" />.</returns>
	public static bool TryParse(string potentialName, [NotNullWhen(returnValue: true)] out SupportedProjectTemplateName? result)
	{
		return All.TryGetValue(potentialName, out result);
	}
}