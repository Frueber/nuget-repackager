namespace NuGetRepackager;

/// <summary>
/// Contains constant values used in version updates.
/// </summary>
internal static class VersionConstants
{
    internal static class RegexPatterns
    {
        internal const string ReleasePackageVersionGroupPattern = @"(\d+\.\d+\.\d+)";

        internal const string PreReleasePackageVersionDividerPattern = @"\-pr\.";

        internal const string PreReleasePackageVersionGroupPattern = $@"{ReleasePackageVersionGroupPattern}({PreReleasePackageVersionDividerPattern}\d+\.\d+\.\d+)";

        internal const string PreReleasePackageFilePathGroupPattern = @"(.*[\\||\/])?(.*)(.nupkg$)";

        internal const string ReleaseLineIndentationPattern = @"(.*?)(?=\d)";
    }

    internal const string PreReleasePackageVersionDivider = "-pr.";

    internal const string PackageVersionDivider = ".";

    internal const string BasePackageVersionPartValue = "0";

    internal const int BasePackageVersionPartNumericValue = 0;

    internal const int PackageVersionPartsCount = 3;

    internal const int PackageVersionMajorPartIndex = 0;

    internal const int PackageVersionMinorPartIndex = 1;

    internal const int PackageVersionPatchPartIndex = 2;

    internal const string NuspecFileExtension = ".nuspec";

    internal const string NuGetPackageFileExtension = ".nupkg";

    internal const string CsProjFileExtension = ".csproj";
}
