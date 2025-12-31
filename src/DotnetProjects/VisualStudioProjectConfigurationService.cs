using System;
using System.IO;
using System.Xml.Linq;

using Tests;

namespace Pri.Essentials.DotnetProjects;

/// <summary>
/// Provides services for configuring Visual Studio project files.
/// </summary>
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
		SetRestorePackagesWithLockFile(fs);
	}

	private static void SetRestorePackagesWithLockFile(Stream fs)
	{
		var doc = XDocument.Load(fs);
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
			    true))
		{
			// Reset stream before saving
			fs.SetLength(0);
			fs.Position = 0;
			doc.Save(fs);
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

	private static void SetGenerateDocumentationFile(Stream fs, bool enable)
	{
		var doc = XDocument.Load(fs);
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
			    enable))
		{
			// Reset stream before saving
			fs.SetLength(0);
			fs.Position = 0;
			doc.Save(fs);
		}
	}

	private static bool TrySetProjectOption<T>(XElement propertyGroupElement,
		string optionName,
		T value)
	{
		var option = propertyGroupElement.Element(optionName);
		if (option != null)
		{
			return false;
		}

		propertyGroupElement.Add(
			new XElement(
				optionName,
				value?.ToString()));
		return true;
	}
}