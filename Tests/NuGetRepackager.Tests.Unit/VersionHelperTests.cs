namespace NuGetRepackager.Tests.Unit;

public class VersionHelperTests
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
    public void GenerateReleasePackageVersion_ShouldReturnExpectedReleasePackageVersion(
        string preReleasePackageVersion,
        string expectedReleasePackageVersion
    )
    {
        var releasePackageVersion = VersionHelper.GenerateReleasePackageVersion(preReleasePackageVersion);

        Assert.Equal(expectedReleasePackageVersion, releasePackageVersion);
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
    public void GenerateUpdatedPreReleasePackageVersion_ShouldReturnExpectedUpdatedPreReleasePackageVersion(
        string preReleasePackageVersion,
        string currentPreReleasePackageVersion,
        string expectedUpdatedPreReleasePackageVersion
    )
    {
        var updatedPreReleasePackageVersion = VersionHelper.GenerateUpdatedPreReleasePackageVersion(
            preReleasePackageVersion,
            currentPreReleasePackageVersion
        );

        Assert.Equal(expectedUpdatedPreReleasePackageVersion, updatedPreReleasePackageVersion);
    }

    [Theory]
    [InlineData("0.0.0-pr.1.1.1", "0.0.0-pr.3.2.2 Did an update.", "1.0.0-pr.2.2.2 [Updated version from 0.0.0-pr.3.2.2] Did an update.")]
    public void GenerateUpdatedPreReleasePackageVersionLine_ShouldReturnExpectedUpdatedPreReleasePackageVersionLine(
        string preReleasePackageVersion,
        string currentPreReleasePackageVersionLine,
        string expectedUpdatedPreReleasePackageVersionLine
    )
    {
        var updatedPreReleasePackageVersion = VersionHelper.GenerateUpdatedPreReleasePackageVersionLine(
            preReleasePackageVersion,
            currentPreReleasePackageVersionLine
        );

        Assert.Equal(expectedUpdatedPreReleasePackageVersionLine, updatedPreReleasePackageVersion);
    }
}
