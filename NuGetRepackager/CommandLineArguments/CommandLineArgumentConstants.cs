namespace NuGetRepackager.CommandLineArguments;

internal static class CommandLineArgumentConstants
{
    internal const string CommandLineArgumentDivider = "--";

    internal const string PreReleaseVersion = $"{CommandLineArgumentDivider}prv";

    internal const string PreReleasePackageFilePath = $"{CommandLineArgumentDivider}nupkg";

    internal const string PreReleaseCsProjFilePath = $"{CommandLineArgumentDivider}csproj";

    internal const string PreReleaseNuspecFilePath = $"{CommandLineArgumentDivider}nuspec";
}
