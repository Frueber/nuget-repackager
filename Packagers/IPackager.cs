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
    /// <param name="preReleaseVersion">The pre-release version.</param>
    void Handle(
        CommandLineArgument commandLineArgument,
        string preReleaseVersion
    );
}
