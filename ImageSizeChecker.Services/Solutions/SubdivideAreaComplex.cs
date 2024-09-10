
namespace ImageSizeChecker.Services.Solutions;
internal class SubdivideAreaComplex : ISolution
{
    // Philosphy:
    // Order the images by largest area,
    // When you place a rectangle, subdivide the remaining space into two more rectangles    //      
    // You will get a series of "empty space" rectangles that you can try to fill. 
    // This has an issue of not being able to re-merge two subdivided rectangles even though visually you can, but should preform quickly on the cases
    // that do qualify. 
    // - Complex addition - when we subdivide the two remaining rectangles, we overlap both rectangles and give them an ID
    // We give these bins "safe space" (their non-intersected parts) and "pseudo space" (their extended width/heights)
    // and try to fit images into the safe spaces, and when we cant, we can extend into pseudo space.
    // once a bin with a given id has claimed psuedo space, the other bin with that id needs to be updated so it cant claim the psuedo space
    public bool DoImagesFit(Size boundry, Size[] images)
    {
        var imagesList = images.ToList();

        Dfs(new List<SizeExtended> { new SizeExtended(boundry) }, imagesList);
        return imagesList.Count == 0;
    }

    private void Dfs(List<SizeExtended> emptyBins, List<Size> images)
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

                    ////Remaining vertical, full horizontal
                    //emptyBins.Add(new Size(bin.Width, bin.Height - item.Height));
                    ////Remaining vertical, remaining horizontal
                    //emptyBins.Add(new Size(bin.Width - item.Width, bin.Height - item.Height));
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
public class SizeExtended : Size
{
    public SizeExtended(Size size, int width = 0, int height = 0) : base(size.Width, size.Height)
    {
        WidthExtension = width;
        HeightExtension = height;

    }
    public Guid Id { get; set; }
    public int WidthExtension { get; set; }
    public int HeightExtension { get; set; }
}