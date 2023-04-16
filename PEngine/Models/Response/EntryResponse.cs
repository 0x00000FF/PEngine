using System.Data;

namespace PEngine.Models.Response;

public class EntryResponse : Response
{
    public Guid? Id { get; set; }

    public string? Type { get; set; }
    public string? Name { get; set; }
}

public class EntityQueryResponse<T> : EntryResponse
{
    public T? Entity { get; set; }
    
    public bool CanRead { get; set; }
    public bool CanWrite { get; set; }
    public bool CanExecute { get; set; }
    
    public EntityQueryResponse(EntryResponse response)
    {
        Id = response.Id;
        Status = response.Status;
        Type = response.Type;
        Name = response.Name;
    }
}

public class EntityActionResponse : EntryResponse
{
    public bool Updated { get; set; }
}