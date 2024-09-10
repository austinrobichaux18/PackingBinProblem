
namespace ImageSizeChecker.Services.Solutions;
internal class SubdivideArea : ISolution
{
    private readonly SubdivideFavor _favor;
    public SubdivideArea(SubdivideFavor favor) => _favor = favor;
    // Philosphy:
    // Order the images by largest area,
    // When you place a rectangle, subdivide the remaining space into two more rectangles
    // You will get a series of "empty space" rectangles that you can try to fill. 
    // This has an issue of not being able to re-merge two subdivided rectangles even though visually you can, but should preform quickly on the cases
    // that do qualify. 
    public bool DoImagesFit(Size boundry, Size[] images)
    {
        var imagesList = images.ToList();

        Dfs([boundry], imagesList);
        return imagesList.Count == 0;
    }

    private void Dfs(List<Size> emptyBins, List<Size> images)
    {
        if (images.Count == 0)
        {
            return;
        }
        var imageAdded = false;
        //Smallest bin first
        foreach (var bin in emptyBins.OrderBy(x => x.Area).ToList())
        {
            //Biggest image first
            foreach (var item in images.OrderByDescending(x => x.Area).ToList())
            {
                //Does image fit in bin?
                if (bin.Width >= item.Width && bin.Height >= item.Height)
                {
                    imageAdded = true;

                    emptyBins.Remove(bin);
                    images.Remove(item);

                    if (_favor == SubdivideFavor.Vertical)
                    {
                        //Full vertical, right side horizontal
                        emptyBins.Add(new Size(bin.Width - item.Width, bin.Height));
                        //Remaining vertical, remaining horizontal
                        emptyBins.Add(new Size(bin.Width - item.Width, bin.Height - item.Height));
                    }
                    else if (_favor == SubdivideFavor.Horizontal)
                    {
                        //Remaining vertical, full horizontal
                        emptyBins.Add(new Size(bin.Width, bin.Height - item.Height));
                        //Remaining vertical, remaining horizontal
                        emptyBins.Add(new Size(bin.Width - item.Width, bin.Height - item.Height));
                    }
                    break;
                }
            }
        }

        if (imageAdded)
        {
            Dfs(emptyBins, images);
        }
    }
}

public enum SubdivideFavor
{
    Horizontal = 0,
    Vertical = 1,
}
