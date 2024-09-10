using System.Reflection;

namespace ImageSizeChecker.Services;
public static class FilePathService
{
    public static string GetFilePath(string fileName)
    {
        var executableLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        return Path.Combine(executableLocation, fileName);
    }
}
