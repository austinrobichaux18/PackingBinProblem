namespace ImageSizeChecker.Services.Solutions;
internal class BruteForce : ISolution
{
    // Philosphy:
    // Try to place each image in the first available spot that it would fit, ordered by area
    // If you reach a point where the image cannot be placed, we remove the previous image and try to place the current image.
    // Then we add the previous image back into the stack
    // This will repeat and cycle so that all permutations are attempted 
    public bool DoImagesFit(Size boundry, Size[] images)
    {
        var sizes = images.OrderByDescending(x => x.Area).ToList();
        var collage = new Collage(boundry);
        return Dfs(collage, sizes);
    }

    public bool Dfs(Collage collage, List<Size> images)
    {
        if (images.Count == 0)
        {
            return true;
        }

        var i = 0;
        foreach (var size in images.ToList())
        {
            var image = collage.TryPlaceImage(size);
            if (image != null)
            {
                images.RemoveAt(i);
                var nestedResult = Dfs(collage, images);
                if (nestedResult)
                {
                    return true;
                }
                images.Insert(i, size);
                collage.RemoveImage(image);
            }
            else
            {
                //Try to rotate the image and see if it fits
                image = collage.TryPlaceImage(new Size(size.Height, size.Width));
                if (image != null)
                {
                    images.RemoveAt(i);
                    var nestedResult = Dfs(collage, images);
                    if (nestedResult)
                    {
                        return true;
                    }
                    images.Insert(i, size);
                    collage.RemoveImage(image);
                }
            }
            i++;
        }
        return false;
    }

    internal class Collage
    {
        private readonly List<Image> Images = new List<Image>();
        private readonly Size _boundry;
        public Collage(Size boundry) => _boundry = boundry;

        // returns the image if it fits, null if it doesn't
        public Image? TryPlaceImage(Size size)
        {
            var x = 0;
            var y = 0;
            while (y < _boundry.Height)
            {
                var minHeight = int.MaxValue;
                while (x < _boundry.Width)
                {
                    //if it cant fully fit with this x,y top left point, skip
                    if (x + size.Width > _boundry.Width || y + size.Height > _boundry.Height)
                    {
                        break;
                    }

                    var existingImage = GetIntersectingImage(x, y, size);
                    if (existingImage == null)
                    {
                        var image = new Image(size, x, y);
                        Images.Add(image);
                        return image;
                    }
                    else
                    {
                        //Optimizes skipping known x,y distances that would be impossible to place into from existing image found
                        x += existingImage.Size.Width;
                        minHeight = minHeight > existingImage.Size.Height ?
                                       existingImage.Size.Height : minHeight;
                    }
                }
                x = 0;
                y += minHeight == int.MaxValue ? 1 : minHeight;
            }
            return null;
        }

        public void RemoveImage(Image image) => Images.Remove(image);

        private Image? GetIntersectingImage(int x, int y, Size size)
        {
            var image = new Image(size, x, y);
            foreach (var existingImage in Images)
            {
                if (DoImagesIntersect(existingImage, image))
                {
                    return existingImage;
                }
            }

            return null;
        }

        public bool DoImagesIntersect(Image image1, Image image2)
        {
            // Checks if two images overlap
            // See https://stackoverflow.com/questions/306316/determine-if-two-rectangles-overlap-each-other
            // Cond1. If A's left edge is to the right of the B's right edge, -then A is Totally to right Of B
            // Cond2. If A's right edge is to the left of the B's left edge, -then A is Totally to left Of B
            // Cond3. If A's top edge is below B's bottom edge, -then A is Totally below B
            // Cond4. If A's bottom edge is above B's top edge, -then A is Totally above B
            return image1.X < image2.X + image2.Size.Width
                && image1.X + image1.Size.Width > image2.X
                && image1.Y < image2.Y + image2.Size.Height
                && image1.Y + image1.Size.Height > image2.Y;
        }
    }
    public class Image
    {
        public Size Size { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Image(Size size, int x, int y)
        {
            Size = size;
            X = x;
            Y = y;
        }
    }
}
