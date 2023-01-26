using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using NuGetRepackager;
using NuGetRepackager.CommandLineArguments;
using NuGetRepackager.CommandLineArguments.Enums;

Console.WriteLine("Taking a look at things...");

var commandLineArguments = args.Select(argument => new CommandLineArgument(argument));

var packagerCommandLineArguments = commandLineArguments.Where(commandLineArgument =>
    commandLineArgument.Key is not CommandLineArgumentKey.Unknown
    and not CommandLineArgumentKey.PreReleaseVersion
)
    .ToArray();

if (
    commandLineArguments is null
    || !commandLineArguments.Any()
)
{
    Console.WriteLine("Please provide arguments.");

    return;
}
else if (
    packagerCommandLineArguments is null
    || !packagerCommandLineArguments.Any()
)
{
    Console.WriteLine("Please provide packager arguments.");

    return;
}

var preReleaseVersion = string.Empty;

try
{
    var preReleaseVersionCommandLineArgument = commandLineArguments.Single(commandLineArgument => commandLineArgument.Key is CommandLineArgumentKey.PreReleaseVersion);

    if (
        preReleaseVersionCommandLineArgument.Value is null
        || !Regex.IsMatch(preReleaseVersionCommandLineArgument.Value, $"^{VersionConstants.RegexPatterns.PreReleasePackageVersionGroupPattern}$")
    )
    {
        Console.WriteLine("Please provide the pre-release version argument.");

        return;
    }

    preReleaseVersion = preReleaseVersionCommandLineArgument.Value;
}
catch
{
    Console.WriteLine("Please provide the pre-release version argument.");

    return;
}

var packagerGenerator = new PackagerGenerator();

foreach (var packagerCommandLineArgument in packagerCommandLineArguments)
{
    var packager = packagerGenerator.CreatePackager(packagerCommandLineArgument);

    packager.Handle(
        packagerCommandLineArgument,
        preReleaseVersion
    );
}
