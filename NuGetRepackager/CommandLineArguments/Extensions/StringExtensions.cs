using NuGetRepackager.CommandLineArguments.Enums;

namespace NuGetRepackager.CommandLineArguments.Extensions;

/// <summary>
/// Command line argument extension methods for <see cref="string"/>.
/// </summary>
internal static class StringExtensions
{
    /// <summary>
    /// Get the associated <see cref="CommandLineArgumentKey"/> for the provided command line argument key/flag.
    /// </summary>
    /// <param name="key">The key/flag from the command line.</param>
    /// <returns>The associated <see cref="CommandLineArgumentKey"/>.</returns>
    public static CommandLineArgumentKey ToCommandLineArgumentKey(this string key)
    {
        return key switch
        {
            CommandLineArgumentConstants.PreReleaseVersion => CommandLineArgumentKey.PreReleaseVersion,
            CommandLineArgumentConstants.PreReleasePackageFilePath => CommandLineArgumentKey.PreReleasePackageFilePath,
            CommandLineArgumentConstants.PreReleaseCsProjFilePath => CommandLineArgumentKey.PreReleaseCsProjFilePath,
            CommandLineArgumentConstants.PreReleaseNuspecFilePath => CommandLineArgumentKey.PreReleaseNuspecFilePath,
            CommandLineArgumentConstants.ForcedPackageVersion => CommandLineArgumentKey.ForcedPackageVersion,
            CommandLineArgumentConstants.ForcedPackageVersionPackageFilePath => CommandLineArgumentKey.ForcedPackageVersionPackageFilePath,
            _ => CommandLineArgumentKey.Unknown
        };
    }
}
