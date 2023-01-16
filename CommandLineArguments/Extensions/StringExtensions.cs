using NuGetRepackager.CommandLineArguments.Enums;

namespace NuGetRepackager.CommandLineArguments.Extensions;

internal static class StringExtensions
{
    internal static CommandLineArgumentKey ToCommandLineArgumentKey(this string key)
    {
        return key switch
        {
            CommandLineArgumentConstants.PreReleaseVersion => CommandLineArgumentKey.PreReleaseVersion,
            CommandLineArgumentConstants.PreReleasePackageFilePath => CommandLineArgumentKey.PreReleasePackageFilePath,
            CommandLineArgumentConstants.PreReleaseCsProjFilePath => CommandLineArgumentKey.PreReleaseCsProjFilePath,
            CommandLineArgumentConstants.PreReleaseNuspecFilePath => CommandLineArgumentKey.PreReleaseNuspecFilePath,
            _ => CommandLineArgumentKey.Unknown
        };
    }
}
