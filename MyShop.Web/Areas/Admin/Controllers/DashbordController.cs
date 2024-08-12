using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Myshop.Utilities;
using MyShop.Entities.Repositiories;

namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("AdminRole")]
    public class DashbordController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashbordController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            ViewBag.Orders = _unitOfWork.orderHeader.GetAll().Count();
            ViewBag.ApprovedOrders = _unitOfWork.orderHeader.GetAll(x=>x.OrderStatus==SD.Approve).Count();
            ViewBag.Users = _unitOfWork.ApplicationUser.GetAll().Count();
            ViewBag.Products = _unitOfWork.product.GetAll().Count();

            return View();
        }
    }
}
