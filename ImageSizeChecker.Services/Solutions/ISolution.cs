namespace ImageSizeChecker.Services.Solutions;
internal interface ISolution
{
    public bool DoImagesFit(Size boundary, Size[] images);
}
