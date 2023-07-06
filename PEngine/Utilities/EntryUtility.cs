namespace PEngine.Utilities;

public static class EntryUtility
{
    public static Queue<string?> MakePathQueue(string path)
    {
        var resultQueue = new Queue<string?>();

        if (path.StartsWith('/'))
        {
            resultQueue.Enqueue(null);
        }
    }
}