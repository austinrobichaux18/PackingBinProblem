namespace ImageSizeChecker.Services.Solutions;
internal class SubdivideArea : ISolution
{
    // Philosphy:
    // Order the images by largest area,
    // When you place a rectangle, subdivide the remaining space into two more rectangles
    // You will get a series of "empty space" rectangles that you can try to fill. 
    // This has an issue of not being able to re-merge two subdivided rectangles even though visually you can, but should preform quickly on the cases
    // that do qualify. 
    public bool DoImagesFit(Size boundary, Size[] images)
    {
        return false;
    }
}
