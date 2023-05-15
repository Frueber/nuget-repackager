using NuGetRepackager.CommandLineArguments;

namespace NuGetRepackager.Packagers;

/// <summary>
/// Provides features for updating files involved in version updates and packaging.
/// </summary>
internal interface IPackager
{
    /// <summary>
    /// Performs tasks involved in version updating and saves the associated files.
    /// </summary>
    /// <param name="commandLineArgument">The command line argument.</param>
    /// <param name="preReleasePackageVersion">The pre-release package version.</param>
    /// <param name="commandLineArguments">A collection of command line arguments.</param> 
    void Handle(
        CommandLineArgument commandLineArgument,
        PackageVersion preReleasePackageVersion,
        CommandLineArgument[] commandLineArguments
    );
}
