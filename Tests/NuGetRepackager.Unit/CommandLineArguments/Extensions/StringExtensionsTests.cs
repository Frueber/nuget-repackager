﻿using NuGetRepackager.CommandLineArguments;
using NuGetRepackager.CommandLineArguments.Enums;
using NuGetRepackager.CommandLineArguments.Extensions;

namespace NuGetRepackager.Unit.CommandLineArguments.Extensions;

public sealed class StringExtensionsTests
{
    [Theory]
    [InlineData(CommandLineArgumentConstants.PreReleaseVersion, CommandLineArgumentKey.PreReleaseVersion)]
    [InlineData(CommandLineArgumentConstants.PreReleasePackageFilePath, CommandLineArgumentKey.PreReleasePackageFilePath)]
    [InlineData(CommandLineArgumentConstants.PreReleaseCsProjFilePath, CommandLineArgumentKey.PreReleaseCsProjFilePath)]
    [InlineData(CommandLineArgumentConstants.PreReleaseNuspecFilePath, CommandLineArgumentKey.PreReleaseNuspecFilePath)]
    [InlineData("SomeUnknownValue", CommandLineArgumentKey.Unknown)]
    public void ToCommandLineArgumentKey_ShouldReturnExpectedCommandLineArgumentKey_WhenInvoked(
        string commandLineArgumentValue,
        object expectedCommandLineArgumentKey
    )
    {
        var commandLineArgumentKey = commandLineArgumentValue.ToCommandLineArgumentKey();

        Assert.Equal(expectedCommandLineArgumentKey, commandLineArgumentKey);
    }
}