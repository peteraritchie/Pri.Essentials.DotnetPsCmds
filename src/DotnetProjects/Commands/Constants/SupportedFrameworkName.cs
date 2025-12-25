using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Pri.Essentials.DotnetProjects.Commands.Constants;

public sealed class SupportedFrameworkName
{
	private readonly string name;
	private SupportedFrameworkName(string name)
	{
		this.name = name;
	}

	public static readonly SupportedFrameworkName Net10 = new("net10.0");
	public static readonly SupportedFrameworkName Net9 = new("net9.0");
	public static readonly SupportedFrameworkName Net8 = new("net8.0");
	public static readonly SupportedFrameworkName NetStandard20 = new("netstandard2.0");
	public static readonly SupportedFrameworkName NetStandard21 = new("netstandard2.1");

	public static readonly IReadOnlyDictionary<string, SupportedFrameworkName> All =
		new Dictionary<string, SupportedFrameworkName>
		{
			{Net10, Net10},
			{Net9, Net9},
			{Net8, Net8},
			{NetStandard20, NetStandard20},
			{NetStandard21, NetStandard21},
		};

	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	public override string ToString() => name;

	public static implicit operator string(SupportedFrameworkName supportedFrameworkName) =>
		supportedFrameworkName.name;

	/// <summary>Converts the string representation of the name of one of the constants to an equivalent object.
	/// The return value indicates whether the conversion succeeded.</summary>
	/// <param name="potentialName">The case-sensitive string representation of the SupportedFrameworkName name
	/// or underlying value to convert.</param>
	/// <param name="result">When this method returns, contains an object of type <c>SupportedFrameworkName</c> whose value
	/// is represented by <paramref name="potentialName" /> if the parse operation succeeds.
	/// If the parse operation fails, contains the default value of the underlying type of <c>TEnum</c>.
	/// This parameter is passed uninitialized.</param>
	/// <exception cref="T:System.ArgumentNullException">
	/// <paramref name="potentialName" /> is <see langword="null" />.</exception>
	/// /// <returns>
	/// <see langword="true" /> if the <paramref name="potentialName" /> parameter was converted successfully; otherwise, <see langword="false" />.</returns>
	public static bool TryParse(string potentialName, [NotNullWhen(returnValue: true)] out SupportedFrameworkName? result)
	{
		return All.TryGetValue(potentialName, out result);
	}
}