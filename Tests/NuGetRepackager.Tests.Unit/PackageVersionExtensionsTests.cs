namespace NuGetRepackager.Tests.Unit;

public class PackageVersionExtensionsTests
{
    [Theory]
    [InlineData("0.0.0-pr.1.1.1", "1.0.0")]
    [InlineData("0.0.0-pr.0.1.1", "0.1.0")]
    [InlineData("0.0.0-pr.0.0.1", "0.0.1")]

    [InlineData("0.0.1-pr.1.1.1", "1.0.0")]
    [InlineData("0.0.1-pr.0.1.1", "0.1.0")]
    [InlineData("0.0.1-pr.0.0.1", "0.0.2")]

    [InlineData("0.1.0-pr.1.1.1", "1.0.0")]
    [InlineData("0.1.0-pr.0.1.1", "0.2.0")]
    [InlineData("0.1.0-pr.0.0.1", "0.1.1")]

    [InlineData("1.0.0-pr.1.1.1", "2.0.0")]
    [InlineData("1.0.0-pr.0.1.1", "1.1.0")]
    [InlineData("1.0.0-pr.0.0.1", "1.0.1")]

    [InlineData("1.0.1-pr.1.1.1", "2.0.0")]
    [InlineData("1.0.1-pr.0.1.1", "1.1.0")]
    [InlineData("1.0.1-pr.0.0.1", "1.0.2")]

    [InlineData("1.1.0-pr.1.1.1", "2.0.0")]
    [InlineData("1.1.0-pr.0.1.1", "1.2.0")]
    [InlineData("1.1.0-pr.0.0.1", "1.1.1")]

    [InlineData("1.1.1-pr.1.1.1", "2.0.0")]
    [InlineData("1.1.1-pr.0.1.1", "1.2.0")]
    [InlineData("1.1.1-pr.0.0.1", "1.1.2")]

    [InlineData("0.0.0-pr.2.2.2", "1.0.0")]
    [InlineData("0.0.0-pr.0.2.2", "0.1.0")]
    [InlineData("0.0.0-pr.0.0.2", "0.0.1")]

    [InlineData("0.0.1-pr.2.2.2", "1.0.0")]
    [InlineData("0.0.1-pr.0.2.2", "0.1.0")]
    [InlineData("0.0.1-pr.0.0.2", "0.0.2")]

    [InlineData("0.1.0-pr.2.2.2", "1.0.0")]
    [InlineData("0.1.0-pr.0.2.2", "0.2.0")]
    [InlineData("0.1.0-pr.0.0.2", "0.1.1")]

    [InlineData("1.0.0-pr.2.2.2", "2.0.0")]
    [InlineData("1.0.0-pr.0.2.2", "1.1.0")]
    [InlineData("1.0.0-pr.0.0.2", "1.0.1")]

    [InlineData("1.0.1-pr.2.2.2", "2.0.0")]
    [InlineData("1.0.1-pr.0.2.2", "1.1.0")]
    [InlineData("1.0.1-pr.0.0.2", "1.0.2")]

    [InlineData("1.1.0-pr.2.2.2", "2.0.0")]
    [InlineData("1.1.0-pr.0.2.2", "1.2.0")]
    [InlineData("1.1.0-pr.0.0.2", "1.1.1")]

    [InlineData("1.1.1-pr.2.2.2", "2.0.0")]
    [InlineData("1.1.1-pr.0.2.2", "1.2.0")]
    [InlineData("1.1.1-pr.0.0.2", "1.1.2")]
    public void GenerateReleasePackageVersion_ShouldReturnExpectedReleasePackageVersion_WhenIsUnmanagedStandardReleaseVersionIsTrue(
        string preReleasePackageVersion,
        string expectedReleasePackageVersion
    )
    {
        var releasePackageVersion = preReleasePackageVersion.ToPackageVersion()
            !.GenerateReleasePackageVersion(true);

        Assert.Equal(expectedReleasePackageVersion, releasePackageVersion.ToPackageVersionString());
    }

