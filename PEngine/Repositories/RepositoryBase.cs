using System.Data;
using Npgsql;

namespace PEngine.Repositories;

public abstract class RepositoryBase
{
    private static string? _connectionString = "";

    public static string? ConnectionString
    {
        private get => _connectionString;
        set => _connectionString = _connectionString ?? value;
    }
    protected IDbConnection Connection;

    public RepositoryBase()
    {
        Connection = new NpgsqlConnection(_connectionString);
    }
}