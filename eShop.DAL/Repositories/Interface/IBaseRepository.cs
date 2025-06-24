using eShop.Core.Entities;
using System.Linq.Expressions;

namespace eShop.DAL.Repositories.Interface
{
    public interface IBaseRepository<TEntity> 
        where TEntity : BaseEntity
    {
        //Dapper part
        TEntity GetById(Guid id);
        Task<TEntity> GetByIdAsync(Guid id);
        IEnumerable<TEntity> GetAll();
        Task<IEnumerable<TEntity>> GetAllAsync();


        //EF Core part
        TEntity Find(Expression<Func<TEntity, bool>> criteria);
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> criteria);
        IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> criteria);
        Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> criteria);


        TEntity Find(Expression<Func<TEntity, bool>> criteria, string[] includes = null);
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> criteria, string[] includes = null);
        IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> criteria, string[] includes = null);
        Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> criteria, string[] includes = null);


        TEntity Add(TEntity entity);
        Task<TEntity> AddAsync(TEntity entity);
        IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities);
        Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities);


        TEntity Update(TEntity entity);


        void Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);


        void Attach(TEntity entity);
        void AttachRange(IEnumerable<TEntity> entities);

        int Count();
        int Count(Expression<Func<TEntity, bool>> criteria);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<TEntity, bool>> criteria);

        IEnumerable<TEntity> Skip(int count);
        IEnumerable<TEntity> Take(int count);
        IEnumerable<TEntity> Include(string navigationPropert);
    }
}