using System.Text.RegularExpressions;

namespace NuGetRepackager;

/// <summary>
/// Contains useful methods for generating or manipulating package versions.
/// </summary>
internal static class PackageVersionExtensions
{
    /// <summary>
    /// Compares a <see cref="PackageVersion"/> instance to another and returns a <see cref="PackageVersionComparisonResult"/>.
    /// </summary>
    /// <param name="packageVersionA">The package version that the comparison is relative to.</param>
    /// <param name="packageVersionB">The package version to compare against.</param>
    /// <returns><see cref="PackageVersionComparisonResult"/> relative to the <see cref="PackageVersion"/> using this extension method (<paramref name="packageVersionA"/>).</returns>
    internal static PackageVersionComparisonResult ComparedTo(
        this PackageVersion packageVersionA,
        PackageVersion packageVersionB
    )
    {
        if (packageVersionA == packageVersionB)
        {
            return PackageVersionComparisonResult.EqualTo;
        }
        else if (packageVersionA.Major < packageVersionB.Major)
        {
            return PackageVersionComparisonResult.LessThan;
        }
        else if (packageVersionA.Major > packageVersionB.Major)
        {
            return PackageVersionComparisonResult.GreaterThan;
        }
        else if (packageVersionA.Minor < packageVersionB.Minor)
        {
            return PackageVersionComparisonResult.LessThan;
        }
        else if (packageVersionA.Minor > packageVersionB.Minor)
        {
            return PackageVersionComparisonResult.GreaterThan;
        }
        else if (packageVersionA.Patch < packageVersionB.Patch)
        {
            return PackageVersionComparisonResult.LessThan;
        }

        return PackageVersionComparisonResult.GreaterThan;
    }

    /// <summary>
    /// Translates a given release or pre-release version string into a <see cref="PackageVersion"/>.  
    /// </summary>
    /// <param name="version">A version string.</param>
    /// <returns><see cref="PackageVersion"/></returns>
    internal static PackageVersion? ToPackageVersion(this string version)
    {
        var versionParts = version.Split(VersionConstants.PreReleasePackageVersionDivider, 2);

        var releaseVersionParts = versionParts[0].Split(VersionConstants.PackageVersionDivider, VersionConstants.PackageVersionPartsCount);
        
        if (releaseVersionParts.Length < 3)
        {
            return null;
        }

        var preReleaseVersionParts = versionParts.Length > 1
            ? versionParts[1].Split(VersionConstants.PackageVersionDivider, VersionConstants.PackageVersionPartsCount)
            : null;

        return new PackageVersion(
            int.Parse(releaseVersionParts[VersionConstants.PackageVersionMajorPartIndex]),
            int.Parse(releaseVersionParts[VersionConstants.PackageVersionMinorPartIndex]),
            int.Parse(releaseVersionParts[VersionConstants.PackageVersionPatchPartIndex]),
            preReleaseVersionParts is not null
                ?new PackageVersion(
                    int.Parse(preReleaseVersionParts[VersionConstants.PackageVersionMajorPartIndex]),
                    int.Parse(preReleaseVersionParts[VersionConstants.PackageVersionMinorPartIndex]),
                    int.Parse(preReleaseVersionParts[VersionConstants.PackageVersionPatchPartIndex]),
                    null
                )
                : null
        );
    }

    /// <summary>
    /// Translates a given <see cref="PackageVersion"/> into a package version string.
    /// </summary>
    /// <param name="packageVersion">The package version.</param>
    /// <returns>A package version string.</returns>
    internal static string ToPackageVersionString(this PackageVersion packageVersion)
    {
        var releasePackageVersionParts = new[]
        {
            packageVersion.Major,
            packageVersion.Minor,
            packageVersion.Patch
        };
        var preReleasePackageVersionParts = packageVersion.PreReleasePackageVersion is not null
            ? new[]
            {
                packageVersion.PreReleasePackageVersion.Major,
                packageVersion.PreReleasePackageVersion.Minor,
                packageVersion.PreReleasePackageVersion.Patch
            }
            : null;

        var packageVersionString = string.Join(VersionConstants.PackageVersionDivider, releasePackageVersionParts);

        if (preReleasePackageVersionParts is not null)
        {
            packageVersionString += VersionConstants.PreReleasePackageVersionDivider
                + string.Join(VersionConstants.PackageVersionDivider, preReleasePackageVersionParts);
        }

        return packageVersionString;
    }

