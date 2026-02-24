using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text.RegularExpressions;
using System.Xml.Linq;

using Pri.Essentials.Abstractions;
using Pri.Essentials.DotnetProjects;

namespace Pri.Essentials.DotnetPsCmds;

/// <summary>
/// Edit_DotnetProject
/// </summary>
[Cmdlet(VerbsData.Edit, nameof(DotnetProject), SupportsShouldProcess = true)]
[OutputType(typeof(DotnetProject))]
// ReSharper disable once UnusedType.Global
public class EditDotnetProjectCmdlet : PSCmdlet
{
	/// <summary>
	/// The target project to modify.
	/// </summary>
	[Parameter(Mandatory = true, ParameterSetName = "WithProjectAndArray",
		Position = 0,
		ValueFromPipeline = true,
		HelpMessage = "What project to modify.")]
	[Parameter(Mandatory = true, ParameterSetName = "WithProjectAndObject",
		Position = 0,
		ValueFromPipeline = true,
		HelpMessage = "What project to modify.")]
	[Parameter(Mandatory = true, ParameterSetName = "WithProjectAndItemProperty",
		Position = 0,
		ValueFromPipeline = true,
		HelpMessage = "What project to modify.")]
	[Parameter(Mandatory = false, ParameterSetName = "WithProjectSuppressMessage",
		Position = 0,
		ValueFromPipeline = true,
		HelpMessage = "What project to modify.")]
	public DotnetProject? Project { get; set; }

	/// <summary>
	/// The path to the target project to modify.
	/// </summary>AndArray
	[Parameter(Mandatory = true, ParameterSetName = "WithPathAndArray",
		Position = 0,
		ValueFromPipeline = true,
		HelpMessage = "What project to modify.")]
	[Parameter(Mandatory = true, ParameterSetName = "WithPathAndObject",
		Position = 0,
		ValueFromPipeline = true,
		HelpMessage = "What project to modify.")]
	[Parameter(Mandatory = true, ParameterSetName = "WithPathAndItemProperty",
		Position = 0,
		ValueFromPipeline = true,
		HelpMessage = "What project to modify.")]
	[Parameter(Mandatory = false, ParameterSetName = "WithPathAndSuppressMessage",
		Position = 0,
		ValueFromPipeline = true,
		HelpMessage = "What project to modify.")]
	public string? Path{ get; set; }

	/// <summary>
	/// An array of strings that represent property name/value pairs.
	/// </summary>
	[Parameter(Mandatory = true, ParameterSetName = "WithProjectAndArray",
		HelpMessage = "The properties to set.")]
	[Parameter(Mandatory = true, ParameterSetName = "WithPathAndArray",
		HelpMessage = "The properties to set.")]
	public string[]? Properties { get; set; }

	/// <summary>
	/// Gets or sets the list of assembly names that are granted access to
	/// internal types and members of this assembly via the
	/// InternalsVisibleTo attribute.
	/// </summary>
	/// <remarks>Specify the names of friend assemblies that should be able
	/// to access internal members. Each entry should be the full name of an
	/// assembly. If no assemblies are specified, internal members remain
	/// inaccessible to other assemblies.</remarks>
	[Parameter(Mandatory = false, ParameterSetName = "WithProjectAndItemProperty")]
	[Parameter(Mandatory = false, ParameterSetName = "WithPathAndItemProperty")]
	[Alias("FriendAssemblies", "Friends")]
	// ReSharper disable once UnusedAutoPropertyAccessor.Global
	public string[]? InternalsVisibleTo { get; set; }
	// AssemblyMetadata?

	/// <summary>
	/// SuppressMessageAttribute details.
	/// </summary>
	/// <remarks>
	/// <code>
	/// -SuppressMessage @{Category="Category"; CheckId="PR1000"}
	/// </code>
	/// </remarks>
	[Parameter(Mandatory = false, ParameterSetName = "WithProjectSuppressMessage")]
	[Parameter(Mandatory = false, ParameterSetName = "WithPathAndSuppressMessage")]
	public SuppressMessageParameters? SuppressMessage { get; set; }

	/// <summary>
	/// The properties, as a hash table, to set in the project file.
	/// </summary>
	[Parameter(Mandatory = true, ParameterSetName = "WithProjectAndObject",
		HelpMessage = "The properties to set.")]
	[Parameter(Mandatory = true, ParameterSetName = "WithPathAndObject",
		HelpMessage = "The properties to set.")]
	// ReSharper disable once UnusedAutoPropertyAccessor.Global
	// ReSharper disable once CollectionNeverUpdated.Global
	public Hashtable? PropertiesObject { get; set; }

	/// <inheritdoc />
	protected override void BeginProcessing()
	{
		if (this.ParameterSetName is "WithPathAndObject" or "WithPathAndArray"
		    && string.IsNullOrWhiteSpace(Path))
		{
			ThrowTerminatingError(
				new ErrorRecord(
					new ArgumentException(
						$"Parameter {nameof(Path)} is required "
						+ $"when using parameter set '{this.ParameterSetName}'",
						nameof(Path)),
					errorId: "Missing Solution Parameter",
					errorCategory: ErrorCategory.InvalidArgument,
					targetObject: null));
		}

		base.BeginProcessing();
	}

