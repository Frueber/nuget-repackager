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
    /*
    #region ComparedTo
    [Theory]
    [MemberData(nameof(ComparedToMemberData_StandardReleasePackageVersions_LessThan))]
    [MemberData(nameof(ComparedToMemberData_StandardReleasePackageVersions_EqualTo))]
    [MemberData(nameof(ComparedToMemberData_StandardReleasePackageVersions_GreaterThan))]
    [MemberData(nameof(ComparedToMemberData_PreReleasePackageVersions_LessThan))]
    [MemberData(nameof(ComparedToMemberData_PreReleasePackageVersions_EqualTo))]
    [MemberData(nameof(ComparedToMemberData_PreReleasePackageVersions_GreaterThan))]
    public void ComparedTo_ShouldReturnExpectedPackageVersionComparisonResult(
        PackageVersion packageVersionA,
        PackageVersion packageVersionB,
        PackageVersionComparisonResult expectedPackageVersionComparisonResult
    )
    {
        var packageVersionComparisonResult = packageVersionA.ComparedTo(packageVersionB);

        Assert.Equal(expectedPackageVersionComparisonResult, packageVersionComparisonResult);
    }

    public static IEnumerable<object[]> ComparedToMemberData_StandardReleasePackageVersions_LessThan()
    {
        yield return new object[]
        {
            new PackageVersion(
                1,
                0,
                0,
                null
            ),
            new PackageVersion(
                2,
                0,
                0,
                null
            ),
            PackageVersionComparisonResult.LessThan
        };

        yield return new object[]
        {
            new PackageVersion(
                1,
                0,
                1,
                null
            ),
            new PackageVersion(
                1,
                1,
                0,
                null
            ),
            PackageVersionComparisonResult.LessThan
        };

        yield return new object[]
        {
            new PackageVersion(
                1,
                0,
                1,
                null
            ),
            new PackageVersion(
                1,
                0,
                2,
                null
            ),
            PackageVersionComparisonResult.LessThan
        };
    }

    public static IEnumerable<object[]> ComparedToMemberData_StandardReleasePackageVersions_EqualTo()
    {
        yield return new object[]
        {
            new PackageVersion(
                1,
                0,
                0,
                null
            ),
            new PackageVersion(
                1,
                0,
                0,
                null
            ),
            PackageVersionComparisonResult.EqualTo
        };

        yield return new object[]
        {
            new PackageVersion(
                1,
                1,
                1,
                null
            ),
            new PackageVersion(
                1,
                1,
                1,
                null
            ),
            PackageVersionComparisonResult.EqualTo
        };

        yield return new object[]
        {
            new PackageVersion(
                4,
                5,
                6,
                null
            ),
            new PackageVersion(
                4,
                5,
                6,
                null
            ),
            PackageVersionComparisonResult.EqualTo
        };
    }

    public static IEnumerable<object[]> ComparedToMemberData_StandardReleasePackageVersions_GreaterThan()
    {
        yield return new object[]
        {
            new PackageVersion(
                2,
                0,
                0,
                null
            ),
            new PackageVersion(
                1,
                0,
                0,
                null
            ),
            PackageVersionComparisonResult.LessThan
        };

        yield return new object[]
        {
            new PackageVersion(
                1,
                1,
                0,
                null
            ),
            new PackageVersion(
                1,
                0,
                1,
                null
            ),
            PackageVersionComparisonResult.LessThan
        };

        yield return new object[]
        {
            new PackageVersion(
                1,
                0,
                2,
                null
            ),
            new PackageVersion(
                1,
                0,
                1,
                null
            ),
            PackageVersionComparisonResult.LessThan
        };
    }

    public static IEnumerable<object[]> ComparedToMemberData_PreReleasePackageVersions_LessThan()
    {
        yield return new object[]
        {
            new PackageVersion(
                1,
                0,
                0,
                new PackageVersion(
                    1,
                    0,
                    0,
                    null
                )
            ),
            new PackageVersion(
                2,
                0,
                0,
                new PackageVersion(
                    2,
                    0,
                    0,
                    null
                )
            ),
            PackageVersionComparisonResult.LessThan
        };

        yield return new object[]
        {
            new PackageVersion(
                1,
                0,
                1,
                new PackageVersion(
                    1,
                    0,
                    1,
                    null
                )
            ),
            new PackageVersion(
                1,
                1,
                0,
                new PackageVersion(
                    1,
                    1,
                    0,
                    null
                )
            ),
            PackageVersionComparisonResult.LessThan
        };

        yield return new object[]
        {
            new PackageVersion(
                1,
                0,
                1,
                new PackageVersion(
                    1,
                    0,
                    1,
                    null
                )
            ),
            new PackageVersion(
                1,
                0,
                2,
                new PackageVersion(
                    1,
                    0,
                    2,
                    null
                )
            ),
            PackageVersionComparisonResult.LessThan
        };
    }

    public static IEnumerable<object[]> ComparedToMemberData_PreReleasePackageVersions_EqualTo()
    {
        yield return new object[]
        {
            new PackageVersion(
                1,
                0,
                0,
                new PackageVersion(
                    1,
                    0,
                    0,
                    null
                )
            ),
            new PackageVersion(
                1,
                0,
                0,
                new PackageVersion(
                    1,
                    0,
                    0,
                    null
                )
            ),
            PackageVersionComparisonResult.EqualTo
        };

        yield return new object[]
        {
            new PackageVersion(
                1,
                1,
                1,
                new PackageVersion(
                    1,
                    1,
                    1,
                    null
                )
            ),
            new PackageVersion(
                1,
                1,
                1,
                new PackageVersion(
                    1,
                    1,
                    1,
                    null
                )
            ),
            PackageVersionComparisonResult.EqualTo
        };

        yield return new object[]
        {
            new PackageVersion(
                4,
                5,
                6,
                new PackageVersion(
                    4,
                    5,
                    6,
                    null
                )
            ),
            new PackageVersion(
                4,
                5,
                6,
                new PackageVersion(
                    4,
                    5,
                    6,
                    null
                )
            ),
            PackageVersionComparisonResult.EqualTo
        };
    }

    public static IEnumerable<object[]> ComparedToMemberData_PreReleasePackageVersions_GreaterThan()
    {
        yield return new object[]
        {
            new PackageVersion(
                2,
                0,
                0,
                new PackageVersion(
                    2,
                    0,
                    0,
                    null
                )
            ),
            new PackageVersion(
                1,
                0,
                0,
                new PackageVersion(
                    1,
                    0,
                    0,
                    null
                )
            ),
            PackageVersionComparisonResult.LessThan
        };

        yield return new object[]
        {
            new PackageVersion(
                1,
                1,
                0,
                new PackageVersion(
                    1,
                    1,
                    0,
                    null
                )
            ),
            new PackageVersion(
                1,
                0,
                1,
                new PackageVersion(
                    1,
                    0,
                    1,
                    null
                )
            ),
            PackageVersionComparisonResult.LessThan
        };

        yield return new object[]
        {
            new PackageVersion(
                1,
                0,
                2,
                new PackageVersion(
                    1,
                    0,
                    2,
                    null
                )
            ),
            new PackageVersion(
                1,
                0,
                1,
                new PackageVersion(
                    1,
                    0,
                    1,
                    null
                )
            ),
            PackageVersionComparisonResult.LessThan
        };
    }
    #endregion
    */

    [Fact]
    public void ToPackageVersion_ShouldReturnExpectedPackageVersion_WhenStringContainsPreReleasePackageVersion()
    {
        var packageVersionAsString = "1.2.3-pr.4.5.6";

        var packageVersion = packageVersionAsString.ToPackageVersion();

        Assert.Equal(
            new PackageVersion(
                1,
                2,
                3,
                new PackageVersion(
                    4,
                    5,
                    6,
                    null
                )
            ),
            packageVersion
        );
    }

    [Fact]
    public void ToPackageVersion_ShouldReturnExpectedPackageVersion_WhenStringDoesNotContainPreReleasePackageVersion()
    {
        var packageVersionAsString = "1.2.3";

        var packageVersion = packageVersionAsString.ToPackageVersion();

        Assert.Equal(
            new PackageVersion(
                1,
                2,
                3,
                null
            ),
            packageVersion
        );
    }

    [Fact]
    public void ToPackageVersionString_ShouldReturnExpectedPackageVersionString_WhenPackageVersionContainsPreReleasePackageVersion()
    {
        var packageVersion = new PackageVersion(
            1,
            2,
            3,
            new PackageVersion(
                4,
                5,
                6,
                null
            )
        );

        var packageVersionString = packageVersion.ToPackageVersionString();

        Assert.Equal(
            "1.2.3-pr.4.5.6",
            packageVersionString
        );
    }

    [Fact]
    public void ToPackageVersion_ShouldReturnExpectedPackageVersion_WhenPackageVersionDoesNotContainPreReleasePackageVersion()
    {
        var packageVersion = new PackageVersion(
            1,
            2,
            3,
            null
        );

        var packageVersionString = packageVersion.ToPackageVersionString();

        Assert.Equal(
            "1.2.3",
            packageVersionString
        );
    }
}
