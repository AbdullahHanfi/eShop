using eShop.Core.Entities;
using eShop.Core.Interface;
using eShop.DAL.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using static Dapper.SqlMapper;

namespace eShop.DAL.Repositories
{
    public class ImageRepository : BaseRepository<Image>, IImageRepository
    {
        private readonly string CN;
        public ImageRepository(eShopDbContext context) : base(context)
        {
            CN = context.Database.GetConnectionString();
        }

        public Image GetProductImage(Guid productId)
        {
            try
            {
                using (var _connection = new SqlConnection(CN))
                {
                    string tableName = GetTableName();
                    string query = $"SELECT TOP 1 * FROM {tableName} where  TargetId = {productId} ";
                    return _connection.QueryFirstOrDefault<Image>(query);
                }
            }
            catch (Exception ex) { }
            return null;
        }
        public async Task<Image> GetProductImageAsync(Guid productId)
        {
            try
            {
                using (var _connection = new SqlConnection(CN))
                {
                    string tableName = GetTableName();
                    string query = $"SELECT TOP 1 * FROM {tableName} where TargetId = {productId} ";
                    return await _connection.QueryFirstOrDefaultAsync<Image>(query);
                }
            }
            catch (Exception ex) { }
            return null;
        }
        public IEnumerable<Image> GetProductImages(Guid productId)
        {
            try
            {
                using (var _connection = new SqlConnection(CN))
                {
                    string tableName = GetTableName();
                    string query = $"SELECT * FROM {tableName} where TargetId = {productId} ";
                    return _connection.Query<Image>(query);
                }
            }
            catch (Exception ex) { }
            return null;
        }

        public async Task<IEnumerable<Image>> GetProductImagesAsync(Guid productId)
        {
            try
            {
                using (var _connection = new SqlConnection(CN))
                {
                    string tableName = GetTableName();
                    string query = $"SELECT * FROM {tableName} where TargetId = {productId} ";
                    return await _connection.QueryAsync<Image>(query);
                }
            }
            catch (Exception ex) { }
            return null;
        }
    }
}
