using System.Data;
using Npgsql;

namespace PEngine.Repositories;

public abstract class RepositoryBase
{
    private static string? ConnectionString => Program.WebsiteConfiguration?["Persistence:Database"];
    protected readonly IDbConnection Connection;

    protected RepositoryBase()
    {
        Connection = new NpgsqlConnection(ConnectionString);
    }
}