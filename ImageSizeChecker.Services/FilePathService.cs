namespace ImageSizeChecker.Services;
public static class FilePathService
{
    public static string GetFilePath(string fileName)
        => Directory.GetCurrentDirectory() + "..\\" + "..\\" + "..\\" + "..\\" + fileName;
}
