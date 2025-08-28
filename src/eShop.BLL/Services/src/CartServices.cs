namespace eShop.BLL.Services.src;

using System.Security.Claims;
using System.Text.Json;
using Abstraction;
using AutoMapper;
using Core.Entities;
using Core.Utilities;
using DAL.Interface;
using Microsoft.AspNetCore.Http;
using ViewModels.Cart;

public class CartService(IUnitOfWork context, IMapper mapper, IHttpContextAccessor httpContextAccessor) : ICartService {
    private ISession _userSession = httpContextAccessor.HttpContext.Session;

    public async Task<List<CartItem>> GetCartItemsAsync(string userId) {
        List<CartItem> CartItems = [];

        if (userId != Session.Anonymous){
            var carts = await context.Carts
                .FindAllAsync(c => c.UserId == userId, e => e.Product);
            CartItems = mapper.Map<List<CartItem>>(carts.ToList());
        }
        return CartItems;
    }

    public async Task<bool> AddToCartAsync(string userId, Guid productId, int quantity) {
        var product = await context.Products.FindAsync(c => c.Id == productId);

        if (userId != Session.Anonymous){
            var existingItem = await context.Carts.FindAsync(c => c.UserId == userId && c.ProductId == productId);
            if ((existingItem != null && product.Quantity < (quantity + existingItem.Quantity)) || (product.Quantity < quantity)){ return false; }

            if (existingItem != null){ existingItem.Quantity += quantity; }
            else{
                var cart = new Cart
                {
                    CreatedDate = DateTime.Now,
                    Quantity = quantity,
                    UserId = userId,
                    ProductId = productId,
                };
                context.Carts.Add(cart);
            }
        }
        return context.Complete() > 0;
    }

    public async Task<bool> UpdateQuantityAsync(string userId, Guid productId, int quantity) {
        bool rus = false;
        var product = await context.Products.FindAsync(c => c.Id == productId);
        if (userId != Session.Anonymous){
            var cartItem = await context.Carts
                .FindAsync(c => c.UserId == userId && c.ProductId == productId);
            if (cartItem != null){
                if (quantity <= 0){ context.Carts.Delete(cartItem); }
                else if (product.Quantity >= quantity){ cartItem.Quantity = quantity; }
                rus = context.Complete() > 0;
            }
            else{ rus = await AddToCartAsync(userId, productId, quantity); }
        }
        return rus;
    }

    public async Task<bool> RemoveFromCartAsync(string userId, Guid productId) {
        var cartItem = await context.Carts
            .FindAsync(c => c.UserId == userId && c.ProductId == productId);

        if (cartItem != null){ context.Carts.Delete(cartItem); }
        return context.Complete() > 0;
    }

    public async Task<CartSummary> GetCartSummaryAsync(string userId) {
        var items = await GetCartItemsAsync(userId);
        var subtotal = items.Sum(i => i.Price * i.Quantity);
        var tax = subtotal * 0.14m; // 14% tax

        return new CartSummary
        {
            Items = items,
            Subtotal = subtotal,
            Tax = tax,
            Total = subtotal + tax,
            TotalItems = items.Sum(i => i.Quantity)
        };
    }

    public async Task ClearCartAsync(string userId) {
        if (userId != Session.Anonymous){
            var items = await context.Carts
                .FindAllAsync(c => c.UserId == userId);

            context.Carts.DeleteRange(items);
            context.Complete();
        }
    }
    public async Task<int> UserCartCountAsync(string userId) {
        return await context.Carts
            .CountAsync(c => c.UserId == userId);
    }
    private void SetObject<T>(ref ISession session, string key, T value) {
        var json = JsonSerializer.Serialize(value);
        session.SetString(key, json);
    }

    private T? GetObject<T>(ISession session, string key) {
        var value = session.GetString(key);
        return value == null ? default : JsonSerializer.Deserialize<T>(value);

    }
    /// <summary>
    ///  Add Cart if not found and update if found
    /// </summary>
    /// <param name="userSession"></param>
    /// <param name="product"></param>
    /// <param name="quantity">if quantity equal zero will be deleted</param>
    private void UpdateCartsSession(ref ISession userSession, Product product, int quantity) {
        var CartItems = GetObject<List<CartItem>>(_userSession, Session.Products);
        if (quantity == 0 && CartItems != null)
            CartItems.Remove(CartItems.FirstOrDefault(e => e.ProductId == product.Id));
        else if (quantity != 0 && CartItems != null)
            CartItems.Find(e => e.ProductId == product.Id).Quantity += quantity;
        else if (quantity != 0 && CartItems == null){
            CartItems = new()
            {
                new()
                {
                    UserId = default,
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.CurrentPrice,
                    Quantity = quantity,
                    CreatedAt = DateTime.Now,
                }
            };
        }

        SetObject(ref _userSession, Session.ProductsCount, CartItems?.Count ?? 0);
        SetObject(ref _userSession, Session.Products, CartItems);
    }
}