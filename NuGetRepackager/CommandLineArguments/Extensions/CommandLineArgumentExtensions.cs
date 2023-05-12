using NuGetRepackager.CommandLineArguments.Enums;
using NuGetRepackager.Packagers;

namespace NuGetRepackager.CommandLineArguments.Extensions;

internal static class CommandLineArgumentExtensions
{
    /// <summary>
    /// Reduces the collection of <see cref="CommandLineArgument"/> to a collection of ones that have an associated packager, if any.
    /// </summary>
    /// <returns>A collection of <see cref="CommandLineArgument"/> where there is an associated packager (implementation of <see cref="IPackager"/>).</returns>
    internal static IEnumerable<CommandLineArgument> ToPackagerCommandLineArguments(this IEnumerable<CommandLineArgument> commandLineArguments)
    {
        return commandLineArguments.Where(commandLineArgument =>
            commandLineArgument.Key is CommandLineArgumentKey.PreReleasePackageFilePath
            or CommandLineArgumentKey.PreReleaseCsProjFilePath
            or CommandLineArgumentKey.PreReleaseNuspecFilePath
            or CommandLineArgumentKey.ForcedPackageVersionPackageFilePath
        );
    }
}
