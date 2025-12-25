using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Pri.Essentials.DotnetProjects.Commands.Constants;

public sealed class SupportedProjectTemplateName
{
	private readonly string name;
	private SupportedProjectTemplateName(string name)
	{
		this.name = name;
	}

	public static readonly SupportedProjectTemplateName XUnit = new("xunit");
	public static readonly SupportedProjectTemplateName XUnit3 = new("xunit3");
	public static readonly SupportedProjectTemplateName Console = new("console");
	public static readonly SupportedProjectTemplateName ClassLib = new("classlib");
	public static readonly SupportedProjectTemplateName Blazor = new("blazor");
	public static readonly SupportedProjectTemplateName Worker = new("worker");
	public static readonly SupportedProjectTemplateName WebApi = new("webapi");
	public static readonly SupportedProjectTemplateName WinForms = new("winforms");

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