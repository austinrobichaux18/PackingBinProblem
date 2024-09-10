using ImageSizeChecker.Services;

namespace ImageSizeChecker;

internal class Program
{
    private const string _fileName = "InputFile.txt";

    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the Image Size Checker Application!");
        Console.WriteLine("Checking Contents of InputFile.txt ... ");

        ReadFromInputFile();
    }

    private static void ReadFromInputFile()
    {
        var lines = File.ReadAllLines(FilePathService.GetFilePath(_fileName));
        var solution = new ImageSizeCheckerService().DoImagesFit(lines);

        Console.WriteLine(solution ? "All images fit!" : "These images do not fit within the master image.");
        Console.WriteLine();
    }
}
