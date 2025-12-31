using System.Xml.Linq;
using System.Xml.XPath;

using Pri.Essentials.DotnetProjects;

namespace Tests;

public class VisualStudioProjectConfigurationServiceShould
{
	[Theory]
	[InlineData("""
	            <Project Sdk="Microsoft.NET.Sdk">
	              <PropertyGroup>
	                <TargetFramework>netstandard2.0</TargetFramework>
	                <LangVersion>latest</LangVersion>
	                <Nullable>enable</Nullable>
	              </PropertyGroup>
	            </Project>
	            """)]
	[InlineData("""
	            <Project Sdk="Microsoft.NET.Sdk">
	              <PropertyGroup>
	                <TargetFramework>netstandard2.0</TargetFramework>
	                <LangVersion>latest</LangVersion>
	                <Nullable>enable</Nullable>
	                <RestorePackagesWithLockFile>true</RestorePackagesWithLockFile>
	              </PropertyGroup>
	            </Project>
	            """)]
	void SetRestorePackagesWithLockFileSuccessfully(string xmlText)
	{
		string? filePath = null;
		try
		{
			filePath = Path.GetTempFileName();
			File.WriteAllText(filePath, xmlText);

			VisualStudioProjectConfigurationService.SetRestorePackagesWithLockFile(filePath);

			var doc = XDocument.Load(filePath);
			var flag = doc.XPathSelectElement(
				"/Project/PropertyGroup/RestorePackagesWithLockFile");
			Assert.NotNull(flag);
		}
		finally
		{
			File.Delete(filePath!);
		}
	}

	[Theory]
	[InlineData("""
	            <NotProject Sdk="Microsoft.NET.Sdk">
	              <PropertyGroup>
	                <TargetFramework>netstandard2.0</TargetFramework>
	                <LangVersion>latest</LangVersion>
	                <Nullable>enable</Nullable>
	              </PropertyGroup>
	            </NotProject>
	            """)]
	void ThrowWithBadProject(string xmlText)
	{
		string? filePath = null;
		try
		{
			filePath = Path.GetTempFileName();
			File.WriteAllText(filePath, xmlText);

			Assert.Throws<InvalidOperationException>(() =>
				VisualStudioProjectConfigurationService.SetRestorePackagesWithLockFile(filePath));
		}
		finally
		{
			File.Delete(filePath!);
		}
	}
}