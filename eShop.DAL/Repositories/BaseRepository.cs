using eShop.Core.Entities;
using eShop.DAL.Data;
using eShop.Core.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Reflection;
using static Dapper.SqlMapper;

namespace eShop.DAL.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly eShopDbContext _context;
        private readonly DbSet<TEntity> _entity;
        public BaseRepository(eShopDbContext context)
        {
            _context = context;
            _entity = context.Set<TEntity>();
        }
        protected string GetTableName()
        {
            string tableName = "";
            var type = typeof(TEntity);
            var tableAttr = type.GetCustomAttribute<TableAttribute>();
            if (tableAttr != null)
            {
                tableName = tableAttr.Name;
                return tableName;
            }
            return type.Name + "s";
        }
        protected string GetKeyColumnName()
        {
            PropertyInfo[] properties = typeof(TEntity).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                object[] keyAttributes = property.GetCustomAttributes(typeof(KeyAttribute), true);

                if (keyAttributes != null && keyAttributes.Length > 0)
                {
                    object[] columnAttributes = property.GetCustomAttributes(typeof(ColumnAttribute), true);

                    if (columnAttributes != null && columnAttributes.Length > 0)
                    {
                        ColumnAttribute columnAttribute = (ColumnAttribute)columnAttributes[0];
                        return columnAttribute.Name;
                    }
                    else
                    {
                        return property.Name;
                    }
                }
            }
            return null;
        }

        protected string GetConnectionString()
        {
            return _context.Database.GetConnectionString();
        }
        public IEnumerable<TEntity> GetAll()
        {
            try
            {
                using (var _connection = new SqlConnection(GetConnectionString()))
                {
                    string tableName = GetTableName();
                    string query = $"SELECT * FROM {tableName}";
                    return _connection.Query<TEntity>(query);
                }
            }
            catch (Exception ex) { }
            return null;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            try
            {
                using (var _connection = new SqlConnection(GetConnectionString()))
                {
                    string tableName = GetTableName();
                    string query = $"SELECT * FROM {tableName}";
                    return await _connection.QueryAsync<TEntity>(query);
                }
            }
            catch (Exception ex) { }
            return null;
        }

        public TEntity GetById(Guid id)
        {
            try
            {
                using (var _connection = new SqlConnection(GetConnectionString()))
                {
                    string tableName = GetTableName();
                    string idName = GetKeyColumnName();
                    string query = $"SELECT * FROM {tableName} where {idName} = {id}";
                    return _connection.QueryFirstOrDefault<TEntity>(query);
                }
            }
            catch (Exception ex) { }
            return null;
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            try
            {
                using (var _connection = new SqlConnection(GetConnectionString()))
                {
                    string tableName = GetTableName();
                    string idName = GetKeyColumnName();
                    string query = $"SELECT * FROM {tableName} where {idName} = {id}";
                    return await _connection.QueryFirstOrDefaultAsync<TEntity>(query);
                }
            }
            catch (Exception ex) { }
            return null;
        }
        public TEntity Find(Expression<Func<TEntity, bool>> criteria)
        {
            return _entity.SingleOrDefault(criteria);
        }

        public TEntity Find(Expression<Func<TEntity, bool>> criteria, string[] includes = null)
        {
            IQueryable<TEntity> Data = _entity;

            if (includes != null)
                foreach (var incluse in includes)
                    Data = Data.Include(incluse);

            return Data.SingleOrDefault(criteria);
        }

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> criteria)
        {
            IQueryable<TEntity> Data = _entity;

            return await Data.SingleOrDefaultAsync(criteria);
        }
        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> criteria, string[] includes = null)
        {
            IQueryable<TEntity> Data = _context.Set<TEntity>();

            if (includes != null)
                foreach (var incluse in includes)
                    Data = Data.Include(incluse);

            return await Data.SingleOrDefaultAsync(criteria);
        }
        public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> criteria)
        {
            IQueryable<TEntity> Data = _entity;

            return Data.Where(criteria).ToList();
        }

        public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> criteria, string[] includes = null)
        {
            IQueryable<TEntity> Data = _entity;

            if (includes != null)
                foreach (var include in includes)
                    Data = Data.Include(include);

            return Data.Where(criteria).ToList();
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> criteria)
        {
            IQueryable<TEntity> Data = _entity;

            return await Data.Where(criteria).ToListAsync();
        }
        public async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> criteria, string[] includes = null)
        {
            IQueryable<TEntity> Data = _entity;

            if (includes != null)
                foreach (var include in includes)
                    Data = Data.Include(include);

            return await Data.Where(criteria).ToListAsync();
        }

        public TEntity Add(TEntity entity)
        {
            _entity.Add(entity);
            return entity;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _entity.AddAsync(entity);
            return entity;
        }

        public IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities)
        {
            _entity.AddRange(entities);
            return entities;
        }

        public async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _entity.AddRangeAsync(entities);
            return entities;
        }

        public TEntity Update(TEntity entity)
        {
            _entity.Update(entity);
            return entity;
        }

        public void Delete(TEntity entity)
        {
            _entity.Remove(entity);
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            _entity.RemoveRange(entities);
        }

        public void Attach(TEntity entity)
        {
            _entity.Attach(entity);
        }

        public void AttachRange(IEnumerable<TEntity> entities)
        {
            _entity.AttachRange(entities);
        }

        public int Count()
        {
            return _entity.Count();
        }

        public int Count(Expression<Func<TEntity, bool>> criteria)
        {
            return _entity.Count(criteria);
        }

        public async Task<int> CountAsync()
        {
            return await _entity.CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> criteria)
        {
            return await _entity.CountAsync(criteria);
        }

    }
}