    [Theory]
    [InlineData("1.1.1-pr.0.0.2", "1.1.1")]
    public void GenerateReleasePackageVersion_ShouldReturnExpectedReleasePackageVersion_WhenIsUnmanagedStandardReleaseVersionIsFalse(
        string preReleasePackageVersion,
        string expectedReleasePackageVersion
    )
    {
        var releasePackageVersion = preReleasePackageVersion.ToPackageVersion()
            !.GenerateReleasePackageVersion(false);

        Assert.Equal(expectedReleasePackageVersion, releasePackageVersion.ToPackageVersionString());
    }

    [Theory]
    [InlineData("0.0.0-pr.0.0.1", "0.0.0-pr.0.0.2", "0.0.1-pr.0.0.1")]
    [InlineData("0.0.0-pr.0.0.1", "0.0.0-pr.0.0.3", "0.0.1-pr.0.0.2")]
    [InlineData("0.0.0-pr.0.0.1", "0.0.0-pr.0.1.0", "0.0.1-pr.0.1.0")]
    [InlineData("0.0.0-pr.0.0.1", "0.0.0-pr.0.1.1", "0.0.1-pr.0.1.1")]
    [InlineData("0.0.0-pr.0.0.1", "0.0.0-pr.0.2.0", "0.0.1-pr.0.2.0")]
    [InlineData("0.0.0-pr.0.0.1", "0.0.0-pr.0.2.1", "0.0.1-pr.0.2.1")]
    [InlineData("0.0.0-pr.0.0.1", "0.0.0-pr.0.2.2", "0.0.1-pr.0.2.2")]
    [InlineData("0.0.0-pr.0.0.1", "0.0.0-pr.0.3.0", "0.0.1-pr.0.3.0")]
    [InlineData("0.0.0-pr.0.0.1", "0.0.0-pr.0.3.1", "0.0.1-pr.0.3.1")]
    [InlineData("0.0.0-pr.0.0.1", "0.0.0-pr.0.3.2", "0.0.1-pr.0.3.2")]

    [InlineData("0.0.0-pr.0.1.0", "0.0.0-pr.0.1.1", "0.1.0-pr.0.0.1")]
    [InlineData("0.0.0-pr.0.1.0", "0.0.0-pr.0.2.0", "0.1.0-pr.0.1.0")]
    [InlineData("0.0.0-pr.0.1.0", "0.0.0-pr.0.2.1", "0.1.0-pr.0.1.1")]
    [InlineData("0.0.0-pr.0.1.0", "0.0.0-pr.0.2.2", "0.1.0-pr.0.1.2")]
    [InlineData("0.0.0-pr.0.1.0", "0.0.0-pr.0.3.0", "0.1.0-pr.0.2.0")]
    [InlineData("0.0.0-pr.0.1.0", "0.0.0-pr.0.3.1", "0.1.0-pr.0.2.1")]
    [InlineData("0.0.0-pr.0.1.0", "0.0.0-pr.0.3.2", "0.1.0-pr.0.2.2")]

