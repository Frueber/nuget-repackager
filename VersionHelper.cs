﻿using System.Text.RegularExpressions;

namespace NuGetRepackager;

/// <summary>
/// Contains useful methods for generating or manipulating version patterns.
/// </summary>
internal static class VersionHelper
{
    /// <summary>
    /// Generates a new release package version from the provided pre-release package version.
    /// </summary>
    /// <param name="preReleasePackageVersion">The pre-release package version.</param>
    /// <returns>A new release package version value in the three-part package version format (Example: <c>0.0.0</c>).</returns>
    internal static string GenerateReleasePackageVersion(string preReleasePackageVersion)
    {
        var versionParts = preReleasePackageVersion.Split(VersionConstants.PreReleasePackageVersionDivider, 2);

        var releaseVersionParts = versionParts[0].Split(VersionConstants.PackageVersionDivider, VersionConstants.PackageVersionPartsCount);
        var preReleaseVersionParts = versionParts[1].Split(VersionConstants.PackageVersionDivider, VersionConstants.PackageVersionPartsCount);

        if (int.Parse(preReleaseVersionParts[0]) > 0)
        {
            releaseVersionParts[0] = (int.Parse(releaseVersionParts[0]) + 1).ToString();
        }
        else if (int.Parse(preReleaseVersionParts[1]) > 0)
        {
            releaseVersionParts[1] = (int.Parse(releaseVersionParts[1]) + 1).ToString();
        }
        else if (int.Parse(preReleaseVersionParts[2]) > 0)
        {
            releaseVersionParts[2] = (int.Parse(releaseVersionParts[2]) + 1).ToString();
        }

        return string.Join(VersionConstants.PackageVersionDivider, releaseVersionParts);
    }

    /// <summary>
    /// Generates a new pre-release package version from the provided pre-release package version.
    /// </summary>
    /// <param name="preReleasePackageVersion">The pre-release package version. Example: <c>"0.0.0-pr.0.1.0."</c>.</param>
    /// <param name="currentPreReleasePackageVersion">The current pre-release package version. Example: <c>"0.0.0-pr.1.1.0."</c>.</param>
    /// <returns>A new pre-release package version value in the pre-release package version format. Example: <c>"0.1.0-pr.0.0.0."</c>.</returns>
    internal static string GenerateUpdatedPreReleasePackageVersion(
        string preReleasePackageVersion,
        string currentPreReleasePackageVersion
    )
    {
        var releasePackageVersion = GenerateReleasePackageVersion(preReleasePackageVersion);

        var versionPartsA = preReleasePackageVersion.Split(VersionConstants.PreReleasePackageVersionDivider, 2);

        var versionPartsB = currentPreReleasePackageVersion.Split(VersionConstants.PreReleasePackageVersionDivider, 2);

        var preReleaseVersionPartsA = versionPartsA[1].Split(VersionConstants.PackageVersionDivider, VersionConstants.PackageVersionPartsCount);
        var preReleaseVersionPartsB = versionPartsB[1].Split(VersionConstants.PackageVersionDivider, VersionConstants.PackageVersionPartsCount);

        var updatedPreReleasePackageVersionParts = new string[VersionConstants.PackageVersionPartsCount];

        for (var versionPartIndex = 0; versionPartIndex < VersionConstants.PackageVersionPartsCount; versionPartIndex++)
        {
            if (int.Parse(preReleaseVersionPartsA[versionPartIndex]) < int.Parse(preReleaseVersionPartsB[versionPartIndex]))
            {
                updatedPreReleasePackageVersionParts[versionPartIndex] = (int.Parse(preReleaseVersionPartsB[versionPartIndex]) - int.Parse(preReleaseVersionPartsA[versionPartIndex])).ToString();
            }
            else
            {
                updatedPreReleasePackageVersionParts[versionPartIndex] = VersionConstants.BasePackageVersionPartValue;
            }
        }

        var updatedPreReleasePackageVersion = string.Join(VersionConstants.PackageVersionDivider, updatedPreReleasePackageVersionParts);

        return $"{releasePackageVersion}{VersionConstants.PreReleasePackageVersionDivider}{updatedPreReleasePackageVersion}";
    }

    /// <summary>
    /// Generates a new pre-release package version release notes line from the provided pre-release package version release notes line.
    /// </summary>
    /// <param name="preReleasePackageVersion">The pre-release package version. Example: <c>"0.0.0-pr.0.1.0."</c>.</param>
    /// <param name="preReleasePackageVersionLine">The pre-release package version release notes line. Example: <c>"0.0.0-pr.0.1.0 This is an example release notes line."</c>.</param>
    /// <returns>A new pre-release package version release notes line value in the pre-release package version format with the prior release notes of the line. Example: <c>"0.1.0-pr.0.0.0 [Updated version from 0.0.0-pr.0.1.0] This is an example release notes line."</c>.</returns>
    internal static string GenerateUpdatedPreReleasePackageVersionLine(
        string preReleasePackageVersion,
        string preReleasePackageVersionLine
    )
    {
        var preReleasePackageVersionLinePattern = $@"^(.*?){VersionConstants.RegexPatterns.PreReleasePackageVersionGroupPattern}(.*)";
        var releasePackageVersion = GenerateReleasePackageVersion(preReleasePackageVersion);

        var versionPartsA = preReleasePackageVersion.Split(VersionConstants.PreReleasePackageVersionDivider, 2);
        var currentPreReleasePackageVersionLineGroups = Regex.Match(preReleasePackageVersionLine, preReleasePackageVersionLinePattern).Groups;
        var currentLineIndentation = currentPreReleasePackageVersionLineGroups[1].Value;
        var currentPreReleasePackageVersion = $"{currentPreReleasePackageVersionLineGroups[2].Value}{currentPreReleasePackageVersionLineGroups[3].Value}";
        var currentReleaseNotes = currentPreReleasePackageVersionLineGroups[4].Value;
        var versionPartsB = currentPreReleasePackageVersion.Split(VersionConstants.PreReleasePackageVersionDivider, 2);

        var preReleaseVersionPartsA = versionPartsA[1].Split(VersionConstants.PackageVersionDivider, VersionConstants.PackageVersionPartsCount);
        var preReleaseVersionPartsB = versionPartsB[1].Split(VersionConstants.PackageVersionDivider, VersionConstants.PackageVersionPartsCount);
        var updatedPreReleasePackageVersionParts = new string[VersionConstants.PackageVersionPartsCount];

        for (var versionPartIndex = 0; versionPartIndex < VersionConstants.PackageVersionPartsCount; versionPartIndex++)
        {
            if (int.Parse(preReleaseVersionPartsA[versionPartIndex]) < int.Parse(preReleaseVersionPartsB[versionPartIndex]))
            {
                updatedPreReleasePackageVersionParts[versionPartIndex] = (int.Parse(preReleaseVersionPartsB[versionPartIndex]) - int.Parse(preReleaseVersionPartsA[versionPartIndex])).ToString();
            }
            else
            {
                updatedPreReleasePackageVersionParts[versionPartIndex] = VersionConstants.BasePackageVersionPartValue;
            }
        }

        var updatedPreReleasePackageVersion = string.Join(VersionConstants.PackageVersionDivider, updatedPreReleasePackageVersionParts);

        return $"{currentLineIndentation}{releasePackageVersion}{VersionConstants.PreReleasePackageVersionDivider}{updatedPreReleasePackageVersion} [Updated version from {currentPreReleasePackageVersion}]{currentReleaseNotes}";
    }
}