namespace ImageSizeChecker.Services;
public class Size
{
    public Size(int width, int height)
    {
        Width = width;
        Height = height;
        Area = width * height;
    }
    public int Width { get; set; }
    public int Height { get; set; }
    public int Area { get; }
}