    /// <summary>
    /// Generates a new release package version from the provided pre-release package version.
    /// </summary>
    /// <param name="preReleasePackageVersion">The pre-release package version.</param>
    /// <param name="isUnmanagedStandardReleaseVersion">Whther or not the standard release version part was not managed (changed during pre-release development).</param>
    /// <returns>A new release package version value in the three-part package version format (Example: <c>0.0.0</c>).</returns>
    internal static PackageVersion GenerateReleasePackageVersion(
        this PackageVersion preReleasePackageVersion,
        bool isUnmanagedStandardReleaseVersion
    )
    {
        var releasePackageVersion = new PackageVersion(
            preReleasePackageVersion.Major,
            preReleasePackageVersion.Minor,
            preReleasePackageVersion.Patch,
            null
        );

        if(isUnmanagedStandardReleaseVersion)
        {
            if (preReleasePackageVersion.PreReleasePackageVersion?.Major > 0)
            {
                releasePackageVersion = releasePackageVersion with
                {
                    Major = releasePackageVersion.Major + 1,
                    Minor = VersionConstants.BasePackageVersionPartNumericValue,
                    Patch = VersionConstants.BasePackageVersionPartNumericValue
                };
            }
            else if (preReleasePackageVersion.PreReleasePackageVersion?.Minor > 0)
            {
                releasePackageVersion = releasePackageVersion with
                {
                    Minor = releasePackageVersion.Minor + 1,
                    Patch = VersionConstants.BasePackageVersionPartNumericValue
                };
            }
            else if (preReleasePackageVersion.PreReleasePackageVersion?.Patch > 0)
            {
                releasePackageVersion = releasePackageVersion with
                {
                    Patch = releasePackageVersion.Patch + 1
                };
            }
        }

        return releasePackageVersion;
    }

