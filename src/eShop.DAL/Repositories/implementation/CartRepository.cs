namespace eShop.DAL.Repositories.implementation;

using Core.Entities;
using Data;
using Interface;

public class CartRepository : BaseRepository<Cart>,ICartRepository  {

    public CartRepository(eShopDbContext context) : base(context) {
    }
}