using ImageSizeChecker.Services;

namespace ImageSizeChecker;

internal class Program
{
    private const string _fileName = "InputFile.txt";
    private const int _readFromInputFile = 0;
    private const int _readFromFolderPicker = 1;

    // We have an input text file where each line specifies the width and the height(in pixels) of an image.
    // The first image is the size of the blank Master image.
    // Write a program that fits the rest of the images into this Master image, or prints an appropriate message if a solution could not be found.
    // To fit an image means to place it somewhere in the Master image, such that no two images overlap.
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the Image Size Checker Application!");

        ReadFromInputFile();
    }

    private static void ReadFromInputFile()
    {
        var lines = File.ReadAllLines(FilePathService.GetFilePath(_fileName));
        var solution = new ImageSizeCheckerService().DoImagesFit(lines);

        Console.WriteLine(solution ? "All images fit!" : "These images do not fit");
        Console.WriteLine();
    }
}
