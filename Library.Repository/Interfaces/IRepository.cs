using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Library.Repository.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        TEntity Get(int id);
        Task<TEntity> GetAsync(object id);
        void Add(TEntity entity);
        void AddRange(IList<TEntity> entities);
        Task AddRangeAsync(IList<TEntity> entities);
        void Update(TEntity entity);
        Task AddAsync(TEntity entity);
        void Delete(TEntity entity);
        void DeleteRange(IList<TEntity> entities);
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        Task<int> RepoSaveAsync();
    }
}
