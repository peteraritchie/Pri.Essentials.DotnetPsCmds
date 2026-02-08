using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Pri.Essentials.DotnetProjects;

/// <summary>
/// Supported names for XML elements in csproj files.
/// </summary>
public static class ProjectElementNames
{
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

	/// <summary>ItemGroup/AssemblyAttribute.Include value</summary>
	public static readonly string ExcludeFromCodeCoverageAttributeName
		= typeof(ExcludeFromCodeCoverageAttribute).FullName!;
	/// <summary>ItemGroup/AssemblyAttribute.Include value</summary>
	public static readonly string InternalsVisibleToAttributeName
		= typeof(InternalsVisibleToAttribute).FullName!;
	/// <summary>ItemGroup/AssemblyAttribute.Include value</summary>
	public static readonly string SuppressMessageAttributeName
		= typeof(SuppressMessageAttribute).FullName!;

	/// <summary>ItemGroup/InternalsVisibleTo</summary>
	public static readonly string InternalsVisibleToName
		= nameof(InternalsVisibleToAttribute)![..^9];

	public static readonly string ExcludeFromCodeCoverageName
		= nameof(ExcludeFromCodeCoverageAttribute)![..^9];
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