using System.Xml;
using System.Xml.Serialization;
using NuGetRepackager.CommandLineArguments;

namespace NuGetRepackager.Packagers;

/// <summary>
/// Provides features for updating files involved in version updates and packaging for nuspec files where we're forcing a version update.
/// </summary>
internal sealed class ForcedPackageVersionNuspecFilePackager : IPackager
{
    private const string _versionXmlTag = "version";

    /// <inheritdoc/>
    public void Handle(
        CommandLineArgument commandLineArgument,
        PackageVersion preReleasePackageVersion,
        CommandLineArgument[] commandLineArguments
    )
    {
        var forcedPackageVersion = commandLineArguments.SingleOrDefault(commandLineArgument => commandLineArgument.Key is CommandLineArguments.Enums.CommandLineArgumentKey.ForcedPackageVersion)
            ?.Value
            ?.ToPackageVersion();

        if (commandLineArgument.Value is null)
        {
            Console.WriteLine("Please provide the pre-release package file path.");

            throw new ArgumentException("The required command line argument value was not provided.");
        }
        else if (forcedPackageVersion is null)
        {
            Console.WriteLine("Please provide the forced package version.");

            throw new ArgumentException("The required command line argument value was not provided.");
        }

        var nuspecFilePath = commandLineArgument.Value;

        var nuspecXmlDocument = new XmlDocument
        {
            PreserveWhitespace = true
        };

        try
        {
            nuspecXmlDocument.Load(nuspecFilePath);
        }
        catch
        {
            Console.WriteLine("Unable to load nuspec file.");

            return;
        }

        var versionXmlNodeList = nuspecXmlDocument.GetElementsByTagName(_versionXmlTag);

        var currentPackageVersion = versionXmlNodeList[0]?.InnerText?.ToPackageVersion();

        if (currentPackageVersion is null)
        {
            Console.WriteLine("The package version could not be found.");

            return;
        }

        Console.WriteLine();
        Console.WriteLine($"Targeted Package Version: {currentPackageVersion.ToPackageVersionString()}");
        Console.WriteLine($"Updated Package Version: {forcedPackageVersion.ToPackageVersionString()}");
        Console.WriteLine();

        Console.WriteLine("Updating...");

        var newNuspecXmlDocument = (XmlDocument)nuspecXmlDocument.Clone();

        var newVersionXmlNodeList = newNuspecXmlDocument.GetElementsByTagName(_versionXmlTag);

        newVersionXmlNodeList[0]!.InnerText = forcedPackageVersion.ToPackageVersionString();

        var xmlSerializer = new XmlSerializer(typeof(XmlDocument));

        var fileStream = File.Create(nuspecFilePath);

        xmlSerializer.Serialize(fileStream, newNuspecXmlDocument);

        fileStream.Close();

        Console.WriteLine("Updated nuspec file.");
        Console.WriteLine();
    }
}
