namespace ImageSizeChecker.Services.Solutions;
internal class BruteForce : ISolution
{
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
            var image = collage.PlaceImage(size);
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
        public Image? PlaceImage(Size size)
        {
            var x = 0;
            var y = 0;
            while (y < _boundry.Height)
            {
                while (x < _boundry.Width)
                {
                    //if it cant fully fit with this x,y top left point, skip
                    if (x + size.Width > _boundry.Width || y + size.Height > _boundry.Height)
                    {
                        break;
                    }

                    var existingImage = FindIntersectingImage(x, y, size);
                    if (existingImage == null)
                    {
                        var image = new Image(size, x, y);
                        Images.Add(image);
                        return image;
                    }
                    else
                    {
                        x += existingImage.Size.Width;
                    }
                }
                x = 0;
                y++;
            }
            return null;
        }

        public void RemoveImage(Image image) => Images.Remove(image);

        private Image? FindIntersectingImage(int x, int y, Size size)
        {
            var image = new Image(size, x, y);

            foreach (var existingImage in Images)
            {
                if (ImagesIntersect(existingImage, image))
                {
                    return existingImage;
                }
            }

            return null;
        }

        public bool ImagesIntersect(Image image1, Image image2)
        {
            //    RectA.Left < RectB.Right
            // && RectA.Right > RectB.Left
            // && RectA.Top > RectB.Bottom
            // && RectA.Bottom < RectB.Top
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
