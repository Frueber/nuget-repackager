using System.IO.Compression;
using System.Text.RegularExpressions;
using NuGetRepackager.CommandLineArguments;

namespace NuGetRepackager.Packagers;

/// <summary>
/// Provides features for updating files involved in version updates and packaging for NuGet packages where we're forcing a version update.
/// </summary>
internal sealed class ForcedPackageVersionPackageFilePackager : IPackager
{
    /// <inheritdoc/>
    public void Handle(
        CommandLineArgument commandLineArgument,
        PackageVersion preReleasePackageVersion,
        CommandLineArgument[] commandLineArguments
    )
    {
        var forcedPackageVersion = commandLineArguments.SingleOrDefault(commandLineArgument => commandLineArgument.Key is CommandLineArguments.Enums.CommandLineArgumentKey.ForcedPackageVersion)
            ?.Value
            ?.ToPackageVersion();

        if (commandLineArgument.Value is null)
        {
            Console.WriteLine("Please provide the package file path which should be forced to a new version.");

            throw new ArgumentException("The required command line argument value was not provided.");
        }
        else if (forcedPackageVersion is null)
        {
            Console.WriteLine("Please provide the forced package version.");

            throw new ArgumentException("The required command line argument value was not provided.");
        }

        // TODO: Update the version with no need to update past history.

        // TODO: This should be able to detect if we were given a standard release or pre-release package version.

        var packageFilePathGroups = Regex.Match(commandLineArgument.Value, VersionConstants.RegexPatterns.PreReleasePackageFilePathGroupPattern).Groups;

        var packageFilePath = packageFilePathGroups[0].Value;
        var packageFileDirectory = packageFilePathGroups[1].Value;
        var packageFileName = packageFilePathGroups[2].Value;

        var packageVersionGroups = Regex.Match(packageFileName, $@"(.*[^\.])\.=?{VersionConstants.RegexPatterns.PreReleasePackageVersionGroupPattern}$").Groups;

        if (packageVersionGroups.Count < 3)
        {
            packageVersionGroups = Regex.Match(packageFileName, $@"(.*[^\.])\.=?{VersionConstants.RegexPatterns.ReleasePackageVersionGroupPattern}$").Groups;
        }

        var packageFileNameWithoutPackageVersion = packageVersionGroups[1].Value;

        var packageVersionFromFileName = Regex.Match(packageFileName, $"{VersionConstants.RegexPatterns.PreReleasePackageVersionGroupPattern}$")
            .Value
            .ToPackageVersion()
            ?? Regex.Match(packageFileName, $"{VersionConstants.RegexPatterns.ReleasePackageVersionGroupPattern}$")
                .Value
                .ToPackageVersion();

        if (packageVersionFromFileName is null)
        {
            Console.WriteLine("The current package version of the targeted package could not be found in the package filename.");

            throw new ArgumentException("The required command line argument value was not provided.");
        }

        var forcedPackageVersionComparisonResult = forcedPackageVersion.ComparedTo(packageVersionFromFileName);

        if (forcedPackageVersionComparisonResult is not PackageVersionComparisonResult.GreaterThan)
        {
            Console.WriteLine("The provided forced package version must be newer than the current package version.");

            return;
        }

        var packageFileNameWithForcedPackageVersion = packageFileName.Replace(
            packageVersionFromFileName.ToPackageVersionString(),
            forcedPackageVersion.ToPackageVersionString()
        );

        var nuspecFileName = $"{packageFileNameWithoutPackageVersion}{VersionConstants.NuspecFileExtension}";
        // The release package file path is also the NuGet package without the ".nupkg" extension.
        var releasePackageFilePath = $"{packageFileDirectory}{packageFileNameWithForcedPackageVersion}";
        var releasePackageFilePathWithExtension = $"{releasePackageFilePath}{VersionConstants.NuGetPackageFileExtension}";
        var nuspecFilePath = $"{releasePackageFilePath}\\{nuspecFileName}";

        Console.WriteLine();
        Console.WriteLine($"Targeted Package Version: {packageVersionFromFileName.ToPackageVersionString()}");
        Console.WriteLine($"Updated Package Version: {forcedPackageVersion.ToPackageVersionString()}");
        Console.WriteLine();

        if (File.Exists(releasePackageFilePathWithExtension))
        {
            Console.WriteLine($"There is already a package with the intended release version {forcedPackageVersion.ToPackageVersionString()}. Will not attempt to repackage.");

            return;
        }

        ZipFile.ExtractToDirectory(
            packageFilePath,
            releasePackageFilePath,
            true
        );

        Console.WriteLine("Repackaging...");

        var forcedPackageVersionNuspecFilePackagerCommandLineArgument = new CommandLineArgument($"{CommandLineArgumentConstants.PreReleaseNuspecFilePath}={nuspecFilePath}");
        
        var forcedPackageVersionNuspecFilePackager = new ForcedPackageVersionNuspecFilePackager();

        forcedPackageVersionNuspecFilePackager.Handle(
            forcedPackageVersionNuspecFilePackagerCommandLineArgument,
            preReleasePackageVersion,
            commandLineArguments
        );

        ZipFile.CreateFromDirectory(
            releasePackageFilePath,
            releasePackageFilePathWithExtension
        );

        Directory.Delete(releasePackageFilePath, true);

        Console.WriteLine("Repackaged.");
        Console.WriteLine();
    }
}
