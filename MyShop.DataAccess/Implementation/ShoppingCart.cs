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
        private readonly ApplicationDbContext _context;

        private readonly IHttpContextAccessor _httpContextAccessor;


        public ShoppingCart(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public void AddItem(string userId, int itemId, int quantity)
        {
            // Ensure the cart exists for the user
            var cart = _context.Carts.FirstOrDefault(c => c.ApplicationUserId == userId);
            if (cart == null)
            {
                cart = new Cart
                {
                    ApplicationUserId = userId,
                    CartItems = new List<CartItem>()
                };
                _context.Carts.Add(cart);
                _context.SaveChanges(); // Save changes to ensure CartId is generated
            }

            // Check if the cart item already exists
            var cartItem = _context.CartItems.SingleOrDefault(
                c => c.ApplicationUserId == userId && c.ProductId == itemId);

            if (cartItem == null)
            {
                // If the cart item does not exist, create a new one
                cartItem = new CartItem
                {
                    ApplicationUserId = userId,
                    ProductId = itemId,
                    Quantity = quantity,
                    CartId = cart.Id // Associate the cart item with the cart
                };
                _context.CartItems.Add(cartItem);
            }
            else
            {
                // If the cart item already exists, update its quantity
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

        public int GetItemCount()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                return 0;
            }

            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = _context.Carts.Include(c =>c.CartItems).FirstOrDefault(c => c.ApplicationUserId == userId);
           
            return cart?.CartItems.Sum(ci => ci.Quantity) ?? 0;
        }

    }
}
