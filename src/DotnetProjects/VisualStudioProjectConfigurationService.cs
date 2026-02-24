using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;

using Pri.Essentials.DotnetProjects.Commands.Constants;

namespace Pri.Essentials.DotnetProjects;

/// <summary>
/// Provides services for configuring Visual Studio project files.
/// </summary>
/// <remarks>
/// This service assumes that options are applied unconditionally in the
/// project file; that is, they are applied to a singe unattributed
/// ProgramGroup element in the root (&lt;Project&gt;) element.
/// </remarks>
public class VisualStudioProjectConfigurationService
{
	private readonly XElement firstPropertyGroupElement;
	private readonly XElement projectElement;

	/// <summary>
	/// Initializes a new instance with an xml document.
	/// </summary>
	/// <param name="doc"></param>
	/// <exception cref="InvalidOperationException"></exception>
	public VisualStudioProjectConfigurationService(XDocument doc)
	{
		projectElement = doc.Root!;
		if (projectElement.Name != ProjectElementNames.Project)
		{
			throw new InvalidOperationException(
				"Root element must be <Project>.");
		}

		firstPropertyGroupElement
			= projectElement.Element(ProjectElementNames.PropertyGroup) ??
			  throw new InvalidOperationException(
				  "No <PropertyGroup> element found in project file.");
	}

	/// <summary>
	/// Adds the RestorePackagesWithLockFile property if it doesn't exist,
	/// and sets it to <paramref name="enable"/>
	/// </summary>
	/// <param name="enable"></param>
	/// <exception cref="InvalidOperationException"></exception>
	public void SetRestorePackagesWithLockFile(bool enable = true)
	{
		_ = TrySetProjectOption(firstPropertyGroupElement,
			    ProjectElementNames.RestorePackagesWithLockFile,
			    enable ? "true" : "false");
	}

	/// <summary>
	/// Sets the target framework for the project using the specified framework name.
	/// </summary>
	/// <param name="frameworkName">The framework to set as the project's
	/// target. Must be a valid value from
	/// <see cref="SupportedFrameworkName"/>.</param>
	public void SetTargetFramework(SupportedFrameworkName frameworkName)
	{
		_ = TrySetProjectOption(firstPropertyGroupElement,
			    ProjectElementNames.TargetFramework,
			    frameworkName);
	}

	/// <summary>
	/// Sets the nullable context for the project to enable or
	/// disable nullable reference type annotations.
	/// </summary>
	/// <param name="enable">A value indicating whether nullable reference
	/// type annotations should be enabled. Specify <see langword="true"/> to
	/// enable nullable context; otherwise, <see langword="false"/>.</param>
	public void SetNullable(bool enable = true)
	{
		_ = TrySetProjectOption(firstPropertyGroupElement,
			    ProjectElementNames.Nullable,
			    enable ? "enable" : "disable");
	}

	/// <summary>
	/// Sets the AppendTargetFrameworkToOutputPath flag.
	/// </summary>
	/// <param name="enable">
	/// A value indicating whether enable or disable.
	/// </param>
	public void SetAppendTargetFrameworkToOutputPath(bool enable = true)
	{
		_ = TrySetProjectOption(firstPropertyGroupElement,
			    ProjectElementNames.AppendTargetFrameworkToOutputPath,
			    enable ? "true" : "false");

	}

	/// <summary>
	/// Sets the GenerateDocumentationFile flag.
	/// </summary>
	/// <param name="enable">
	/// A value indicating whether enable or disable.
	/// </param>
	public void SetGenerateDocumentationFile(bool enable = true)
	{
		_ = TrySetProjectOption(firstPropertyGroupElement,
				ProjectElementNames.GenerateDocumentationFile,
				enable ? "true" : "false");
	}

	/// <summary>
	/// Sets the AssemblyName value.
	/// </summary>
	/// <param name="valueText">
	/// The value to set.
	/// </param>
	public void SetAssemblyName(string valueText)
	{
		_ = TrySetProjectOption(firstPropertyGroupElement,
				ProjectElementNames.AssemblyName,
				valueText);
	}


	/// <summary>
	/// Sets the IntermediateOutputPath value.
	/// </summary>
	/// <param name="valueText">
	/// The value to set.
	/// </param>
	public void SetIntermediateOutputPath(string valueText)
	{
		_ = TrySetProjectOption(firstPropertyGroupElement,
				ProjectElementNames.IntermediateOutputPath,
				valueText);
	}


