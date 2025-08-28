namespace eShop.DAL.Repositories.implementation;

using System.Xml;
using Core.Entities;
using Data;
using Interface;

public class OrderRepository(eShopDbContext dbContext) : BaseRepository<Order>(dbContext), IOrderRepository {
    /// <summary>
    /// When using with order it will create transaction handle product modification and adding order for concurrency control 
    /// </summary>
    /// <param name="entity"></param>
    /// <returns>object of success and null if failed</returns>
    public override Order Add(Order entity) {
        using var transaction = dbContext.Database.BeginTransaction();
        try{
            dbContext.Orders.Add(entity);
            foreach (var item in entity.Items){
                var product = dbContext.Products.Find(item.ProductId);
                if (product.Quantity >= item.Quantity)
                    product.Quantity -= item.Quantity;
                else
                    throw new Exception("Item Quantity exceed");
                dbContext.Products.Update(product);
            }
            dbContext.SaveChanges();
            transaction.Commit();
            return entity;
        }
        catch (Exception){
            transaction.Rollback();
            return null;
        }
    }
    /// <summary>
    /// When using with order it will create transaction handle product modification and adding order for concurrency control 
    /// </summary>
    /// <param name="entity"></param>
    /// <returns>object of success and null if failed</returns>
    public override async Task<Order> AddAsync(Order entity) {
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        try{
            await dbContext.Orders.AddAsync(entity);
            foreach (var item in entity.Items){
                var product = await dbContext.Products.FindAsync(item.ProductId);
                if (product.Quantity >= item.Quantity)
                    product.Quantity -= item.Quantity;
                else
                    throw new Exception("Item Quantity exceed");
                dbContext.Products.Update(product);
            }
            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
            return entity;
        }
        catch (Exception){
            await transaction.RollbackAsync();
            return null;
        }
    }
    /// <summary>
    /// When using with order it will create transaction handle product modification and adding order for concurrency control 
    /// </summary>
    /// <param name="entity"></param>
    /// <returns>object of success and null if failed</returns>
    public override async Task<IEnumerable<Order>> AddRangeAsync(IEnumerable<Order> entities) {
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        try{
            await dbContext.Orders.AddRangeAsync(entities);
            foreach (var entity in entities){

                foreach (var item in entity.Items){
                    var product = await dbContext.Products.FindAsync(item.ProductId);
                    if (product.Quantity >= item.Quantity)
                        product.Quantity -= item.Quantity;
                    else
                        throw new Exception("Item Quantity exceed");
                    dbContext.Products.Update(product);
                }
            }
            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
            return entities;
        }
        catch (Exception){
            await transaction.RollbackAsync();
            return null;
        }
    }
    /// <summary>
    /// When using with order it will create transaction handle product modification and adding order for concurrency control 
    /// </summary>
    /// <param name="entity"></param>
    /// <returns>object of success and null if failed</returns>
    public override IEnumerable<Order> AddRange(IEnumerable<Order> entities) {
        using var transaction = dbContext.Database.BeginTransaction();
        try{
            dbContext.Orders.AddRange(entities);
            foreach (var entity in entities){
                
                foreach (var item in entity.Items){
                    var product = dbContext.Products.Find(item.ProductId);
                    if (product.Quantity >= item.Quantity)
                        product.Quantity -= item.Quantity;
                    else
                        throw new Exception("Item Quantity exceed");
                    dbContext.Products.Update(product);
                }
            }
            dbContext.SaveChanges();
            transaction.Commit();
            return entities;
        }
        catch (Exception){
            transaction.Rollback();
            return null;
        }
    }
}