using Dapper;
using eShop.Core.Entities;
using eShop.DAL.Data;
using eShop.DAL.Interface;
using Microsoft.Data.SqlClient;

namespace eShop.DAL.Repositories.implementation
{
    internal class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        private readonly string _CN;
        public ProductRepository(eShopDbContext context) : base(context)
        {
            _CN = GetConnectionString();
        }

        public Image GetProductImage(Guid productId)
        {
            try
            {
                using (var _connection = new SqlConnection(_CN))
                {
                    string tableName = GetTableName(typeof(Image));
                    string query = $"SELECT TOP 1 * FROM {tableName} where  TargetId = @ProductId ";
                    return _connection.QueryFirstOrDefault<Image>(query, new { ProductId = productId });
                }
            }
            catch (Exception ex)
            {
                return new Image();
            }
        }
        public async Task<Image> GetProductImageAsync(Guid productId)
        {
            try
            {
                using (var _connection = new SqlConnection(_CN))
                {
                    string tableName = GetTableName(typeof(Image));
                    string query = $"SELECT TOP 1 * FROM {tableName} where TargetId = @ProductId ";
                    return await _connection.QueryFirstOrDefaultAsync<Image>(query, new { ProductId = productId });
                }
            }
            catch (Exception ex)
            {
                return new Image();
            }
        }
        public IEnumerable<Image> GetProductImages(Guid productId)
        {
            try
            {
                using (var _connection = new SqlConnection(_CN))
                {
                    string tableName = GetTableName(typeof(Image));
                    string query = $"SELECT * FROM {tableName} where TargetId = @ProductId ";
                    return _connection.Query<Image>(query, new { ProductId = productId });
                }
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<Image>();
            }
        }

        public async Task<IEnumerable<Image>> GetProductImagesAsync(Guid productId)
        {
            try
            {
                using (var _connection = new SqlConnection(_CN))
                {
                    string tableName = GetTableName(typeof(Image));
                    string query = $"SELECT * FROM {tableName} where TargetId = @ProductId ";
                    return await _connection.QueryAsync<Image>(query, new { ProductId = productId });
                }
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<Image>();
            }
        }
    }
}
