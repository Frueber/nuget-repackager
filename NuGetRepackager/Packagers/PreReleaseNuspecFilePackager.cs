﻿using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using NuGetRepackager.CommandLineArguments;
using NuGetRepackager.CommandLineArguments.Enums;

namespace NuGetRepackager.Packagers;

/// <summary>
/// Provides features for updating files involved in version updates and packaging for nuspec files.
/// </summary>
internal sealed class PreReleaseNuspecFilePackager : IPackager
{
    private const string _releaseNotesXmlTag = "releaseNotes";
    private const string _versionXmlTag = "version";
    private const string _newLineCharacters = "\r\n";

    /// <inheritdoc/>
    public void Handle(
        CommandLineArgument commandLineArgument,
        PackageVersion preReleasePackageVersion,
        CommandLineArgument[] commandLineArguments
    )
    {
        if (commandLineArgument.Value is null)
        {
            Console.WriteLine("Please provide the pre-release package file path.");

            throw new ArgumentException("The required command line argument value was not provided.");
        }

        var isUnmanagedStandardReleaseVersion = commandLineArguments.Any(commandLineArgument => commandLineArgument.Key is CommandLineArgumentKey.UnmanagedStandardReleaseVersion);

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
        else if (preReleasePackageVersion != currentPackageVersion)
        {
            Console.WriteLine("The provided nuspec file version does not match the provided pre-release version.");

            return;
        }

        var releasePackageVersion = currentPackageVersion.GenerateReleasePackageVersion(isUnmanagedStandardReleaseVersion);

        Console.WriteLine();
        Console.WriteLine($"Targeted Package Version: {currentPackageVersion.ToPackageVersionString()}");
        Console.WriteLine($"Updated Package Version: {releasePackageVersion.ToPackageVersionString()}");
        Console.WriteLine();

        Console.WriteLine("Updating...");

        var newNuspecXmlDocument = (XmlDocument)nuspecXmlDocument.Clone();

        var newVersionXmlNodeList = newNuspecXmlDocument.GetElementsByTagName(_versionXmlTag);

        newVersionXmlNodeList[0]!.InnerText = releasePackageVersion.ToPackageVersionString();

        Console.WriteLine();
        Console.WriteLine($"Current Package Version In Nuspec file: {currentPackageVersion.ToPackageVersionString()}");
        Console.WriteLine();

        var releaseNotesXmlNodeList = nuspecXmlDocument.GetElementsByTagName(_releaseNotesXmlTag);

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

                if (trimmedReleaseNotesLine.StartsWith(preReleasePackageVersion.ToPackageVersionString()))
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
                    releaseNotesLines[releaseNotesLineIndex] = preReleasePackageVersion.GenerateUpdatedPreReleasePackageVersionLine(
                        releaseNotesLine,
                        isUnmanagedStandardReleaseVersion
                    );
                }

                Console.WriteLine(releaseNotesLine);
            }

            if (isPreReleaseVersionFound)
            {
                var releaseNotesLine = $"{releaseLineIndentation}{releasePackageVersion.ToPackageVersionString()} Version change for release of {preReleasePackageVersion.ToPackageVersionString()}.";

                if (releaseNotesLineIndexForInsert is not null)
                {
                    releaseNotesLines.Insert(
                        (int)releaseNotesLineIndexForInsert,
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

            var newReleaseNotesXmlNodeList = newNuspecXmlDocument.GetElementsByTagName(_releaseNotesXmlTag);

            newReleaseNotesXmlNodeList[0]!.InnerText = string.Join(_newLineCharacters, releaseNotesLines);
        }
        else
        {
            Console.WriteLine("Release notes could not be found. Continuing...");
        }

        var xmlSerializer = new XmlSerializer(typeof(XmlDocument));

        var fileStream = File.Create(nuspecFilePath);

        xmlSerializer.Serialize(fileStream, newNuspecXmlDocument);

        fileStream.Close();

        Console.WriteLine("Updated nuspec file.");
        Console.WriteLine();
    }
}