    [InlineData("0.0.0-pr.1.1.1", "0.0.0-pr.1.2.0", "1.0.0-pr.0.1.0")]
    [InlineData("0.0.0-pr.1.1.1", "0.0.0-pr.1.2.1", "1.0.0-pr.0.1.1")]
    [InlineData("0.0.0-pr.1.1.1", "0.0.0-pr.1.2.2", "1.0.0-pr.0.1.2")]
    [InlineData("0.0.0-pr.1.1.1", "0.0.0-pr.1.3.0", "1.0.0-pr.0.2.0")]
    [InlineData("0.0.0-pr.1.1.1", "0.0.0-pr.1.3.1", "1.0.0-pr.0.2.1")]
    [InlineData("0.0.0-pr.1.1.1", "0.0.0-pr.1.3.2", "1.0.0-pr.0.2.2")]
    [InlineData("0.0.0-pr.1.1.1", "0.0.0-pr.2.0.0", "1.0.0-pr.1.0.0")]
    [InlineData("0.0.0-pr.1.1.1", "0.0.0-pr.2.0.1", "1.0.0-pr.1.0.1")]
    [InlineData("0.0.0-pr.1.1.1", "0.0.0-pr.2.1.0", "1.0.0-pr.1.1.0")]
    [InlineData("0.0.0-pr.1.1.1", "0.0.0-pr.2.1.1", "1.0.0-pr.1.1.1")]
    [InlineData("0.0.0-pr.1.1.1", "0.0.0-pr.2.2.0", "1.0.0-pr.1.2.0")]
    [InlineData("0.0.0-pr.1.1.1", "0.0.0-pr.2.2.1", "1.0.0-pr.1.2.1")]
    [InlineData("0.0.0-pr.1.1.1", "0.0.0-pr.2.2.2", "1.0.0-pr.1.2.2")]
    [InlineData("0.0.0-pr.1.1.1", "0.0.0-pr.3.2.2", "1.0.0-pr.2.2.2")]
    public void GenerateUpdatedPreReleasePackageVersion_ShouldReturnExpectedUpdatedPreReleasePackageVersion_WhenIsUnmanagedStandardReleaseVersionIsTrue(
        string preReleasePackageVersion,
        string currentPreReleasePackageVersion,
        string expectedUpdatedPreReleasePackageVersion
    )
    {
        var updatedPreReleasePackageVersion = preReleasePackageVersion.ToPackageVersion()
            !.GenerateUpdatedPreReleasePackageVersion(
                currentPreReleasePackageVersion.ToPackageVersion()!,
                true
            );

        Assert.Equal(expectedUpdatedPreReleasePackageVersion, updatedPreReleasePackageVersion.ToPackageVersionString());
    }

    [Theory]
    [InlineData("1.0.0-pr.1.1.1", "1.0.0-pr.3.2.2", "1.0.0-pr.2.2.2")]
    public void GenerateUpdatedPreReleasePackageVersion_ShouldReturnExpectedUpdatedPreReleasePackageVersion_WhenIsUnmanagedStandardReleaseVersionIsFalse(
        string preReleasePackageVersion,
        string currentPreReleasePackageVersion,
        string expectedUpdatedPreReleasePackageVersion
    )
    {
        var updatedPreReleasePackageVersion = preReleasePackageVersion.ToPackageVersion()
            !.GenerateUpdatedPreReleasePackageVersion(
                currentPreReleasePackageVersion.ToPackageVersion()!,
                false
            );

        Assert.Equal(expectedUpdatedPreReleasePackageVersion, updatedPreReleasePackageVersion.ToPackageVersionString());
    }

    [Theory]
    [InlineData("0.0.0-pr.1.1.1", true, "0.0.0-pr.3.2.2 Did an update.", "1.0.0-pr.2.2.2 [Updated version from 0.0.0-pr.3.2.2] Did an update.")]
    [InlineData("1.0.0-pr.1.1.1", false, "1.0.0-pr.3.2.2 Did an update.", "1.0.0-pr.2.2.2 [Updated version from 1.0.0-pr.3.2.2] Did an update.")]
    public void GenerateUpdatedPreReleasePackageVersionLineForUnmanagedStandardReleaseVersionPart_ShouldReturnExpectedUpdatedPreReleasePackageVersionLine(
        string preReleasePackageVersion,
        bool isUnmanagedStandardReleaseVersion,
        string currentPreReleasePackageVersionLine,
        string expectedUpdatedPreReleasePackageVersionLine
    )
    {
        var updatedPreReleasePackageVersion = preReleasePackageVersion.ToPackageVersion()
            !.GenerateUpdatedPreReleasePackageVersionLine(
                currentPreReleasePackageVersionLine,
                isUnmanagedStandardReleaseVersion
            );

        Assert.Equal(expectedUpdatedPreReleasePackageVersionLine, updatedPreReleasePackageVersion);
    }

    // TODO: Test ComparedTo
    // TODO: Test ToPackageVersion
    // TODO: Test ToPackageVersionString
}