	/// <summary>
	/// Sets the LangVersion value.
	/// </summary>
	/// <param name="valueText">
	/// The value to set.
	/// </param>
	public void SetLangVersion(string valueText)
	{
		_ = TrySetProjectOption(firstPropertyGroupElement,
				ProjectElementNames.LangVersion,
				valueText);
	}


	/// <summary>
	/// Sets the OutputPath value.
	/// </summary>
	/// <param name="valueText">
	/// The value to set.
	/// </param>
	public void SetOutputPath(string valueText)
	{
		_ = TrySetProjectOption(firstPropertyGroupElement,
				ProjectElementNames.OutputPath,
				valueText);
	}


	/// <summary>
	/// Sets the OutputType value.
	/// </summary>
	/// <param name="outputType">
	/// The value to set.
	/// </param>
	public void SetOutputType(AssemblyOutputType outputType)
	{
		_ = TrySetProjectOption(firstPropertyGroupElement,
				ProjectElementNames.OutputType,
				outputType);
	}


	/// <summary>
	/// Sets the Version value.
	/// </summary>
	/// <param name="valueText">
	/// The value to set.
	/// </param>
	public void SetVersion(string valueText)
	{
		_ = TrySetProjectOption(firstPropertyGroupElement,
				ProjectElementNames.Version,
				valueText);
	}


	/// <summary>
	/// Sets the VersionPrefix value.
	/// </summary>
	/// <param name="valueText">
	/// The value to set.
	/// </param>
	public void SetVersionPrefix(string valueText)
	{
		_ = TrySetProjectOption(firstPropertyGroupElement,
				ProjectElementNames.VersionPrefix,
				valueText);
	}


	/// <summary>
	/// Sets the VersionSuffix value.
	/// </summary>
	/// <param name="valueText">
	/// The value to set.
	/// </param>
	public void SetVersionSuffix(string valueText)
	{
		_ = TrySetProjectOption(firstPropertyGroupElement,
				ProjectElementNames.VersionSuffix,
				valueText);
	}

	// ReSharper disable once SuggestBaseTypeForParameter
	private static bool TrySetProjectOption(XElement propertyGroupElement,
		string optionName,
		string value)
	{
		var option = propertyGroupElement.Element(optionName);
		if (option != null)
		{
			option.Value = value;
			return true;
		}

		propertyGroupElement.Add(
			new XElement(
				optionName,
				value));
		return true;
	}

	/// <summary>
	/// Adds an InternalsVisibleTo entry for the specified assembly name to
	/// the project if it does not already exist.
	/// </summary>
	/// <remarks>This method ensures that duplicate InternalsVisibleTo
	/// entries are not added. Use this to allow another assembly to access
	/// internal members of the current project, typically for unit testing
	/// or friend assemblies.</remarks>
	/// <param name="assemblyName">
	/// The name of the assembly to grant internal access to.
	/// Cannot be null or empty.
	/// </param>
	public void AddFriendAssemblyName(string assemblyName)
	{
		var existingElement
			= TryFindExistingInternalsVisibleToElement(assemblyName);
		if (existingElement != null) return;
		var anyElement = projectElement.XPathSelectElement(
			$"./{ProjectElementNames.ItemGroup}/{ProjectElementNames.InternalsVisibleToName}");
		var itemGroupElement = anyElement != null ? anyElement.Parent! : CreateNewItemGroup();
		var newInternalsVisibleToElement = new XElement(ProjectElementNames.InternalsVisibleToName);
		newInternalsVisibleToElement.SetValue(assemblyName);
		itemGroupElement.Add(newInternalsVisibleToElement);
	}

	// ReSharper disable once SuggestBaseTypeForParameter
	private void AddAssemblyAttribute(XElement itemGroupElement,
		string fullyQualifiedTypeName,
		IReadOnlyDictionary<string, string>? elements = null)
	{
		XElement assemblyAttributeElement = new(ProjectElementNames.AssemblyAttribute);
		assemblyAttributeElement.SetAttributeValue(ProjectElementNames.Include, fullyQualifiedTypeName);
		foreach(var (childElementName, value) in elements ?? IReadOnlyDictionary<string,string>.Empty)
		{
			XElement childElement = new (childElementName) { Value = value };
			assemblyAttributeElement.Add(childElement);
		}
		itemGroupElement.Add(assemblyAttributeElement);
	}

	private XElement CreateNewItemGroup()
	{
		XElement itemGroupElement = new(ProjectElementNames.ItemGroup);
		projectElement.Add(itemGroupElement);
		return itemGroupElement;
	}


