using System.Threading.Tasks;

namespace Library.Repository.Interfaces
{
    public interface IUnitofWork
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

        int SaveChanges();
        Task<int> SaveChangesAsync();
    }

    public interface IUnitofWork<TContext> : IUnitofWork
    {
    }
}
