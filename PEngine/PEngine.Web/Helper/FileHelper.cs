using System.Data.SqlTypes;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PEngine.Web.Helper;

public enum BasePath
{
    StorageBase,
    UploadBase,
    IntroductionBase,
    SettingsBase
}

public static class FileHelper
{
    private static string StorageBase => Path.GetFullPath("Storage");
    private static string UploadBasePath => $"{StorageBase}/Uploads";
    private static string IntroductionBasePath => $"{StorageBase}/Introduction";
    private static string SettingsBasePath => $"{StorageBase}/Settings";

    private static Stream LoadAsStream(string path)
    {
        var fullPath = $"{StorageBase}/{path}";

        foreach (var ch in Path.GetInvalidFileNameChars())
        {
            if (path.Contains(ch, StringComparison.InvariantCultureIgnoreCase))
            {
                return Stream.Null;
            }
        }

        return File.Exists(fullPath) ? File.OpenRead(fullPath) : Stream.Null;
    }

    private static void SaveFromStream(string path, Stream sourceStream)
    {
        if (sourceStream == Stream.Null)
        {
            return;
        }
        
        var fullPath = $"{StorageBase}/{path}";
        using var stream = File.OpenWrite(fullPath);
        
        sourceStream.CopyTo(stream);
    }

    private static string SelectBase(BasePath basePath)
    {
        switch (basePath)
        {
            case BasePath.StorageBase:
                return StorageBase;
            
            case BasePath.UploadBase:
                return UploadBasePath;
            
            case BasePath.IntroductionBase:
                return IntroductionBasePath;
            
            case BasePath.SettingsBase:
                return SettingsBasePath;
            
            default:
                throw new ArgumentOutOfRangeException(nameof(basePath), basePath, null);
        }
    }

    private static bool IsPathInRange(string basePath, string fullPath)
    {
        var normalizedPath = Path.GetFullPath(fullPath);

        return normalizedPath.StartsWith(basePath);
    }

    private static bool IsSafePath(BasePath basePathSelector, string path, out string outFullPath)
    {
        var basePath = SelectBase(basePathSelector);
        var fullPath = $"{basePathSelector}/{path}/";

        outFullPath = fullPath;
        
        return IsPathInRange(basePath, fullPath);
    }

    private static T SafePathDelegate<T>(BasePath basePathSelector, string path, Func<string, T> callback)
    {
        if (IsSafePath(basePathSelector, path, out var fullPath))
        {
            throw new InvalidOperationException();
        }

        return callback(fullPath);
    }
    
    private static void SafePathDelegate(BasePath basePathSelector, string path, Action<string> callback)
    {
        if (IsSafePath(basePathSelector, path, out var fullPath))
        {
            throw new InvalidOperationException();
        }

        callback(fullPath);
    }
    
    public static T? LoadFromJson<T>(BasePath basePath, string path)
    {
        return JsonSerializer.Deserialize<T>(
            SafePathDelegate(basePath, path, LoadAsStream)
            );
    }

    public static void SaveToJson(BasePath basePath, string path, object obj)
    {
        SafePathDelegate(basePath, path, (fullPath) =>
        {
            var bytes = JsonSerializer.SerializeToUtf8Bytes(obj);
            SaveFromStream(fullPath, new MemoryStream(bytes));
        });
    }
    
    public static Stream LoadAsStream(BasePath basePathSelector, string path)
    {
        return SafePathDelegate(basePathSelector, path, LoadAsStream);
    }

    public static void SaveFromStream(BasePath basePathSelector, string path, Stream sourceStream)
    {
        SafePathDelegate(basePathSelector, path, (fullPath) =>
        {
            SaveFromStream(fullPath, sourceStream);
        });
    }
}