	/// <inheritdoc />
	protected override void ProcessRecord()
	{
		if (this.ParameterSetName is "WithProjectAndObject" or "WithProjectAndArray"
		    && string.IsNullOrWhiteSpace(Path))
		{
			ThrowTerminatingError(
				new ErrorRecord(
					new ArgumentException(
						$"Parameter {nameof(Project)} is required "
						+ $"when using parameter set '{this.ParameterSetName}'",
						nameof(Project)),
					errorId: "Missing Solution Parameter",
					errorCategory: ErrorCategory.InvalidArgument,
					targetObject: null));
		}

		string path = EditDotnetProjectService.DetermineProjectFileFullPath(Project, Path, SessionState);

		var properties = new Dictionary<string, object>();
		if (Properties is not null)
		{
			foreach (var p in Properties)
			{
				var match = Regex.Match(p, "(.+):(.+)");
				if (!match.Success)
				{
					ThrowTerminatingError(
						new ErrorRecord(
							new ArgumentException(
								$"Parameter {nameof(Properties)} was not of valid format.",
								nameof(Properties)),
							errorId: "Invalid Properties",
							errorCategory: ErrorCategory.InvalidArgument,
							targetObject: null));
				}
				properties.Add(match.Groups[1].Value, match.Groups[2].Value);
			}
		}
		else if(PropertiesObject is not null)
		{
			foreach (var key in PropertiesObject.Keys)
			{
				properties.Add(key.ToString(), PropertiesObject[key]);
			}
		}

		IFileSystem fileSystem = IFileSystem.Default;
		using Stream stream = fileSystem.FileOpen(path,
			FileMode.Open,
			FileAccess.ReadWrite,
			FileShare.None);
		var doc = XDocument.Load(stream);
		bool hasDocChanged = false;
		doc.Changed += (_, _) => hasDocChanged = true;

		var projectConfigurationService = new VisualStudioProjectConfigurationService(doc);

		var actions = EditDotnetProjectService.ProcessPropertyRequests(this, properties, projectConfigurationService);

		if ((properties.Any() || (InternalsVisibleTo?.Any() ?? false))
			&& ShouldProcess(path, EditDotnetProjectService.DetermineActionDescription(InternalsVisibleTo, properties).ToString()))
		{
			WriteVerbose($"Modifying project at '{path}'");

			foreach ((string propertyName, Action action) in actions)
			{
				WriteVerbose($"Setting property {propertyName}");
				action();
			}

			if (InternalsVisibleTo is not null)
			{
				foreach (var assemblyName in InternalsVisibleTo)
				{
					projectConfigurationService.AddFriendAssemblyName(assemblyName);
				}
			}
		}

		WriteVerbose($"SuppressMessage is {(SuppressMessage is null ? " not" : "")}passed.");
		if (SuppressMessage is not null)
		{
			if (string.IsNullOrWhiteSpace(SuppressMessage.CheckId))
			{
				ThrowTerminatingError(
					new ErrorRecord(
						new ArgumentException(
							$"Parameter {nameof(SuppressMessage)} was not of valid format, missing CheckId.",
							nameof(SuppressMessage)),
						errorId: "Invalid Properties",
						errorCategory: ErrorCategory.InvalidArgument,
						targetObject: null));
			}
			if (string.IsNullOrWhiteSpace(SuppressMessage.Category))
			{
				ThrowTerminatingError(
					new ErrorRecord(
						new ArgumentException(
							$"Parameter {nameof(SuppressMessage)} was not of valid format, missing Category.",
							nameof(SuppressMessage)),
						errorId: "Invalid Properties",
						errorCategory: ErrorCategory.InvalidArgument,
						targetObject: null));
			}
			if (ShouldProcess(path, "Add AssemblyAttribute SuppressMessage."))
			{
				WriteVerbose($"To create AssemblyAttribute with SuppressMessage"
					+ $" Category: {SuppressMessage.Category}"
					+ $" CheckId: {SuppressMessage.CheckId}"
					+ $" Justification: {SuppressMessage.Justification}"
					+ $" Scope: {SuppressMessage.Scope}"
#if UNSPPORTABLE
					+ $" Target: {SuppressMessage.Target}"
#endif
					);
				projectConfigurationService.AddSuppressMessageAttributeAssemblyAttribute(
					category: SuppressMessage.Category,
					checkId: SuppressMessage.CheckId,
					justification: SuppressMessage.Justification
#if UNSPPORTABLE
					, scope: SuppressMessage.Scope
					, SuppressMessage.Target
#endif
					);
			}
		}

		if (!hasDocChanged)
		{
			return;
		}

		// Reset position before saving
		stream.Position = 0;
		doc.Save(stream);
	}
}

#if true
#region candidate for Pri.ProductivityExtensions.Source
/// <summary>
/// Temporary class until available in Pri.ProductivityExtensions.Source
/// </summary>
internal static class KeyValueExtensions
{
	#region KeyValuePair

	/// <param name="kvp"></param>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	extension<TKey, TValue>(KeyValuePair<TKey, TValue> kvp)
	{
		/// <summary>
		/// A Deconstruct method for KeyValuePair
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void Deconstruct(out TKey key,
			out TValue value)
		{
			key = kvp.Key;
			value = kvp.Value;
		}
	}
	#endregion
}
#endregion
#endif