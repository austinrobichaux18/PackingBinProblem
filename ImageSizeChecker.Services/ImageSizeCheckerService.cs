namespace ImageSizeChecker.Services;

public class ImageSizeCheckerService
{
    public bool DoImagesFit(string[] lines)
    {
        if (lines == null || lines.Length < 2)
        {
            return false;
        }

        var sizes = new List<Size>();
        try
        {
            foreach (var line in lines)
            {
                sizes.Add(GetSizeFromString(line));
            }
        }
        catch (FormatException)
        {
            return false;
        }

        var boundry = sizes[0];
        var images = sizes.ToList();
        images.RemoveAt(0);

        if (!IsValid(boundry, images))
        {
            return false;
        }

        return DoImagesFit(boundry, images.ToArray());
    }

    // Philosphy:
    // Order the images by largest area,
    // look for all possible x,y coords in the boundry box such that if the top-left pixel is place at this location, the whole image can fit to the right and down
    // For each candidate of ^, we check if all coords of the body of the image is available
    // Place image if possible, otherwise repeat the above
    private bool DoImagesFit(Size boundary, Size[] images)
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

    #region Validation
    private bool IsValid(Size boundry, List<Size> images)
    {
        if (boundry.Width < 1 || boundry.Height < 1)
        {
            return false;
        }
        if (images.Count < 1)
        {
            //Fail if we have no images
            return false;
        }
        var areaSum = 0;
        foreach (var image in images.ToList())
        {
            // Remove negative or zero size images.
            // Arguably could fail if these are sent in, but we opt for being graceful and just ignoring bad sizes
            // Possible to run through the whole code check with no images if we remove them all, we opt for this being a pass as well
            if (image.Width < 1 || image.Height < 1)
            {
                images.Remove(image);
                continue;
            }
            areaSum += image.Area;
            if (image.Width > boundry.Width || image.Height > boundry.Height)
            {
                // Trivial boundry check
                return false;
            }
        }
        if (areaSum > boundry.Area)
        {
            // Trivial area check
            return false;
        }

        return true;
    }
    private static Size GetSizeFromString(string size)
    {
        if (!size.Contains(' '))
        {
            throw new FormatException();
        }
        var split = size.Split(' ');
        if (!int.TryParse(split[0], out var x) || !int.TryParse(split[1], out var y) || split.Length > 2)
        {
            throw new FormatException();
        }
        return new Size(x, y);
    }
    #endregion
}
