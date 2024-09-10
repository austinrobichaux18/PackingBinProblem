using ImageSizeChecker.Services;

namespace ImageSizeChecker.Tests;

public class MainTests
{
    [Fact]
    public void GivenSample_ShouldFail()
    {
        // Explanation:
        // the 80x50 bounding box is reduced to 80x44 due to the 80x6 entry taking up the top 6 vertical space, completely horizontally(80).
        // The bounding box is now 80x44 size, but we have a 48x50,
        // which means the 50 height exceeds the height limit of 44, so these two entries cause a failure.
        var service = new ImageSizeCheckerService();
        var sizes = new string[] { "80 50", "20 5", "7 18", "7 18", "6 32", "48 50", "3 7", "80 6" };

        Assert.False(service.DoImagesFit(sizes));
    }

    #region Trivial Fails
    [Fact]
    public void EmptyArray_ShouldFail()
    {
        var service = new ImageSizeCheckerService();
        var sizes = new string[] { };

        Assert.False(service.DoImagesFit(sizes));
    }
    [Fact]
    public void NullArray_ShouldFail()
    {
        var service = new ImageSizeCheckerService();
        string[] sizes = null;

        Assert.False(service.DoImagesFit(sizes));
    }
    #endregion

    #region Simple Tests
    [Fact]
    public void Simple_TooSmall_ShouldFail()
    {
        var service = new ImageSizeCheckerService();
        var sizes = new string[] { "1 1", "2 2" };

        Assert.False(service.DoImagesFit(sizes));
    }
    [Fact]
    public void Simple_ShouldPass()
    {
        var service = new ImageSizeCheckerService();
        var sizes = new string[] { "2 2", "1 1" };

        Assert.True(service.DoImagesFit(sizes));
    }

    [Fact]
    public void SimpleOneImage_FullSize_ShouldPass()
    {
        var service = new ImageSizeCheckerService();
        var sizes = new string[] { "1 1", "1 1" };

        Assert.True(service.DoImagesFit(sizes));
    }
    [Fact]
    public void SimpleTwoImage_TakeFullSize_ShouldPass()
    {
        var service = new ImageSizeCheckerService();
        var sizes = new string[] { "10 10", "2 10", "8 10" };

        Assert.True(service.DoImagesFit(sizes));
    }
    [Fact]
    public void SimpleTwoImage_SlightlyLessThanFullSize_ShouldPass()
    {
        var service = new ImageSizeCheckerService();
        var sizes = new string[] { "10 10", "2 9", "7 10" };

        Assert.True(service.DoImagesFit(sizes));
    }
    [Fact]
    public void SimpleTwoImage_SlightlyMoreThanFullSize_ShouldFail()
    {
        var service = new ImageSizeCheckerService();
        var sizes = new string[] { "10 10", "4 10", "7 10" };

        Assert.False(service.DoImagesFit(sizes));
    }
    [Fact]
    public void SimpleTwoImage_WontFitIfTakenInOrder_ShouldFitIfOrderIgnored_ShouldPass()
    {
        //if placed in order, this will fail.
        //it needs to place 9 1 under 1 1 and 1 10 to the right side
        var service = new ImageSizeCheckerService();
        var sizes = new string[] { "10 10", "1 1", "1 10", "9 1" };

        Assert.True(service.DoImagesFit(sizes));
    }
    #endregion

    [Fact]
    public void IfOrderByArea_ThenDoesNotFit_ShouldPass()
    {
        // This is an example that should fit and result in a passing case
        // However, if you order by area size only, this will fail. Need a robust solution that tries other permutations
        // If you place 5x5, to the right of 10x10, this should fit 15x1 under them
        // If you place 5x5 under 10x10, 15x1 wont fit
        // Inverse is also true ^ 
        var service = new ImageSizeCheckerService();

        var sizes = new string[] { "15 15", "10 10", "5 5", "1 15" };
        Assert.True(service.DoImagesFit(sizes));

        var sizes2 = new string[] { "15 15", "10 10", "5 5", "15 1" };
        Assert.True(service.DoImagesFit(sizes2));
    }

    [Theory]
    [InlineData("FailingCases_Format.txt", false)]
    [InlineData("FailingCases.txt", false)]
    [InlineData("PassingCases.txt", true)]
    public void FromFile_BulkTestCases(string fileName, bool shouldPass)
    {
        // Note: Bulk test cases use "_" instead of newline to seperate entries so that we can fit multiple test cases in one file.
        var service = new ImageSizeCheckerService();
        var filePath = FilePathService.GetFilePath(fileName);
        var lines = File.ReadAllLines(filePath);

        foreach (var item in lines)
        {
            var sizes = item.Contains("_") ? item.Split("_")
                                          : (new string[] { item });
            Assert.Equal(shouldPass, service.DoImagesFit(sizes));
        }
    }
}