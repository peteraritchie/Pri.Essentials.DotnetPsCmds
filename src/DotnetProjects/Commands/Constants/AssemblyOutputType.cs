using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Pri.Essentials.DotnetProjects.Commands.Constants;

public sealed class AssemblyOutputType
{
	private readonly string outputTypeName;

	private AssemblyOutputType(string outputTypeName)
	{
		this.outputTypeName = outputTypeName;
	}

	public static readonly AssemblyOutputType Exe = new("exe");
	public static readonly AssemblyOutputType Library = new("library");
	public static readonly AssemblyOutputType WinExe = new("winexe");
	public static readonly AssemblyOutputType Module = new("module");

	/// <summary>
	/// Gets a read-only dictionary containing all supported output type names mapped to their corresponding <see
	/// cref="AssemblyOutputType"/> instances.
	/// </summary>
	/// <remarks>The dictionary keys are string representations of supported output types, and the values are the
	/// associated <see cref="AssemblyOutputType"/> objects. This collection can be used to enumerate or look up
	/// output types by name.</remarks>
	public static readonly IReadOnlyDictionary<string, AssemblyOutputType> All =
		new Dictionary<string, AssemblyOutputType>
		{
			{Exe, Exe},
			{Library, Library},
			{WinExe, WinExe},
			{ Module, Module },
		};

	/// <inheritdoc />
	[ExcludeFromCodeCoverage]
	public override string ToString() => outputTypeName;

	/// <summary>
	///
	/// </summary>
	/// <param name="assemblyOutputType"></param>
	/// <returns></returns>
	public static implicit operator string(AssemblyOutputType assemblyOutputType) =>
		assemblyOutputType.outputTypeName;

	/// <summary>Converts the string representation of the name of one of the constants to an equivalent object.
	/// The return value indicates whether the conversion succeeded.</summary>
	/// <param name="potentialName">The case-sensitive string representation of the AssemblyOutputType name
	/// or underlying value to convert.</param>
	/// <param name="result">When this method returns, contains an object of type <c>AssemblyOutputType</c> whose value
	/// is represented by <paramref name="potentialName" /> if the parse operation succeeds.
	/// If the parse operation fails, contains the default value of the underlying type of <c>TEnum</c>.
	/// This parameter is passed uninitialized.</param>
	/// <exception cref="T:System.ArgumentNullException">
	/// <paramref name="potentialName" /> is <see langword="null" />.</exception>
	/// /// <returns>
	/// <see langword="true" /> if the <paramref name="potentialName" /> parameter was converted successfully; otherwise, <see langword="false" />.</returns>
	public static bool TryParse(string potentialName, [NotNullWhen(returnValue: true)] out AssemblyOutputType? result)
	{
		return All.TryGetValue(potentialName, out result);
	}

	/// <summary>
	/// Try to get a AssemblyOutputType object by name
	/// </summary>
	/// <param name="nameText"></param>
	/// <param name="name"></param>
	/// <returns></returns>
	public static bool TryFind(string nameText, out AssemblyOutputType name)
	{
		return All.TryGetValue(nameText, out name);
	}
}