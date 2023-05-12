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
        string preReleaseVersion
    )
    {
        if (commandLineArgument.Value is null)
        {
            Console.WriteLine("Please provide the package file path which should be forced to a new version.");

            throw new ArgumentException("The required command line argument value was not provided.");
        }

        // TODO: Verify that we have the new version.
        // TODO: Forcing the version to an older or same version is not allowed.
        // TODO: Update the version with no need to update past history.

        // TODO: This should be able to detect if we were given a standard release or pre-release package version.

        var preReleasePackageFilePathGroups = Regex.Match(commandLineArgument.Value, VersionConstants.RegexPatterns.PreReleasePackageFilePathGroupPattern).Groups;

        var preReleasePackageFilePath = preReleasePackageFilePathGroups[0].Value;
        var preReleasePackageFileDirectory = preReleasePackageFilePathGroups[1].Value;
        var preReleasePackageFileName = preReleasePackageFilePathGroups[2].Value;

        var preReleasePackageVersion = Regex.Match(preReleasePackageFileName, $"{VersionConstants.RegexPatterns.PreReleasePackageVersionGroupPattern}$").Value;

        if (preReleasePackageVersion is null)
        {
            Console.WriteLine("The provided package is not a pre-release. Nothing to repackage.");

            return;
        }
        else if (preReleaseVersion != preReleasePackageVersion)
        {
            Console.WriteLine("The provided pre-release package version does not match the provided pre-release version.");

            return;
        }

        var packageFileName = Regex.Match(preReleasePackageFileName, $@"(.*[^\.])\.=?{VersionConstants.RegexPatterns.PreReleasePackageVersionGroupPattern}$").Groups[1];

        var releasePackageVersion = VersionHelper.GenerateReleasePackageVersion(preReleasePackageVersion);

        var nuspecFileName = $"{packageFileName}{VersionConstants.NuspecFileExtension}";
        // The release package file path is also the NuGet package without the ".nuget" extension.
        var releasePackageFilePath = $"{preReleasePackageFileDirectory}{packageFileName}.{releasePackageVersion}";
        var releasePackageFilePathWithExtension = $"{releasePackageFilePath}{VersionConstants.NuGetPackageFileExtension}";
        var nuspecFilePath = $"{releasePackageFilePath}\\{nuspecFileName}";

        Console.WriteLine();
        Console.WriteLine($"Targeted Package Version: {preReleasePackageVersion}");
        Console.WriteLine($"Updated Package Version: {releasePackageVersion}");
        Console.WriteLine();

        if (File.Exists(releasePackageFilePathWithExtension))
        {
            Console.WriteLine($"There is already a package with the intended release version {releasePackageVersion}. Will not attempt to repackage.");

            return;
        }

        ZipFile.ExtractToDirectory(
            preReleasePackageFilePath,
            releasePackageFilePath,
            true
        );

        Console.WriteLine("Repackaging...");

        var preReleaseNuspecFilePackagerCommandLineArgument = new CommandLineArgument($"{CommandLineArgumentConstants.PreReleaseNuspecFilePath}={nuspecFilePath}");
        
        // TODO: Make a ForcedPackageVersionNuspecFilePackager
        var preReleaseNuspecFilePackager = new PreReleaseNuspecFilePackager();

        preReleaseNuspecFilePackager.Handle(
            preReleaseNuspecFilePackagerCommandLineArgument,
            preReleaseVersion
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
