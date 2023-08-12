namespace PEngine.Web.Helper;

public static class FileHelper
{
    private static string BasePath => Path.GetFullPath("Storage/Uploads");
    
    public static Stream LoadAsStream(string path)
    {
        var fullPath = Path.GetFullPath($"{BasePath}/{path}");

        foreach (var ch in Path.GetInvalidFileNameChars())
        {
            if (path.Contains(ch, StringComparison.InvariantCultureIgnoreCase))
            {
                return Stream.Null;
            }
        }

        return File.Exists(fullPath) ? File.OpenRead(fullPath) : Stream.Null;
    }

    public static void SaveFromStream(string path, Stream sourceStream)
    {
        if (sourceStream == Stream.Null)
        {
            return;
        }
        
        var fullPath = Path.GetFullPath($"{BasePath}/{path}");
        using var stream = File.OpenWrite(fullPath);
        
        sourceStream.CopyTo(stream);
    }
}