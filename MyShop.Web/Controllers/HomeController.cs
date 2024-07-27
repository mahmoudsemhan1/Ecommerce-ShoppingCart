using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyShop.Entities.Models.ViewModels;
using MyShop.Entities.Models;
using MyShop.Entities.Repositiories;

namespace MyShop.Web.Areas.Customre.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var Products = _unitOfWork.product.GetAll();
            return View(Products);
        }
        public IActionResult Details(int id)
        {

            ShoppingCart shoppingCart = new ShoppingCart()
            {
                product= _unitOfWork.category.GetById(id),
                Count=1
        };
         
            return View(shoppingCart);
        }
    }
}
