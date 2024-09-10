namespace ImageSizeChecker.Services.Solutions;
internal class OrderByArea : ISolution
{
    // Philosphy:
    // Order the images by largest area,
    // look for all possible x,y coords in the boundry box such that if the top-left pixel is place at this location, the whole image can fit to the right and down
    // For each candidate of ^, we check if all coords of the body of the image is available
    // Place image if possible, otherwise repeat the above
    public bool DoImagesFit(Size boundary, Size[] images)
    {
        var ordered = images.OrderByDescending(img => img.Area).ToArray();
        var grid = new bool[boundary.Width, boundary.Height];

        foreach (var image in ordered)
        {
            var placed = false;

            // Available starting spots that have enough room to fit the full image to the right, starting at the top-left most point of the image
            for (var x = 0; x <= boundary.Width - image.Width; x++)
            {
                for (var y = 0; y <= boundary.Height - image.Height; y++)
                {
                    if (CanPlaceImage(image, grid, x, y))
                    {
                        PlaceImage(image, grid, x, y);
                        placed = true;
                        break;
                    }
                }
                if (placed)
                {
                    break;
                }
            }

            if (!placed)
            {
                // Image doesnt fit
                return false;
            }
        }

        return true;
    }

    private bool CanPlaceImage(Size image, bool[,] grid, int x, int y)
    {
        for (var i = x; i < x + image.Width; i++)
        {
            for (var j = y; j < y + image.Height; j++)
            {
                // Check every grid square and make sure its available for the body of the image
                if (grid[i, j])
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void PlaceImage(Size img, bool[,] grid, int x, int y)
    {
        for (var i = x; i < x + img.Width; i++)
        {
            for (var j = y; j < y + img.Height; j++)
            {
                grid[i, j] = true;
            }
        }
    }
}
