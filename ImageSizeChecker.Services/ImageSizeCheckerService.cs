﻿using ImageSizeChecker.Services.Solutions;

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

    private bool DoImagesFit(Size boundry, Size[] sizes)
    {
        // Philosphy: 
        // We use a collection of approximation algorithms in hopes of achieving a well performing success result. 
        // For each algorithm that fails, we move to the next. 
        // Finally, we use a brute force algorithm as the final case.
        // For sufficiently large and complex N images, the performance for the brute force case will likely be poor.
        // Upgrade Note: If we were to add more algorithms,
        // I would replace this else if set with a reflection call to grab all implementors of the ISolution interface and loop through them.
        // I'd probably also add a priority level on the ISolution interface so that we can run some in priority compared to others. 
        if (new OrderByArea().DoImagesFit(boundry, sizes))
        {
            return true;
        }
        else if (new SubdivideArea(SubdivideFavor.Vertical).DoImagesFit(boundry, sizes))
        {
            return true;
        }
        else if (new SubdivideArea(SubdivideFavor.Horizontal).DoImagesFit(boundry, sizes))
        {
            return true;
        }
        else if (new SubdivideAreaComplex().DoImagesFit(boundry, sizes))
        {
            return true;
        }
        else if (new BruteForce().DoImagesFit(boundry, sizes))
        {
            return true;
        }
        return false;
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

