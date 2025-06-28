using eShop.Core.Entities;
using eShop.DAL.Data;
using eShop.DAL.Repositories.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Reflection;
using static Dapper.SqlMapper;

namespace eShop.DAL.Repositories.implementation
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly eShopDbContext _context;
        private readonly DbSet<TEntity> _entity;
        private readonly string _CN;

        public BaseRepository(eShopDbContext context)
        {
            _context = context;
            _entity = context.Set<TEntity>();
            _CN = GetConnectionString();
        }
        protected string GetTableName(Type entityType)
        {
            string tableName = "";
            var tableAttr = entityType.GetCustomAttribute<TableAttribute>();
            if (tableAttr != null)
            {
                tableName = tableAttr.Name;
            }
            else
            {
                tableName = entityType.Name + "s";
            }
            return tableName;
        }

        // Update from reflection to EF metaData for reading fluent api
        protected string GetKeyColumnName(Type entityType)
        {
            var entity = _context.Model.FindEntityType(entityType);
            if (entity == null)
                return string.Empty;

            var key = entity.FindPrimaryKey();
            if (key == null || key.Properties.Count == 0)
                return string.Empty;

            var keyProperty = key.Properties[0];

            var tableName = entity.GetTableName();
            var schema = entity.GetSchema();

            return keyProperty.GetColumnName(StoreObjectIdentifier.Table(tableName, schema));
        }

        protected string GetConnectionString()
        {
            return _context.Database.GetConnectionString();
        }
        public IEnumerable<TEntity> GetAll()
        {
            try
            {
                using (var _connection = new SqlConnection(_CN))
                {
                    string tableName = GetTableName(typeof(TEntity));
                    string query = $"SELECT * FROM {tableName} WHERE IsDeleted is false";
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
                using (var _connection = new SqlConnection(_CN))
                {
                    string tableName = GetTableName(typeof(TEntity));
                    string query = $"SELECT * FROM {tableName} WHERE IsDeleted = 'false'";
                    return await _connection.QueryAsync<TEntity>(query);
                }
            }
            catch (Exception ex) { }
            return null;
        }

        public TEntity GetById(Guid Id)
        {
            try
            {
                using (var _connection = new SqlConnection(_CN))
                {
                    string tableName = GetTableName(typeof(TEntity));
                    string idName = GetKeyColumnName(typeof(TEntity));
                    string query = $"SELECT * FROM {tableName} where {idName} = @Id AND IsDeleted = 'false'";
                    return _connection.QueryFirstOrDefault<TEntity>(query, new { Id = Id });
                }
            }
            catch (Exception ex) { }
            return null;
        }

        public async Task<TEntity> GetByIdAsync(Guid Id)
        {
            try
            {
                using (var _connection = new SqlConnection(_CN))
                {
                    string tableName = GetTableName(typeof(TEntity));
                    string idName = GetKeyColumnName(typeof(TEntity));
                    string query = $"SELECT * FROM {tableName} where {idName} = @Id and IsDeleted = 'false'";

                    return await _connection.QueryFirstOrDefaultAsync<TEntity>(query, new { Id = Id });
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

        public IEnumerable<TEntity> Skip(int count)
            => _entity.Skip(count);
        
        public IEnumerable<TEntity> Take(int count)
            => _entity.Take(count);

        public IEnumerable<TEntity> Include(string navigationPropert)
            => _entity.Include(navigationPropert);
    }
}