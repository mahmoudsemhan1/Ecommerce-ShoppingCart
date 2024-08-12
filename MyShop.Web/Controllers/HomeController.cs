using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyShop.DataAccess.Implementation;
using MyShop.Entities.Models;
using MyShop.Entities.Repositiories;
using System.Security.Claims;
using X.PagedList;


namespace MyShop.Web.Areas.Customre.Controllers
{

    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ShoppingCart _shoppingCart;


        public HomeController(IUnitOfWork unitOfWork, ShoppingCart shoppingCart)
        {
            _unitOfWork = unitOfWork;
            _shoppingCart = shoppingCart;
        }

        public IActionResult Index(int? page)
        {
            var PageNumber = page ?? 1;
            int PageSize = 8;
            var Products = _unitOfWork.product.GetAll().ToPagedList(PageNumber, PageSize);
            return View(Products);
        }
        public IActionResult Details(int id)
        {
            if(!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var product = _unitOfWork.product.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public  IActionResult AddItemToShoppingCart(int id, int quantity)
        {
            var item = _unitOfWork.product.GetById(id);
            if(item != null)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrEmpty(userId))
                {
                    _shoppingCart.AddItem(userId, item.Id, quantity); // Add item to cart with quantity
                }
                else
                {
                    // Handle the case where the user ID is not found (e.g., user is not logged in)
                    return RedirectToAction("Login", "Account");
                }

            }
          
            return RedirectToAction(nameof(Index));
        }

    }
}