	//public void AddAssemblyAttribute<T>(string? parameter1 = null,
	//	string? parameter2 = null,
	//	IReadOnlyDictionary<string, string>? otherProperties = null) where T : Type
	//{
	//	// find ItemGroup with existing AssemblyAttribute (./Project/ItemGroup[AssemblyAttribute]);
	//	// if not found, find ItemGroup that doesn't have PackageReference or ProjectReference;
	//	// if not found, create a new one
	//	AddAssemblyAttribute(typeof(T).FullName!, parameter1, parameter2, otherProperties);
	//}

	/// <summary>
	/// Adds an AssemblyAttribute element for a SuppressMessageAttribute to
	/// the project file, specifying the code analysis rule to suppress and
	/// its associated metadata.
	/// </summary>
	/// <remarks>If an AssemblyAttribute element with the same category and
	/// checkId already exists, no new element is added. This method ensures
	/// that duplicate suppressions are not created.</remarks>
	/// <param name="category">
	/// The category of the code analysis rule to suppress. Cannot be null.
	/// </param>
	/// <param name="checkId">
	/// The identifier of the code analysis rule to suppress. Cannot be null.
	/// </param>
	/// <param name="justification">
	/// The justification for suppressing the code analysis rule. If null or
	/// empty, the justification is omitted.
	/// </param>
#if UNSUPPORTABLE
	/// <param name="scope">
	/// The scope of the suppression, such as "member" or "type". If null or
	/// empty, the scope is omitted.
	/// </param>
	/// <param name="target">
	/// The target of the suppression, such as the fully qualified name of the
	/// member or type. If null or empty, the target is omitted.</param>
#endif
	public void AddSuppressMessageAttributeAssemblyAttribute(string category,
		string checkId,
		string? justification
#if UNSUPPORTABLE
		string? scope,
		,string? target
#endif
		)
	{
		ArgumentNullException.ThrowIfNull(category);
		ArgumentNullException.ThrowIfNull(checkId);

		var elementValues = new Dictionary<string, string>
		{
			{ ProjectElementNames.Parameter1, category },
			{ ProjectElementNames.Parameter2, checkId }
		};
		var xPathPredicateBuilder = new StringBuilder(
			$"@Include='System.Diagnostics.CodeAnalysis.SuppressMessageAttribute'"
			+ $" and _Parameter1='{category}' and _Parameter2 = '{checkId}'");

		if (!string.IsNullOrWhiteSpace(justification))
		{
			xPathPredicateBuilder.Append($" and Justification = '{justification}'");
			elementValues.Add(ProjectElementNames.Justification, justification!);
		}
#if UNSUPPORTABLE
		if (!string.IsNullOrWhiteSpace(scope))
		{
			xPathPredicateBuilder.Append($" and Scope = '{scope}'");
			elementValues.Add(ProjectElementNames.Scope, scope!);
		}
		if (!string.IsNullOrWhiteSpace(target))
		{
			xPathPredicateBuilder.Append($" and Target = '{target}'");
			elementValues.Add(ProjectElementNames.Target, target!);
		}
#else
		elementValues.Add(ProjectElementNames.Scope,
			ProjectElementNames.ScopeName.Module);
#endif
		var xPath =
			$"./ItemGroup[./AssemblyAttribute[ {xPathPredicateBuilder} ]]";
		var existingElement = projectElement.XPathSelectElement(
			xPath);
		if (existingElement != null) return;

		// find any other AssemblyAttribute elements if none create a new ItemGroup
		existingElement = projectElement.XPathSelectElement(
			"./ItemGroup[./AssemblyAttribute]") ?? CreateNewItemGroup();

		AddAssemblyAttribute(existingElement,
			typeof(SuppressMessageAttribute).FullName!,
			elementValues);
	}

	/// <summary>
	/// exclude an entire assembly from code coverage
	/// </summary>
	public void AddExcludeFromCodeCoverageAssemblyAttribute()
	{
		// TODO:
	}

	/// <summary>
	/// Remove a friend assembly (InternalsVisibleTo) ItemGroup element
	/// from a project that matches the assembly name
	/// </summary>
	/// <remarks>Unused (only unit tested) for the time being.</remarks>
	/// <param name="assemblyName"></param>
	public void RemoveFriendAssembly(string assemblyName)
	{
		var existingElement
			= TryFindExistingInternalsVisibleToElement(assemblyName);
		existingElement?.Remove();
	}

	private XElement? TryFindExistingInternalsVisibleToElement(string assemblyName)
	{
		return projectElement.XPathSelectElement(
			$"./{ProjectElementNames.ItemGroup}"
			+ $"[{ProjectElementNames.InternalsVisibleToName}='{assemblyName}']");
	}
}
