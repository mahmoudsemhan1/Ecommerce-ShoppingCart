using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.DataAccess.Implementation;
using MyShop.Entities.Models.ViewModels;
using MyShop.Entities.Repositiories;
using System.Security.Claims;

namespace MyShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ShoppingCart _shoppingCart;
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public  int TotalCarts { get; set; }

		public CartController(IUnitOfWork unitOfWork, ShoppingCart shoppingCart)
		{
			_unitOfWork = unitOfWork;
			_shoppingCart = shoppingCart;
		}

		public IActionResult Index()
        {
            var ClaimIdentity = (ClaimsIdentity)User.Identity;
            var claim =ClaimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var product = _unitOfWork.product.GetAll();
			

			ShoppingCartVM = new ShoppingCartVM()
            {
                CartItemsList = _shoppingCart.GetCartItems(u => u.ApplicationUserId==claim.Value)
            };
			foreach (var item in ShoppingCartVM.CartItemsList)
			{
				ShoppingCartVM.TotalCarts += (item.Quantity * item.Product.Price);
			}

			return View(ShoppingCartVM);
        }
        [HttpGet]
        public IActionResult GetCartItemCount()
        {
            int count = _shoppingCart.GetCartItemCount(); // Method to get cart item count
            return Json(new { count });
        }
        [HttpGet]
        public IActionResult Plus(int CartItemId)
        {
            var CartItem = _shoppingCart.GetFirstOrDefualt(u=>u.CartItemId== CartItemId);
            {
                CartItem.Quantity += 1;
                _unitOfWork.Complete();
            }
			return RedirectToAction("Index");
		}
       
        [HttpGet]
        public IActionResult Minus(int CartItemId)
        {
			var CartItem = _shoppingCart.GetFirstOrDefualt(u=>u.CartItemId== CartItemId);

            if (CartItem.Quantity<= 1)
            {
                _shoppingCart.Remove(CartItem);
				_unitOfWork.Complete();

				return RedirectToAction("Index", "Home", new { area = "" });

			}
			else
            {
                CartItem.Quantity -= 1;
            }
			_unitOfWork.Complete();

			return RedirectToAction("Index");
		}
        [HttpGet]
        public IActionResult Remove(int CartItemId)
        {
			var CartItem = _shoppingCart.GetFirstOrDefualt(u => u.CartItemId == CartItemId);
			_shoppingCart.Remove(CartItem);
			_unitOfWork.Complete();
			return RedirectToAction("Index");
        }

        public IActionResult Summary()
        {
			var ClaimIdentity = (ClaimsIdentity)User.Identity;
			var claim = ClaimIdentity.FindFirst(ClaimTypes.NameIdentifier);
			var product = _unitOfWork.product.GetAll();


			ShoppingCartVM = new ShoppingCartVM()
			{
				CartItemsList = _shoppingCart.GetCartItems(u => u.ApplicationUserId == claim.Value),
				OrderHeader=new()
			};
			ShoppingCartVM.OrderHeader.ApplicationUser=_unitOfWork.ApplicationUser.GetFirstOrDefualt(x=>x.Id==claim.Value);
			ShoppingCartVM.OrderHeader.Adress = ShoppingCartVM.OrderHeader.ApplicationUser.Adress;
			ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
			ShoppingCartVM.OrderHeader.Phone = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;

			foreach (var item in ShoppingCartVM.CartItemsList)
			{
				ShoppingCartVM.TotalCarts += (item.Quantity * item.Product.Price);
			}

			return View(ShoppingCartVM);
		}

    }
}
