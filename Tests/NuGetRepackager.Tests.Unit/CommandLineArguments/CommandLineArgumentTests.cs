using NuGetRepackager.CommandLineArguments;
using NuGetRepackager.CommandLineArguments.Enums;

namespace NuGetRepackager.Tests.Unit.CommandLineArguments;

public class CommandLineArgumentTests
{
    [Theory]
    [MemberData(nameof(GetKeyArgumentMemberData))]
    public void Constructor_ShouldReturnExpectedCommandLineArgument_WhenArgumentIsOnlyTheKey(
        string argument,
        object expectedCommandLineArgumentKey
    )
    {
        var commandLineArgument = new CommandLineArgument(argument);

        Assert.Equal(expectedCommandLineArgumentKey, commandLineArgument.Key);
        Assert.Null(commandLineArgument.Value);
    }

    [Theory]
    [MemberData(nameof(GetKeyWithValueArgumentMemberData))]
    public void Constructor_ShouldReturnExpectedCommandLineArgument_WhenArgumentIsTheKeyWithValue(
        string argument,
        object expectedCommandLineArgumentKey,
        string expectedCommandLineArgumentValue
    )
    {
        var commandLineArgument = new CommandLineArgument(argument);

        Assert.Equal(expectedCommandLineArgumentKey, commandLineArgument.Key);
        Assert.Equal(expectedCommandLineArgumentValue, commandLineArgument.Value);
    }

    [Theory]
    [InlineData("")]
    public void Constructor_ShouldThrowArgumentException_WhenArgumentIsNotSupported(string unsupportedArgument)
    {
        var commandLineArgumentAction = () => new CommandLineArgument(unsupportedArgument);

        Assert.Throws<ArgumentException>(commandLineArgumentAction);
    }

    public static IEnumerable<object[]> GetKeyArgumentMemberData()
    {
        yield return new object[]
        {
            CommandLineArgumentConstants.PreReleaseVersion,
            CommandLineArgumentKey.PreReleaseVersion
        };

        yield return new object[]
        {
            CommandLineArgumentConstants.PreReleasePackageFilePath,
            CommandLineArgumentKey.PreReleasePackageFilePath
        };

        yield return new object[]
        {
            CommandLineArgumentConstants.PreReleaseCsProjFilePath,
            CommandLineArgumentKey.PreReleaseCsProjFilePath
        };

        yield return new object[]
        {
            CommandLineArgumentConstants.PreReleaseNuspecFilePath,
            CommandLineArgumentKey.PreReleaseNuspecFilePath
        };

        yield return new object[]
        {
            "--wow",
            CommandLineArgumentKey.Unknown
        };
    }

    public static IEnumerable<object[]> GetKeyWithValueArgumentMemberData()
    {
        yield return new object[]
        {
            $"{CommandLineArgumentConstants.PreReleaseVersion}=yep",
            CommandLineArgumentKey.PreReleaseVersion,
            "yep"
        };

        yield return new object[]
        {
            $"{CommandLineArgumentConstants.PreReleasePackageFilePath}=yay",
            CommandLineArgumentKey.PreReleasePackageFilePath,
            "yay"
        };

        yield return new object[]
        {
            $"{CommandLineArgumentConstants.PreReleaseCsProjFilePath}=okay",
            CommandLineArgumentKey.PreReleaseCsProjFilePath,
            "okay"
        };

        yield return new object[]
        {
            $"{CommandLineArgumentConstants.PreReleaseNuspecFilePath}=wow",
            CommandLineArgumentKey.PreReleaseNuspecFilePath,
            "wow"
        };

        yield return new object[]
        {
            $" {CommandLineArgumentConstants.PreReleaseNuspecFilePath} = spaces ",
            CommandLineArgumentKey.PreReleaseNuspecFilePath,
            "spaces"
        };

        yield return new object[]
        {
            $" {CommandLineArgumentConstants.ForcedPackageVersion}=2.0.0",
            CommandLineArgumentKey.ForcedPackageVersion,
            "2.0.0"
        };

        yield return new object[]
        {
            $" {CommandLineArgumentConstants.ForcedPackageVersionPackageFilePath} = C:wow\\Package.nupkg ",
            CommandLineArgumentKey.ForcedPackageVersionPackageFilePath,
            "C:wow\\Package.nupkg"
        };

        yield return new object[]
        {
            $" unknown = space ",
            CommandLineArgumentKey.Unknown,
            "space"
        };
    }
}
