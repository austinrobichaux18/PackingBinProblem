
namespace ImageSizeChecker.Services.Solutions;
internal class SubdivideAreaComplex : ISolution
{
    // Philosphy:
    // Order the images by largest area,
    // When you place a rectangle, subdivide the remaining space into two more rectangles, preferring the larger one be either vertical or horizontal.
    // You will get a series of "empty space" rectangles (bins) that you can try to fill. 
    // This has an issue of not being able to re-merge two subdivided rectangles even though visually you can, but should preform quickly on the cases
    // that do qualify. 
    // - Complex addition - when we subdivide the two remaining rectangles, we overlap both rectangles and give them an ID
    // We give these bins "safe space" (their non-intersected parts) and "pseudo space" (their extended width/heights that intersect)
    // and try to fit images into the safe spaces, and when we cant, we can extend into pseudo space.
    // once a bin with a given id has claimed psuedo space, the other bin with that id needs to be updated so it cant claim the psuedo space
    // This is not optimal (denying the other bin its psuedo space).
    // Optimally we'd update the available space in the other bin, likely making more psuedo space bins, but this would get
    //      extremely complex as you lose the ability to know how the bins can be oriented and re-merged
    // Cons - This still inherently places things in the order of area, and doesnt rearrange the box. 
    //        This algorithm is just a smart way to place things the first time O(n) without removing and re-adding (which would be non-polynomial time complexity AKA "a lot")
    public bool DoImagesFit(Size boundry, Size[] images)
    {
        var imagesList = images.ToList();

        var psuedoSpaceDictionary = new Dictionary<Guid, bool>();
        Dfs(new List<SizeExtended> { new SizeExtended(boundry, Guid.Empty) }, imagesList, psuedoSpaceDictionary);
        return imagesList.Count == 0;
    }

    private void Dfs(List<SizeExtended> emptyBins, List<Size> images, Dictionary<Guid, bool> psuedoSpaceDictionary)
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

                    //SizeExtendedId and 'can use psuedo space' bool
                    var id = Guid.NewGuid();
                    psuedoSpaceDictionary.Add(id, true);

                    //space to the right of the image 
                    emptyBins.Add(new SizeExtended(new Size(bin.Width - item.Width, item.Height), id, height: bin.Height - item.Height));
                    //space below the image
                    emptyBins.Add(new SizeExtended(new Size(item.Width, bin.Height - item.Height), id, width: bin.Width - item.Height));
                    break;
                }
                //Does this bin id have the ability to use it's psuedo space, and does it fit in the bin's extended psuedo space?
                else if (psuedoSpaceDictionary[bin.Id] && bin.Width + bin.WidthExtension >= item.Width && bin.Height + bin.HeightExtension >= item.Height)
                {
                    psuedoSpaceDictionary[bin.Id] = false;

                    imageAdded = true;
                    emptyBins.Remove(bin);
                    images.Remove(item);

                    var id = Guid.NewGuid();
                    psuedoSpaceDictionary.Add(id, true);

                    //space to the right of the image 
                    emptyBins.Add(new SizeExtended(new Size(bin.Width - item.Width, item.Height), id, height: bin.Height - item.Height));
                    //space below the image
                    emptyBins.Add(new SizeExtended(new Size(item.Width, bin.Height - item.Height), id, width: bin.Width - item.Height));
                }
            }
        }

        if (imageAdded)
        {
            Dfs(emptyBins, images, psuedoSpaceDictionary);
        }
    }
}
public class SizeExtended : Size
{
    public SizeExtended(Size size, Guid id, int width = 0, int height = 0) : base(size.Width, size.Height)
    {
        Id = id;
        WidthExtension = width;
        HeightExtension = height;
    }
    public Guid Id { get; set; }
    public int WidthExtension { get; set; }
    public int HeightExtension { get; set; }
}