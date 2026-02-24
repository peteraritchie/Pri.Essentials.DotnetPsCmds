using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Pri.Essentials.DotnetProjects;

/// <summary>
/// Supported names for XML elements in csproj files.
/// </summary>
public static class ProjectElementNames
{
	/// <summary>
	/// A strongly-typed enum pattern implementation of
	/// SuppressMessageAttribute Scope property values.
	/// </summary>
	public sealed class ScopeName
	{
		private readonly string scopeName;

		private ScopeName(string scopeName)
		{
			this.scopeName = scopeName;
		}

		/// <summary>namespaceanddescendants</summary>
		public static readonly ScopeName NamespaceAndDescendants = new("namespaceanddescendants");
		/// <summary>namespace</summary>
		public static readonly ScopeName Namespace = new("namespace");
		/// <summary>member</summary>
		public static readonly ScopeName Member = new("member");
		/// <summary>module</summary>
		public static readonly ScopeName Module = new("module");
		/// <summary>type</summary>
		public static readonly ScopeName Type = new("type");

		/// <summary>
		/// Gets a read-only dictionary containing all supported scope names
		/// mapped to their corresponding <see cref="ScopeName"/> instances.
		/// </summary>
		/// <remarks>
		/// The dictionary keys are string representations of supported output
		/// types, and the values are the associated <see cref="ScopeName"/>
		/// objects. This collection can be used to enumerate or look up
		/// output types by name.
		/// </remarks>
		public static readonly IReadOnlyDictionary<string, ScopeName> All =
			new Dictionary<string, ScopeName>
			{
				{NamespaceAndDescendants, NamespaceAndDescendants},
				{Namespace, Namespace},
				{Member, Member},
				{Module, Module},
				{Type, Type}
			};

		/// <inheritdoc />
		[ExcludeFromCodeCoverage]
		public override string ToString() => scopeName;

		/// <summary>
		/// Implicitly converts a <see cref="ScopeName"/> to its string
		/// representation.
		/// </summary>
		/// <param name="scopeName">
		/// The <see cref="ScopeName"/> instance to convert.
		/// </param>
		/// <returns>The string value of the scope name.</returns>
		public static implicit operator string(ScopeName scopeName) =>
			scopeName.scopeName;
	}

	/// <summary>Project</summary>
	public const string Project = "Project";

	/// <summary>Project/PropertyGroup</summary>
	public const string PropertyGroup = "PropertyGroup";
	/// <summary>Project/ItemGroup</summary>
	public const string ItemGroup = "ItemGroup";

	#region ItemGroup properties

	/// <summary>ItemGroup/AssemblyAttribute</summary>
	public const string AssemblyAttribute = "AssemblyAttribute";
	/// <summary>ItemGroup/AssemblyAttribute.Include property</summary>
	public const string Include = "Include";

	/// <summary><code>_Parameter1</code> property for AssemblyAttribute</summary>
	public const string Parameter1 = "_Parameter1";

	/// <summary><code>_Parameter2</code> property for AssemblyAttribute</summary>
	public const string Parameter2 = "_Parameter2";

	/// <summary>ItemGroup Justification property for AssemblyAttribute SuppressMessageAttribute</summary>
	public const string Justification = nameof(SuppressMessageAttribute.Justification);
	/// <summary>Scope property for AssemblyAttribute SuppressMessageAttribute</summary>
	public const string Scope = nameof(SuppressMessageAttribute.Scope);
#if UNSUPPORTABLE
	/// <summary>Scope property for AssemblyAttribute SuppressMessageAttribute</summary>
	public const string Target = nameof(SuppressMessageAttribute.Target);
#endif

	/// <summary>ItemGroup/InternalsVisibleTo</summary>
	public static readonly string InternalsVisibleToName
		= nameof(InternalsVisibleToAttribute)[..^9];
#endregion // ItemGroup

	#region PropertyGroup properties

	/// <summary>AppendTargetFrameworkToOutputPath</summary>
	public const string AppendTargetFrameworkToOutputPath
		= "AppendTargetFrameworkToOutputPath";
	/// <summary>AssemblyName</summary>
	public const string AssemblyName = "AssemblyName";
	/// <summary>GenerateDocumentationFile</summary>
	public const string GenerateDocumentationFile
		= "GenerateDocumentationFile";
	/// <summary>IntermediateOutputPath</summary>
	public const string IntermediateOutputPath
		= "IntermediateOutputPath";
	/// <summary>LangVersion</summary>
	public const string LangVersion = "LangVersion";
	/// <summary>Nullable</summary>
	public const string Nullable = "Nullable";
	/// <summary>OutputType</summary>
	public const string OutputType = "OutputType";
	/// <summary>OutputPath</summary>
	public const string OutputPath = "OutputPath";
	/// <summary>RestorePackagesWithLockFile</summary>
	public const string RestorePackagesWithLockFile
		= "RestorePackagesWithLockFile";
	/// <summary>TargetFramework</summary>
	public const string TargetFramework = "TargetFramework";
	/// <summary>Version</summary>
	public const string Version = "Version";
	/// <summary>VersionPrefix</summary>
	public const string VersionPrefix = "VersionPrefix";
	/// <summary>VersionSuffix</summary>
	public const string VersionSuffix = "VersionSuffix";

	#endregion // PropertyGroup

	/// <summary>
	/// Gets a read-only collection containing the names of all supported
	/// properties.
	/// </summary>
	public static IReadOnlyCollection<string> AllProperties = [
		AppendTargetFrameworkToOutputPath,
		AssemblyName,
		GenerateDocumentationFile,
		IntermediateOutputPath,
		LangVersion,
		Nullable,
		OutputType,
		OutputPath,
		RestorePackagesWithLockFile,
		TargetFramework,
		Version,
		VersionPrefix,
		VersionSuffix,
	];

	[ExcludeFromCodeCoverage]
	static ProjectElementNames()
	{
	}
}