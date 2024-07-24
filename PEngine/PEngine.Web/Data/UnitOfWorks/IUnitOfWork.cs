namespace PEngine.Web.Data.UnitOfWorks
{
    public interface IUnitOfWork
    {
        Task<int> CompleteAsync();
    }
}
