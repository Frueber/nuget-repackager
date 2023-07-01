using System.IO.Compression;
using System.Text.RegularExpressions;
using NuGetRepackager.CommandLineArguments;
using NuGetRepackager.CommandLineArguments.Enums;

namespace NuGetRepackager.Packagers;

/// <summary>
/// Provides features for updating files involved in version updates and packaging for pre-release NuGet packages.
/// </summary>
internal sealed class PreReleasePackageFilePackager : IPackager
{
    /// <inheritdoc/>
    public void Handle(
        CommandLineArgument commandLineArgument,
        PackageVersion preReleasePackageVersion,
        CommandLineArgument[] commandLineArguments
    )
    {
        if (commandLineArgument.Value is null)
        {
            Console.WriteLine("Please provide the pre-release package file path.");

            throw new ArgumentException("The required command line argument value was not provided.");
        }

        var isUnmanagedStandardReleaseVersion = commandLineArguments.Any(commandLineArgument => commandLineArgument.Key is CommandLineArgumentKey.UnmanagedStandardReleaseVersion);

        var preReleasePackageFilePathGroups = Regex.Match(commandLineArgument.Value, VersionConstants.RegexPatterns.PreReleasePackageFilePathGroupPattern).Groups;

        var preReleasePackageFilePath = preReleasePackageFilePathGroups[0].Value;
        var preReleasePackageFileDirectory = preReleasePackageFilePathGroups[1].Value;
        var preReleasePackageFileName = preReleasePackageFilePathGroups[2].Value;

        var preReleasePackageVersionGroups = Regex.Match(preReleasePackageFileName, $@"(.*[^\.])\.=?{VersionConstants.RegexPatterns.PreReleasePackageVersionGroupPattern}$").Groups;

        var preReleasePackageVersionFromFileName = (
            preReleasePackageVersionGroups[2].Value
            + preReleasePackageVersionGroups[3].Value
        )
            .ToPackageVersion();

        if (preReleasePackageVersionFromFileName?.PreReleasePackageVersion is null)
        {
            Console.WriteLine("The provided package is not a pre-release. Nothing to repackage.");

            return;
        }
        else if (preReleasePackageVersion != preReleasePackageVersionFromFileName)
        {
            Console.WriteLine("The provided pre-release package version does not match the provided pre-release version.");

            return;
        }

        var packageFileName = preReleasePackageVersionGroups[1].Value;

        var releasePackageVersion = preReleasePackageVersion.GenerateReleasePackageVersion(isUnmanagedStandardReleaseVersion);

        var nuspecFileName = $"{packageFileName}{VersionConstants.NuspecFileExtension}";
        // The release package file path is also the NuGet package without the ".nupkg" extension.
        var releasePackageFilePath = $"{preReleasePackageFileDirectory}{packageFileName}.{releasePackageVersion.ToPackageVersionString()}";
        var releasePackageFilePathWithExtension = $"{releasePackageFilePath}{VersionConstants.NuGetPackageFileExtension}";
        var nuspecFilePath = $"{releasePackageFilePath}\\{nuspecFileName}";

        Console.WriteLine();
        Console.WriteLine($"Targeted Package Version: {preReleasePackageVersion.ToPackageVersionString()}");
        Console.WriteLine($"Updated Package Version: {releasePackageVersion.ToPackageVersionString()}");
        Console.WriteLine();

        if (File.Exists(releasePackageFilePathWithExtension))
        {
            Console.WriteLine($"There is already a package with the intended release version {releasePackageVersion.ToPackageVersionString()}. Will not attempt to repackage.");

            return;
        }

        ZipFile.ExtractToDirectory(
            preReleasePackageFilePath,
            releasePackageFilePath,
            true
        );

        Console.WriteLine("Repackaging...");

        var preReleaseNuspecFilePackagerCommandLineArgument = new CommandLineArgument($"{CommandLineArgumentConstants.PreReleaseNuspecFilePath}={nuspecFilePath}");
        
        var preReleaseNuspecFilePackager = new PreReleaseNuspecFilePackager();

        preReleaseNuspecFilePackager.Handle(
            preReleaseNuspecFilePackagerCommandLineArgument,
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
