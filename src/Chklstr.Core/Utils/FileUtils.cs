namespace Chklstr.Core.Utils;

public static class FileUtils
{
    public static string? EnsureDirectoryExists(string? path)
    {
        if (path == null) return path;
        
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        return path;
    }
}