using System.Xml.Linq;
using System.Xml.XPath;

using Pri.Essentials.DotnetProjects;
using Pri.Essentials.DotnetProjects.Commands.Constants;

namespace Tests;

public class VisualStudioProjectConfigurationServiceShould
{
	[Fact]
	void ThrowOnMissingCategory()
	{
		var doc = XDocument.Parse("<Project><PropertyGroup/></Project>");
		var sut = new VisualStudioProjectConfigurationService(doc);
		Assert.Throws<ArgumentNullException>(() => sut.AddSuppressMessageAttributeAssemblyAttribute(null!, "CheckId", null
#if UNSUPPORTABLE
			, null, null
#endif
           ));
	}

	[Fact]
	void ThrowOnMissingCheckId()
	{
		var doc = XDocument.Parse("<Project><PropertyGroup/></Project>");
		var sut = new VisualStudioProjectConfigurationService(doc);
		Assert.Throws<ArgumentNullException>(() => sut.AddSuppressMessageAttributeAssemblyAttribute("Category", null!, null
#if UNSUPPORTABLE
			, null, null
#endif
		));
	}

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
	                <RestorePackagesWithLockFile>false</RestorePackagesWithLockFile>
	              </PropertyGroup>
	            <ItemGroup>
	              <InternalsVisibleTo>Tests</InternalsVisibleTo>
	            </ItemGroup>
	            </Project>
	            """)]
	[InlineData("""
	            <Project Sdk="Microsoft.NET.Sdk">
	              <PropertyGroup>
	                <TargetFramework>netstandard2.0</TargetFramework>
	                <LangVersion>latest</LangVersion>
	                <Nullable>enable</Nullable>
	                <RestorePackagesWithLockFile>false</RestorePackagesWithLockFile>
	              </PropertyGroup>
	              <ItemGroup>
	                <AssemblyAttribute Include="System.Diagnostics.CodeAnalysis.SuppressMessageAttribute">
	                  <_Parameter1>Style</_Parameter1>
	                  <_Parameter2>IDE1006:Naming Styles</_Parameter2>
	                  <Justification>&lt;Pending&gt;</Justification>
	                  <Scope>module</Scope>
	                </AssemblyAttribute>
	              </ItemGroup>
	            </Project>
	            """)]
	[InlineData("""
	            <Project Sdk="Microsoft.NET.Sdk">
	              <PropertyGroup>
	                <TargetFramework>netstandard2.0</TargetFramework>
	                <LangVersion>latest</LangVersion>
	                <Nullable>enable</Nullable>
	                <RestorePackagesWithLockFile>false</RestorePackagesWithLockFile>
	              </PropertyGroup>
	              <ItemGroup>
	                <AssemblyAttribute Include="System.Diagnostics.CodeAnalysis.SuppressMessageAttribute">
	                  <_Parameter1>Other</_Parameter1>
	                  <_Parameter2>IDE1000</_Parameter2>
	                  <Justification>something</Justification>
	                  <Scope>module</Scope>
	                </AssemblyAttribute>
	              </ItemGroup>
	            </Project>
	            """)]
	void AddSuppressMessageAttributeSuccessfully(string xmlText)
	{
		var doc = XDocument.Parse(xmlText);
		var sut = new VisualStudioProjectConfigurationService(doc);
		sut.AddSuppressMessageAttributeAssemblyAttribute(
			category: "Style",
			checkId: "IDE1006:Naming Styles",
			justification: "<Pending>"
#if UNSUPPORTABLE
			scope: ProjectElementNames.ScopeName.Module,
			,target: null
#endif
			);
		var elements = doc.XPathSelectElements(
			"/Project/ItemGroup/AssemblyAttribute");
		Assert.NotEmpty(elements);
		var element = doc.XPathSelectElement(
			"/Project/ItemGroup[./AssemblyAttribute[ @Include='System.Diagnostics.CodeAnalysis.SuppressMessageAttribute' and _Parameter1='Style' and _Parameter2 = 'IDE1006:Naming Styles' and Justification = '<Pending>' and Scope = 'module']]");
		Assert.NotNull(element);
	}

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
	void AddSuppressMessageAttributeWithTargetSuccessfully(string xmlText)
	{
		var doc = XDocument.Parse(xmlText);
		var sut = new VisualStudioProjectConfigurationService(doc);
		sut.AddSuppressMessageAttributeAssemblyAttribute(
			category: "Maintainability",
			checkId: "PRm1001:XML Comments Not Complete",
			justification: "<Pending>"
#if UNSUPPORTABLE
			scope: "member",
			, target: "~M:System.Index.FromStart(System.Int32)~System.Index"
#endif
			);
		var elements = doc.XPathSelectElements(
			"/Project/ItemGroup/AssemblyAttribute");
		Assert.NotEmpty(elements);
		var element = doc.XPathSelectElement(
			"/Project/ItemGroup[./AssemblyAttribute[ @Include='System.Diagnostics.CodeAnalysis.SuppressMessageAttribute' and _Parameter1='Maintainability' and _Parameter2 = 'PRm1001:XML Comments Not Complete' and Justification = '<Pending>' and Scope = 'member']]");
		Assert.NotNull(element);
	}

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
	                <RestorePackagesWithLockFile>false</RestorePackagesWithLockFile>
	              </PropertyGroup>
	            <ItemGroup>
	              <InternalsVisibleTo>Tests</InternalsVisibleTo>
	            </ItemGroup>
	            </Project>
	            """)]
	void AddFriendAssemblySuccessfully(string xmlText)
	{
		var doc = XDocument.Parse(xmlText);
		var sut = new VisualStudioProjectConfigurationService(doc);
		sut.AddFriendAssemblyName("Tests");
		var elements = doc.XPathSelectElements(
			"/Project/ItemGroup/InternalsVisibleTo");
		var element = Assert.Single(elements);
		Assert.NotNull(element);
		Assert.Equal("Tests", element.Value);
	}

	[Theory]
	[InlineData("""
	            <Project Sdk="Microsoft.NET.Sdk">
	              <PropertyGroup>
	                <TargetFramework>netstandard2.0</TargetFramework>
	                <LangVersion>latest</LangVersion>
	                <Nullable>enable</Nullable>
	                <RestorePackagesWithLockFile>false</RestorePackagesWithLockFile>
	              </PropertyGroup>
	            <ItemGroup>
	              <InternalsVisibleTo>Tests</InternalsVisibleTo>
	            </ItemGroup>
	            </Project>
	            """)]
	void RemoveFriendAssemblySuccessfully(string xmlText)
	{
		var doc = XDocument.Parse(xmlText);
		var sut = new VisualStudioProjectConfigurationService(doc);
		sut.RemoveFriendAssembly("Tests");
		var elements = doc.XPathSelectElements(
			"/Project/ItemGroup/InternalsVisibleTo");
		Assert.Empty(elements);
	}

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
	                <RestorePackagesWithLockFile>false</RestorePackagesWithLockFile>
	              </PropertyGroup>
	            </Project>
	            """)]
	void SetRestorePackagesWithLockFileSuccessfully(string xmlText)
	{
		var doc = XDocument.Parse(xmlText);
		var sut = new VisualStudioProjectConfigurationService(doc);
		sut.SetRestorePackagesWithLockFile();
		var flags = doc.XPathSelectElements(
			"/Project/PropertyGroup/RestorePackagesWithLockFile");
		var flag = Assert.Single(flags);
		Assert.NotNull(flag);
		Assert.Equal("true", flag.Value);
	}

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
	                <TargetFramework>net10.0</TargetFramework>
	                <LangVersion>latest</LangVersion>
	                <Nullable>enable</Nullable>
	              </PropertyGroup>
	            </Project>
	            """)]
	void SetTargetFrameworkSuccessfully(string xmlText)
	{
		var doc = XDocument.Parse(xmlText);
		var sut = new VisualStudioProjectConfigurationService(doc);
		sut.SetTargetFramework(SupportedFrameworkName.Net10);
		var flags = doc.XPathSelectElements(
			"/Project/PropertyGroup/TargetFramework");
		var flag = Assert.Single(flags);
		Assert.NotNull(flag);
		Assert.Equal("net10.0", flag.Value);
	}

	//Nullable
	[Theory]
	[InlineData("""
	            <Project Sdk="Microsoft.NET.Sdk">
	              <PropertyGroup>
	                <TargetFramework>netstandard2.0</TargetFramework>
	                <LangVersion>latest</LangVersion>
	              </PropertyGroup>
	            </Project>
	            """)]
	[InlineData("""
	            <Project Sdk="Microsoft.NET.Sdk">
	              <PropertyGroup>
	                <TargetFramework>net10.0</TargetFramework>
	                <LangVersion>latest</LangVersion>
	                <Nullable>enable</Nullable>
	              </PropertyGroup>
	            </Project>
	            """)]
	void SetNullableSuccessfully(string xmlText)
	{
		var doc = XDocument.Parse(xmlText);
		var sut = new VisualStudioProjectConfigurationService(doc);
		sut.SetNullable();
		var flags = doc.XPathSelectElements(
			"/Project/PropertyGroup/Nullable");
		var flag = Assert.Single(flags);

		Assert.NotNull(flag);
		Assert.Equal("enable", flag.Value);
	}

	//LangVersion
	[Theory]
	[InlineData("""
	            <Project Sdk="Microsoft.NET.Sdk">
	              <PropertyGroup>
	                <TargetFramework>netstandard2.0</TargetFramework>
	              </PropertyGroup>
	            </Project>
	            """)]
	[InlineData("""
	            <Project Sdk="Microsoft.NET.Sdk">
	              <PropertyGroup>
	                <TargetFramework>net10.0</TargetFramework>
	                <LangVersion>latest</LangVersion>
	              </PropertyGroup>
	            </Project>
	            """)]
	void SetLangVersionSuccessfully(string xmlText)
	{
		var doc = XDocument.Parse(xmlText);
		var sut = new VisualStudioProjectConfigurationService(doc);
		sut.SetLangVersion("latest");
		var flags = doc.XPathSelectElements(
			"/Project/PropertyGroup/LangVersion");
		var flag = Assert.Single(flags);

		Assert.NotNull(flag);
		Assert.Equal("latest", flag.Value);
	}

	//AppendTargetFrameworkToOutputPath
	[Theory]
	[InlineData("""
	            <Project Sdk="Microsoft.NET.Sdk">
	              <PropertyGroup>
	                <TargetFramework>netstandard2.0</TargetFramework>
	              </PropertyGroup>
	            </Project>
	            """)]
	[InlineData("""
	            <Project Sdk="Microsoft.NET.Sdk">
	              <PropertyGroup>
	                <TargetFramework>net10.0</TargetFramework>
	                <AppendTargetFrameworkToOutputPath>true</AppendTargetFrameworkToOutputPath>
	              </PropertyGroup>
	            </Project>
	            """)]
	void SetAppendTargetFrameworkToOutputPathSuccessfully(string xmlText)
	{
		var doc = XDocument.Parse(xmlText);
		var sut = new VisualStudioProjectConfigurationService(doc);
		sut.SetAppendTargetFrameworkToOutputPath(false);
		var flags = doc.XPathSelectElements(
			"/Project/PropertyGroup/AppendTargetFrameworkToOutputPath");
		var flag = Assert.Single(flags);

		Assert.NotNull(flag);
		Assert.Equal("false", flag.Value);
	}

	//OutputType
	[Theory]
	[InlineData("""
	            <Project Sdk="Microsoft.NET.Sdk">
	              <PropertyGroup>
	                <TargetFramework>netstandard2.0</TargetFramework>
	              </PropertyGroup>
	            </Project>
	            """)]
	[InlineData("""
	            <Project Sdk="Microsoft.NET.Sdk">
	              <PropertyGroup>
	                <TargetFramework>net10.0</TargetFramework>
	                <OutputType>library</OutputType>
	              </PropertyGroup>
	            </Project>
	            """)]
	void SetOutputTypeSuccessfully(string xmlText)
	{
		var doc = XDocument.Parse(xmlText);
		var sut = new VisualStudioProjectConfigurationService(doc);
		sut.SetOutputType(AssemblyOutputType.Exe);
		var flags = doc.XPathSelectElements(
			"/Project/PropertyGroup/OutputType");
		var flag = Assert.Single(flags);

		Assert.NotNull(flag);
		Assert.Equal("exe", flag.Value);
	}

	//GenerateDocumentationFile
	[Theory]
	[InlineData("""
	            <Project Sdk="Microsoft.NET.Sdk">
	              <PropertyGroup>
	                <TargetFramework>netstandard2.0</TargetFramework>
	              </PropertyGroup>
	            </Project>
	            """)]
	[InlineData("""
	            <Project Sdk="Microsoft.NET.Sdk">
	              <PropertyGroup>
	                <TargetFramework>net10.0</TargetFramework>
	                <GenerateDocumentationFile>false</GenerateDocumentationFile>
	              </PropertyGroup>
	            </Project>
	            """)]
	void SetGenerateDocumentationFileSuccessfully(string xmlText)
	{
		var doc = XDocument.Parse(xmlText);
		var sut = new VisualStudioProjectConfigurationService(doc);
		sut.SetGenerateDocumentationFile();
		var flags = doc.XPathSelectElements(
			"/Project/PropertyGroup/GenerateDocumentationFile");
		var flag = Assert.Single(flags);

		Assert.NotNull(flag);
		Assert.Equal("true", flag.Value);
	}

	//AssemblyName
	[Theory]
	[InlineData("""
	            <Project Sdk="Microsoft.NET.Sdk">
	              <PropertyGroup>
	                <TargetFramework>netstandard2.0</TargetFramework>
	              </PropertyGroup>
	            </Project>
	            """)]
	[InlineData("""
	            <Project Sdk="Microsoft.NET.Sdk">
	              <PropertyGroup>
	                <TargetFramework>net10.0</TargetFramework>
	                <AssemblyName>SomethingElse</AssemblyName>
	              </PropertyGroup>
	            </Project>
	            """)]
	void SetAssemblyNameSuccessfully(string xmlText)
	{
		var doc = XDocument.Parse(xmlText);
		var sut = new VisualStudioProjectConfigurationService(doc);
		sut.SetAssemblyName("SomethingElse");
		var flags = doc.XPathSelectElements(
			"/Project/PropertyGroup/AssemblyName");
		var flag = Assert.Single(flags);

		Assert.NotNull(flag);
		Assert.Equal("SomethingElse", flag.Value);
	}

	//OutputPath
	[Theory]
	[InlineData("""
	            <Project Sdk="Microsoft.NET.Sdk">
	              <PropertyGroup>
	                <TargetFramework>netstandard2.0</TargetFramework>
	              </PropertyGroup>
	            </Project>
	            """)]
	[InlineData("""
	            <Project Sdk="Microsoft.NET.Sdk">
	              <PropertyGroup>
	                <TargetFramework>net10.0</TargetFramework>
	                <OutputPath>AnotherPath</OutputPath>
	              </PropertyGroup>
	            </Project>
	            """)]
	void SetOutputPathSuccessfully(string xmlText)
	{
		var doc = XDocument.Parse(xmlText);
		var sut = new VisualStudioProjectConfigurationService(doc);
		sut.SetOutputPath("AnotherPath");
		var flags = doc.XPathSelectElements(
			"/Project/PropertyGroup/OutputPath");
		var flag = Assert.Single(flags);

		Assert.NotNull(flag);
		Assert.Equal("AnotherPath", flag.Value);
	}

	//Version
	[Theory]
	[InlineData("""
	            <Project Sdk="Microsoft.NET.Sdk">
	              <PropertyGroup>
	                <TargetFramework>netstandard2.0</TargetFramework>
	              </PropertyGroup>
	            </Project>
	            """)]
	[InlineData("""
	            <Project Sdk="Microsoft.NET.Sdk">
	              <PropertyGroup>
	                <TargetFramework>net10.0</TargetFramework>
	                <Version>1.2.3</Version>
	              </PropertyGroup>
	            </Project>
	            """)]
	void SetVersionSuccessfully(string xmlText)
	{
		var doc = XDocument.Parse(xmlText);
		var sut = new VisualStudioProjectConfigurationService(doc);
		sut.SetVersion("1.2.3");
		var flags = doc.XPathSelectElements(
			"/Project/PropertyGroup/Version");
		var flag = Assert.Single(flags);

		Assert.NotNull(flag);
		Assert.Equal("1.2.3", flag.Value);
	}

	//VersionPrefix
	[Theory]
	[InlineData("""
	            <Project Sdk="Microsoft.NET.Sdk">
	              <PropertyGroup>
	                <TargetFramework>netstandard2.0</TargetFramework>
	              </PropertyGroup>
	            </Project>
	            """)]
	[InlineData("""
	            <Project Sdk="Microsoft.NET.Sdk">
	              <PropertyGroup>
	                <TargetFramework>net10.0</TargetFramework>
	                <VersionPrefix>1.2</VersionPrefix>
	              </PropertyGroup>
	            </Project>
	            """)]
	void SetVersionPrefixSuccessfully(string xmlText)
	{
		var doc = XDocument.Parse(xmlText);
		var sut = new VisualStudioProjectConfigurationService(doc);
		sut.SetVersionPrefix("1.2");
		var flags = doc.XPathSelectElements(
			"/Project/PropertyGroup/VersionPrefix");
		var flag = Assert.Single(flags);

		Assert.NotNull(flag);
		Assert.Equal("1.2", flag.Value);
	}

	//VersionSuffix
	[Theory]
	[InlineData("""
	            <Project Sdk="Microsoft.NET.Sdk">
	              <PropertyGroup>
	                <TargetFramework>netstandard2.0</TargetFramework>
	              </PropertyGroup>
	            </Project>
	            """)]
	[InlineData("""
	            <Project Sdk="Microsoft.NET.Sdk">
	              <PropertyGroup>
	                <TargetFramework>net10.0</TargetFramework>
	                <VersionSuffix>beta</VersionSuffix>
	              </PropertyGroup>
	            </Project>
	            """)]
	void SetVersionSuffixSuccessfully(string xmlText)
	{
		var doc = XDocument.Parse(xmlText);
		var sut = new VisualStudioProjectConfigurationService(doc);
		sut.SetVersionSuffix("beta");
		var flags = doc.XPathSelectElements(
			"/Project/PropertyGroup/VersionSuffix");
		var flag = Assert.Single(flags);

		Assert.NotNull(flag);
		Assert.Equal("beta", flag.Value);
	}

	//IntermediateOutputPath
	[Theory]
	[InlineData("""
	            <Project Sdk="Microsoft.NET.Sdk">
	              <PropertyGroup>
	                <TargetFramework>netstandard2.0</TargetFramework>
	              </PropertyGroup>
	            </Project>
	            """)]
	[InlineData("""
	            <Project Sdk="Microsoft.NET.Sdk">
	              <PropertyGroup>
	                <TargetFramework>net10.0</TargetFramework>
	                <IntermediateOutputPath>a\path</IntermediateOutputPath>
	              </PropertyGroup>
	            </Project>
	            """)]
	void SetIntermediateOutputPathSuccessfully(string xmlText)
	{
		var doc = XDocument.Parse(xmlText);
		var sut = new VisualStudioProjectConfigurationService(doc);
		sut.SetIntermediateOutputPath(@"a\path");
		var flags = doc.XPathSelectElements(
			"/Project/PropertyGroup/IntermediateOutputPath");
		var flag = Assert.Single(flags);

		Assert.NotNull(flag);
		Assert.Equal(@"a\path", flag.Value);
	}

	[Theory]
	[InlineData("""
	            <NotProject Sdk="Microsoft.NET.Sdk">
	              <PropertyGroup>
	                <TargetFramework>netstandard2.0</TargetFramework>
	              </PropertyGroup>
	            </NotProject>
	            """)]
	void InvalidXmlContentThrows(string xmlText)
	{
		var doc = XDocument.Parse(xmlText);
		Assert.Throws<InvalidOperationException>(() =>
		{
			_ = new VisualStudioProjectConfigurationService(doc);
		});
	}
}