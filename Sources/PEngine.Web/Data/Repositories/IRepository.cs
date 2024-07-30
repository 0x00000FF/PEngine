namespace PEngine.Web.Data.Repositories
{
    public interface IRepository
    {

    }

    public interface IRepository<T> : IRepository
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
