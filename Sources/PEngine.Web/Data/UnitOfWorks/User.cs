using Npgsql;

namespace PEngine.Web.Data.UnitOfWorks
{
    public class User : UnitOfWork
    {   
        public User(NpgsqlConnection connection) : base(connection) { }
    }
}
