using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MyShop.DataAcess;
using MyShop.Entities.Models;
using MyShop.Entities.Repositiories;
using System;
using System.Linq.Expressions;
using System.Security.Claims;

namespace MyShop.DataAccess.Implementation
{
    public class ShoppingCart : GenericRepositiory<CartItem>,IShoppingCart
    {
        private readonly ApplicationDbConext _context;

        private readonly IHttpContextAccessor _httpContextAccessor;


        public ShoppingCart(ApplicationDbConext context, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public void AddItem(string userId, int itemId, int quantity)
        {
            var cartItem = _context.CartItems.SingleOrDefault(
                c => c.ApplicationUserId == userId && c.ProductId == itemId);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    ApplicationUserId = userId,
                    ProductId = itemId,
                    Quantity = quantity
                };
                _context.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
            }

            _context.SaveChanges();
        }

        public IEnumerable<CartItem> GetCartItems(Expression<Func<CartItem, bool>>? perdicate = null)
        {
            var query = _context.CartItems.Include(c => c.Product).AsQueryable();

            if (perdicate != null)
            {
                query = query.Where(perdicate);
            }

            return query.ToList();
        }
       
        public void ClearCart()
        {
            var cartItems = _context.CartItems.ToList();
            _context.CartItems.RemoveRange(cartItems);
            _context.SaveChanges();
        }

        public int GetCartItemCount()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                return 0;
            }

            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            var cart = _context.Carts.Include(c =>c.CartItems).FirstOrDefault(c => c.ApplicationUserId == userId);
            if (cart == null)
            {
                cart = new Cart
                {
                    ApplicationUserId = userId,
                    CartItems = new List<CartItem>()
                };

            }
            return cart?.CartItems.Sum(ci => ci.Quantity) ?? 0;
        }
    }
}
