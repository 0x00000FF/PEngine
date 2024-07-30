using System.Collections;
using System.Data;

namespace PEngine.Web.Data.Repositories
{
    public abstract class Repository<T> : IRepository<T>
    {
        public Repository(IDbConnection connection)
        {

        }

        public void Add(T entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public T GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }

    }
}
