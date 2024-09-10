namespace ImageSizeChecker.Services.Solutions;
internal interface ISolution
{
    public bool DoImagesFit(Size boundry, Size[] images);
}
