using NuGetRepackager.CommandLineArguments.Extensions;
using NuGetRepackager.CommandLineArguments;

namespace NuGetRepackager.Tests.Unit.CommandLineArguments.Extensions;

public sealed class CommandLineArugmentExtensionsTests
{
    [Fact]
    public void ToPackagerCommandLineArguments_ShouldReturnExpectedPackagerCommandLineArguments_WhenInvoked()
    {
        var commandLineArguments = new[]
        {
            new CommandLineArgument("--unknown-flag")
        };
        var expectedPackagerCommandLineArguments = Array.Empty<CommandLineArgument>();

        var packagerCommandLineArguments = commandLineArguments.ToPackagerCommandLineArguments();

        Assert.Equal(expectedPackagerCommandLineArguments, packagerCommandLineArguments);
    }

    [Fact]
    public void ToPackagerCommandLineArguments_ShouldReturnExpectedPackagerCommandLineArguments_WhenInvoked2()
    {
        var commandLineArguments = new[]
        {
            new CommandLineArgument("--prv=1.1.1-pr.1.1.1")
        };
        var expectedPackagerCommandLineArguments = Array.Empty<CommandLineArgument>();

        var packagerCommandLineArguments = commandLineArguments.ToPackagerCommandLineArguments();

        Assert.Equal(expectedPackagerCommandLineArguments, packagerCommandLineArguments);
    }

    [Fact]
    public void ToPackagerCommandLineArguments_ShouldReturnExpectedPackagerCommandLineArguments_WhenInvoked3()
    {
        var packagerCommandLineArgument = new CommandLineArgument("--nupkg=C:wow\\Package.nupkg");
        var commandLineArguments = new[]
        {
            packagerCommandLineArgument
        };
        var expectedPackagerCommandLineArguments = new[]
        {
            packagerCommandLineArgument
        };

        var packagerCommandLineArguments = commandLineArguments.ToPackagerCommandLineArguments();

        Assert.Equal(expectedPackagerCommandLineArguments, packagerCommandLineArguments);
    }
}
