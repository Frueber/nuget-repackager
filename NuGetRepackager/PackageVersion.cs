namespace NuGetRepackager;

/// <summary>
/// Represents a package version with each part of a standard three-part semantic versioning package version ("Major.Minor.Patch") 
/// and an optional pre-release version portion.
/// </summary>
/// <param name="Major">The major version part.</param>
/// <param name="Minor">The minor version part.</param>
/// <param name="Patch">The patch version part.</param>
/// <param name="PreReleasePackageVersion">The pre-release version portion.</param>
internal sealed record PackageVersion(
    int Major,
    int Minor,
    int Patch,
    PackageVersion? PreReleasePackageVersion
);
