using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using NuGetRepackager.CommandLineArguments;

namespace NuGetRepackager.Packagers;

/// <summary>
/// Provides features for version updates of csproj files.
/// </summary>
internal sealed class PreReleaseCsProjFilePackager : IPackager
{
    private const string _packageReleaseNotesXmlTag = "PackageReleaseNotes";
    private const string _versionXmlTag = "Version";
    private const string _newLineCharacters = "\r\n";

    /// <inheritdoc/>
    public void Handle(
        CommandLineArgument commandLineArgument,
        string preReleaseVersion
    )
    {
        if (commandLineArgument.Value is null)
        {
            Console.WriteLine("Please provide the csproj file path.");

            throw new ArgumentException("The required command line argument value was not provided.");
        }

        Console.WriteLine("Updating csproj file...");
        Console.WriteLine();

        var csProjXmlDocument = new XmlDocument
        {
            PreserveWhitespace = true
        };

        try
        {
            csProjXmlDocument.Load(commandLineArgument.Value);
        }
        catch
        {
            Console.WriteLine("Unable to load csproj file.");

            return;
        }

        var versionXmlNodeList = csProjXmlDocument.GetElementsByTagName(_versionXmlTag);

        var currentPackageVersion = versionXmlNodeList[0]?.InnerText;

        if (currentPackageVersion is null)
        {
            Console.WriteLine("The version could not be found.");

            return;
        }

        var updatedPackageVersion = currentPackageVersion == preReleaseVersion
            ? VersionHelper.GenerateReleasePackageVersion(preReleaseVersion)
            : VersionHelper.GenerateUpdatedPreReleasePackageVersion(
                preReleaseVersion,
                currentPackageVersion
            );

        Console.WriteLine($"Targeted Package Version: {preReleaseVersion}");
        Console.WriteLine($"Current Package Version: {currentPackageVersion}");
        Console.WriteLine($"Updated Package Version: {updatedPackageVersion}");
        Console.WriteLine();

        var currentPackageVersionParts = Regex.Match(currentPackageVersion, VersionConstants.RegexPatterns.ReleasePackageVersionGroupPattern)
            .Value
            .Split(VersionConstants.PackageVersionDivider, VersionConstants.PackageVersionPartsCount);

        var updatedPackageVersionParts = Regex.Match(updatedPackageVersion, VersionConstants.RegexPatterns.ReleasePackageVersionGroupPattern)
            .Value
            .Split(VersionConstants.PackageVersionDivider, VersionConstants.PackageVersionPartsCount);

        for (var packageVersionPartIndex = VersionConstants.PackageVersionMajorPartIndex; packageVersionPartIndex < VersionConstants.PackageVersionPatchPartIndex; packageVersionPartIndex++)
        {
            if (int.Parse(currentPackageVersionParts[packageVersionPartIndex]) > int.Parse(updatedPackageVersionParts[packageVersionPartIndex]))
            {
                Console.WriteLine($"The current csproj file managed package version part, {currentPackageVersion}, is past the expected update version {updatedPackageVersion}.");

                return;
            }
            else if (int.Parse(currentPackageVersionParts[packageVersionPartIndex]) < int.Parse(updatedPackageVersionParts[packageVersionPartIndex]))
            {
                // This version part was updated and the following version parts are expected to be reset to 0.
                break;
            }
        }

        var newCsProjXmlDocument = (XmlDocument)csProjXmlDocument.Clone();

        var newVersionXmlNodeList = newCsProjXmlDocument.GetElementsByTagName(_versionXmlTag);

        newVersionXmlNodeList[0]!.InnerText = updatedPackageVersion;

        Console.WriteLine();
        Console.WriteLine($"Current Version In csproj file: {currentPackageVersion}");
        Console.WriteLine();

        var releaseNotesXmlNodeList = csProjXmlDocument.GetElementsByTagName(_packageReleaseNotesXmlTag);

        var releaseNotesVersionNode = releaseNotesXmlNodeList[0];

        var releaseNotesLines = releaseNotesVersionNode?.InnerText.Split(_newLineCharacters)
            .ToList();

        if (releaseNotesLines is not null)
        {
            var isPreReleaseVersionFound = false;
            var releaseNotesLineIndexForInsert = default(int?);
            var releaseLineIndentation = string.Empty;

            Console.WriteLine("Current Release Notes:");

            // Perform updates, print current values.
            for (var releaseNotesLineIndex = 0; releaseNotesLineIndex < releaseNotesLines.Count; releaseNotesLineIndex++)
            {
                var releaseNotesLine = releaseNotesLines.ElementAt(releaseNotesLineIndex);
                var trimmedReleaseNotesLine = releaseNotesLine.TrimStart();

                if (trimmedReleaseNotesLine.StartsWith(preReleaseVersion))
                {
                    isPreReleaseVersionFound = true;
                    releaseLineIndentation = Regex.Match(releaseNotesLine, $@"^{VersionConstants.RegexPatterns.ReleaseLineIndentationPattern}").Value;
                    // TODO: Ideally, we shouldn't have a production release version past this point... but what if we did?
                }
                else if (
                    isPreReleaseVersionFound
                    && Regex.IsMatch(trimmedReleaseNotesLine, $"^{VersionConstants.RegexPatterns.PreReleasePackageVersionGroupPattern}")
                )
                {
                    if (releaseNotesLineIndexForInsert is null)
                    {
                        releaseNotesLineIndexForInsert = releaseNotesLineIndex;
                    }

                    // We aren't expecting to perform a production release of a package version that is older than the latest package version.
                    releaseNotesLines[releaseNotesLineIndex] = VersionHelper.GenerateUpdatedPreReleasePackageVersionLine(
                        preReleaseVersion,
                        releaseNotesLine
                    );
                }

                Console.WriteLine(releaseNotesLine);
            }

            if (isPreReleaseVersionFound)
            {
                var releaseNotesLine = $"{releaseLineIndentation}{Regex.Match(updatedPackageVersion, VersionConstants.RegexPatterns.ReleasePackageVersionGroupPattern).Value} Version change for release of {preReleaseVersion}.";

                if (releaseNotesLineIndexForInsert is not null)
                {
                    releaseNotesLines.Insert(
                        (int)releaseNotesLineIndexForInsert,
                        releaseNotesLine
                    );
                }
                else if (string.IsNullOrWhiteSpace(releaseNotesLines.Last()))
                {
                    // Assuming a new line with indentation. We will insert before it to retain formatting.
                    releaseNotesLines.Insert(
                        releaseNotesLines.Count - 1,
                        releaseNotesLine
                    );
                }
                else
                {
                    releaseNotesLines.Add(releaseNotesLine);
                }
            }

            Console.WriteLine();

            Console.WriteLine("Updated Release Notes:");

            // Printing updates
            foreach (var releaseNotesLine in releaseNotesLines)
            {
                Console.WriteLine(releaseNotesLine);
            }

            Console.WriteLine();

            var newReleaseNotesXmlNodeList = newCsProjXmlDocument.GetElementsByTagName(_packageReleaseNotesXmlTag);

            newReleaseNotesXmlNodeList[0]!.InnerText = string.Join(_newLineCharacters, releaseNotesLines);
        }
        else
        {
            Console.WriteLine("Release notes could not be found. Continuing...");
        }

        var xmlSerializer = new XmlSerializer(typeof(XmlDocument));

        var fileStream = File.Create(commandLineArgument.Value);

        xmlSerializer.Serialize(fileStream, newCsProjXmlDocument);

        fileStream.Close();

        Console.WriteLine("Updated csproj file.");
        Console.WriteLine();
    }
}
