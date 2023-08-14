using System.Data.SqlTypes;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PEngine.Web.Helper;

public enum BasePath
{
    StorageBase,
    UploadBase,
    IntroductionBase,
    ThumbnailsBase,
    SettingsBase
}

public static class FileHelper
{
    private static string StorageBase => Path.GetFullPath("Storage");
    
    private static Dictionary<BasePath, string> BaseMap = new()
    {
        { BasePath.UploadBase, $"{StorageBase}/Uploads"},
        { BasePath.IntroductionBase, $"{StorageBase}/Introduction"},
        { BasePath.ThumbnailsBase, $"{StorageBase}/Thumbnails"},
        { BasePath.SettingsBase, $"{StorageBase}/Settings"}
    };
    
    static FileHelper()
    {
        if (!Directory.Exists(StorageBase))
        {
            Directory.CreateDirectory(StorageBase);
        }

        foreach (var map in BaseMap)
        {
            if (!Directory.Exists(map.Value))
            {
                Directory.CreateDirectory(map.Value);
            }
        }
    }
    
    private static Stream LoadAsStream(string path)
    {
        return File.Exists(path) ? File.OpenRead(path) : Stream.Null;
    }

    private static void SaveFromStream(string path, Stream sourceStream)
    {
        if (sourceStream == Stream.Null)
        {
            return;
        }
        
        using var stream = File.OpenWrite(path);
        sourceStream.CopyTo(stream);
    }

    private static string SelectBase(BasePath basePath)
    {
        return basePath switch
        {
            BasePath.StorageBase => StorageBase,
            _ => BaseMap[basePath]
        };
    }

    private static bool IsPathInRange(string basePath, string fullPath)
    {
        return fullPath.StartsWith(basePath);
    }

    private static bool IsSafePath(BasePath basePathSelector, string path, out string outFullPath)
    {
        if (path.StartsWith(".."))
        {
            outFullPath = "";
            return false;
        }
        
        var basePath = SelectBase(basePathSelector);
        var fullPath = Path.GetFullPath($"{basePath}/{path}");

        outFullPath = fullPath;
        
        return IsPathInRange(basePath, fullPath);
    }

    private static T SafePathDelegate<T>(BasePath basePathSelector, string path, Func<string, T> callback)
    {
        if (!IsSafePath(basePathSelector, path, out var fullPath))
        {
            throw new InvalidOperationException();
        }

        return callback(fullPath);
    }
    
    private static void SafePathDelegate(BasePath basePathSelector, string path, Action<string> callback)
    {
        if (!IsSafePath(basePathSelector, path, out var fullPath))
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