using Dapper;
using Dapper.Contrib.Extensions;
using PEngine.Models.Data;
using PEngine.Models.Query;

namespace PEngine.Repositories;

public class EntryRepository : RepositoryBase
{
    public async Task<IEnumerable<EntryPathLookupResult>> LookupDirectory(string path)
    {
        return await Connection.QueryAsync<EntryPathLookupResult>(
            $"SELECT * FROM \"EntryPath_Lookup\"(@path) " +
            $"WHERE \"Type\" = @t1 OR \"Type\" = @t2",
            new { path, t1 = EntryType.Directory, t2 = EntryType.Post });
    }
}