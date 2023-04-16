namespace PEngine.Models.Response;

public static class EntryResponseExtension
{
    public static EntityQueryResponse<T> Entity<T>(this EntryResponse response, T entity) where T : IEntry
    {
        return Entity<T>(response, entity, false, false, false);
    }

    public static EntityQueryResponse<T> Entity<T>(this EntryResponse response, T entity, bool canRead, bool canWrite,
        bool canExecute) where T : IEntry
    {
        var newResponse = new EntityQueryResponse<T>(response)
        {
            Entity = entity,
            
            CanRead = canRead,
            CanWrite = canWrite,
            CanExecute = canExecute
        };

        return newResponse;
    }
}