    /// <summary>
    /// Generates a new pre-release package version from the provided pre-release package version.
    /// </summary>
    /// <param name="preReleasePackageVersion">The pre-release package version. Example: <c>"0.0.0-pr.0.1.0."</c>.</param>
    /// <param name="currentPreReleasePackageVersion">The current pre-release package version. Example: <c>"0.0.0-pr.1.1.0."</c>.</param>
    /// <param name="isUnmanagedStandardReleaseVersion">Whether or not the standard release version part was not managed (changed during pre-release development).</param>
    /// <returns>A new pre-release package version value in the pre-release package version format. Example: <c>"0.1.0-pr.0.0.0."</c>.</returns>
    internal static PackageVersion GenerateUpdatedPreReleasePackageVersion(
        this PackageVersion preReleasePackageVersion,
        PackageVersion currentPreReleasePackageVersion,
        bool isUnmanagedStandardReleaseVersion
    )
    {
        var releasePackageVersion = GenerateReleasePackageVersion(
            preReleasePackageVersion,
            isUnmanagedStandardReleaseVersion
        );

        if (
            preReleasePackageVersion?.PreReleasePackageVersion is null
            || currentPreReleasePackageVersion?.PreReleasePackageVersion is null
        )
        {
            // TODO: Could update this handling / message.
            throw new ArgumentException("The pre-release versions are not valid.");
        }

        var updatedPreReleasePackageVersion = new PackageVersion(
            currentPreReleasePackageVersion.PreReleasePackageVersion.Major - preReleasePackageVersion.PreReleasePackageVersion.Major,
            0,
            0,
            null
        );

        if (updatedPreReleasePackageVersion.Major > 0)
        {
            updatedPreReleasePackageVersion = updatedPreReleasePackageVersion with
            {
                Minor = currentPreReleasePackageVersion.PreReleasePackageVersion.Minor
            };
        }
        else if (currentPreReleasePackageVersion.PreReleasePackageVersion.Minor >= preReleasePackageVersion.PreReleasePackageVersion.Minor)
        {
            updatedPreReleasePackageVersion = updatedPreReleasePackageVersion with
            {
                Minor = currentPreReleasePackageVersion.PreReleasePackageVersion.Minor - preReleasePackageVersion.PreReleasePackageVersion.Minor
            };
        }

        if (
            updatedPreReleasePackageVersion.Major > 0
            || updatedPreReleasePackageVersion.Minor > 0
        )
        {
            updatedPreReleasePackageVersion = updatedPreReleasePackageVersion with
            {
                Patch = currentPreReleasePackageVersion.PreReleasePackageVersion.Patch
            };
        }
        else if (currentPreReleasePackageVersion.PreReleasePackageVersion.Patch >= preReleasePackageVersion.PreReleasePackageVersion.Patch)
        {
            updatedPreReleasePackageVersion = updatedPreReleasePackageVersion with
            {
                Patch = currentPreReleasePackageVersion.PreReleasePackageVersion.Patch - preReleasePackageVersion.PreReleasePackageVersion.Patch
            };
        }

        return releasePackageVersion with
        {
            PreReleasePackageVersion = updatedPreReleasePackageVersion
        };
    }

    /// <summary>
    /// Generates a new pre-release package version release notes line from the provided pre-release package version release notes line.
    /// </summary>
    /// <param name="preReleasePackageVersion">The pre-release package version. Example: <c>"0.0.0-pr.0.1.0."</c>.</param>
    /// <param name="preReleasePackageVersionLine">The pre-release package version release notes line. Example: <c>"0.0.0-pr.0.1.0 This is an example release notes line."</c>.</param>
    /// <param name="isUnmanagedStandardReleaseVersion">Whether or not the standard release version part was not managed (changed during pre-release development).</param>
    /// <returns>A new pre-release package version release notes line value in the pre-release package version format with the prior release notes of the line. Example: <c>"0.1.0-pr.0.0.0 [Updated version from 0.0.0-pr.0.1.0] This is an example release notes line."</c>.</returns>
    internal static string GenerateUpdatedPreReleasePackageVersionLine(
        this PackageVersion preReleasePackageVersion,
        string preReleasePackageVersionLine,
        bool isUnmanagedStandardReleaseVersion
    )
    {
        var preReleasePackageVersionLinePattern = $@"^(.*?){VersionConstants.RegexPatterns.PreReleasePackageVersionGroupPattern}(.*)";
        var currentPreReleasePackageVersionLineGroups = Regex.Match(preReleasePackageVersionLine, preReleasePackageVersionLinePattern).Groups;
        var currentLineIndentation = currentPreReleasePackageVersionLineGroups[1].Value;
        var currentPreReleasePackageVersion = $"{currentPreReleasePackageVersionLineGroups[2].Value}{currentPreReleasePackageVersionLineGroups[3].Value}".ToPackageVersion()!;
        var currentReleaseNotes = currentPreReleasePackageVersionLineGroups[4].Value;

        var updatedPreReleasePackageVersion = GenerateUpdatedPreReleasePackageVersion(
            preReleasePackageVersion,
            currentPreReleasePackageVersion,
            isUnmanagedStandardReleaseVersion
        );

        return $"{currentLineIndentation}{updatedPreReleasePackageVersion.ToPackageVersionString()} [Updated version from {currentPreReleasePackageVersion.ToPackageVersionString()}]{currentReleaseNotes}";
    }
}
