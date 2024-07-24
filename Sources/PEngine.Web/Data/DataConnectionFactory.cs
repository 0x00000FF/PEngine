using Npgsql;
using System.Data;

namespace PEngine.Web.Data
{
    public class DataConnectionFactory<T> where T : IDbConnection
    {
        private string _connectionString;

        public DataConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public T Create(IServiceProvider _)
        {
            var type = typeof(T);
            return (T) Activator.CreateInstance(type, _connectionString)!;
        }
    }
}
