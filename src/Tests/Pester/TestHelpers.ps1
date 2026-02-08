function Test-ProjectPropertyExists {
	param ([string]$projectFilePath, [string]$propertyName)
	[xml]$projectXml = Get-Content -Path $projectFilePath;
	$null -ne $projectXml.Project.PropertyGroup.$propertyName;
}

function Get-ProjectProperty
{
	param ([string]$projectFilePath, [string]$propertyName)
	[xml]$projectXml = Get-Content -Path $projectFilePath;
	return $projectXml.Project.PropertyGroup.$propertyName;
}
function Get-ItemProperty
{
	param ([string]$projectFilePath, [string]$propertyName)
	[xml]$projectXml = Get-Content -Path $projectFilePath;
	return $projectXml.Project.ItemGroup.$propertyName;
}

