namespace NuGetRepackager.CommandLineArguments.Enums;

/// <summary>
/// The possible keys expected in command line arguments.
/// </summary>
internal enum CommandLineArgumentKey
{
    /// <summary>
    /// This key was not expected.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// The pre-release package file path.
    /// </summary>
    PreReleasePackageFilePath = 1,

    /// <summary>
    /// The pre-release csproj file path.
    /// </summary>
    PreReleaseCsProjFilePath = 2,

    /// <summary>
    /// The pre-release version.
    /// </summary>
    PreReleaseVersion = 3,

    /// <summary>
    /// The pre-release nuspec file path.
    /// </summary>
    PreReleaseNuspecFilePath = 4,

    /// <summary>
    /// The version to force a package or associated files to.
    /// </summary>
    ForcedPackageVersion = 5,

    /// <summary>
    /// The package file path being forced to a specified version.
    /// </summary>
    ForcedPackageVersionPackageFilePath = 6
}
