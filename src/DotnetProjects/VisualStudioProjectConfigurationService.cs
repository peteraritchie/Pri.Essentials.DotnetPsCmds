using System;
using System.IO;
using System.Xml.Linq;

namespace Pri.Essentials.DotnetProjects;

/// <summary>
/// Provides services for configuring Visual Studio project files.
/// </summary>
/// <remarks>
/// This service assumes that options are applied unconditionally in the
/// project file; that is, they are applied to a singe unattributed
/// ProgramGroup element in the root (&lt;Project&gt;) element.
/// </remarks>
public static class VisualStudioProjectConfigurationService
{
	/// <summary>
	/// Adds the RestorePackagesWithLockFile property if it doesn't exist,
	/// and sets it to true
	/// </summary>
	/// <param name="filePath"></param>
	/// <exception cref="InvalidOperationException"></exception>
	public static void SetRestorePackagesWithLockFile(string filePath)
	{
		using FileStream fs = new(filePath,
			FileMode.Open,
			FileAccess.ReadWrite,
			FileShare.None);
		SetRestorePackagesWithLockFile(fs, true);
	}

	/// <summary>
	/// Adds the RestorePackagesWithLockFile property if it doesn't exist,
	/// and sets it to true
	/// </summary>
	/// <param name="stream">
	/// The stream representing the project file to update. The stream must be
	/// writable and positioned at the start of the project file content.
	/// </param>
	/// <exception cref="InvalidOperationException"></exception>
	public static void EnableRestorePackagesWithLockFile(Stream stream)
	{
		SetRestorePackagesWithLockFile(stream, true);
	}

	private static void SetRestorePackagesWithLockFile(Stream stream, bool enable)
	{
		var doc = XDocument.Load(stream);
		var projectElement = doc.Root!;
		if (projectElement.Name != ProjectElementNames.Project)
		{
			throw new InvalidOperationException(
				"Root element must be <Project>.");
		}

		var propertyGroupElement
			= projectElement.Element(ProjectElementNames.PropertyGroup) ??
			  throw new InvalidOperationException(
				  "No <PropertyGroup> element found in project file.");

		if (TrySetProjectOption(propertyGroupElement,
			    ProjectElementNames.RestorePackagesWithLockFile,
			    enable ? "true" : "false"))
		{
			// Reset position before saving
			stream.Position = 0;
			doc.Save(stream);
		}
	}

	/// <summary>
	/// Enables the generation of XML documentation file output for the
	/// specified project file stream.
	/// </summary>
	/// <param name="stream">
	/// The stream representing the project file to update. The stream must be
	/// writable and positioned at the start of the project file content.
	/// </param>
	public static void EnableGenerateDocumentationFile(Stream stream)
	{
		SetGenerateDocumentationFile(stream, true);
	}

	private static void SetGenerateDocumentationFile(Stream stream, bool enable)
	{
		var doc = XDocument.Load(stream);
		var projectElement = doc.Root!;
		if (projectElement.Name != ProjectElementNames.Project)
		{
			throw new InvalidOperationException(
				"Root element must be <Project>.");
		}

		var propertyGroupElement
			= projectElement.Element(ProjectElementNames.PropertyGroup) ??
			  throw new InvalidOperationException(
				  "No <PropertyGroup> element found in project file.");

		if (TrySetProjectOption(propertyGroupElement,
			    ProjectElementNames.GenerateDocumentationFile,
			    enable ? "true" : "false"))
		{
			// Reset position before saving
			stream.Position = 0;
			doc.Save(stream);
		}
	}

	private static bool TrySetProjectOption(XElement propertyGroupElement,
		string optionName,
		string value)
	{
		var option = propertyGroupElement.Element(optionName);
		if (option != null)
		{
			return false;
		}

		propertyGroupElement.Add(
			new XElement(
				optionName,
				value));
		return true;
	}
}