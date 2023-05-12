using NuGetRepackager.CommandLineArguments;
using NuGetRepackager.CommandLineArguments.Enums;
using NuGetRepackager.Packagers;

namespace NuGetRepackager;

/// <summary>
/// Handles creation of implementations of <see cref="IPackager"/>.
/// </summary>
internal sealed class PackagerGenerator
{
    /// <summary>
    /// Creates an instance of the appropriate implementation of <see cref="IPackager"/> 
    /// based on the provided <see cref="CommandLineArgument"/>.
    /// </summary>
    /// <param name="commandLineArgument">The command line argument.</param>
    /// <returns>An instance of <see cref="IPackager"/> for the provided <see cref="CommandLineArgument"/>.</returns>
    internal IPackager CreatePackager(CommandLineArgument commandLineArgument)
    {
        return commandLineArgument.Key switch
        {
            CommandLineArgumentKey.PreReleasePackageFilePath => new PreReleasePackageFilePackager(),
            CommandLineArgumentKey.PreReleaseCsProjFilePath => new PreReleaseCsProjFilePackager(),
            CommandLineArgumentKey.PreReleaseNuspecFilePath => new PreReleaseNuspecFilePackager(),
            _ => throw new InvalidOperationException("There is no packager for the provided command line argument.")
        };
    }
}
