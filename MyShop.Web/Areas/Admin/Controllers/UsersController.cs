using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Myshop.Utilities;
using MyShop.DataAcess;
using MyShop.Entities.Models;
using MyShop.Entities.Repositiories;
using System.Security.Claims;

namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("AdminRole")]

    public class UsersController : Controller
    {
        private readonly ApplicationDbConext _conext;
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(ApplicationDbConext conext, IUnitOfWork unitOfWork)
        {
            _conext = conext;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var claimsidentity = (ClaimsIdentity)User.Identity;
            var claim=claimsidentity.FindFirst(ClaimTypes.NameIdentifier);

            string userid = claim.Value;
            return View(_conext.applicationUsers.Where(x=>x.Id!=userid).ToList());
        }

        public IActionResult LockUnlock(string? id)
        {
            var user = _conext.applicationUsers.FirstOrDefault(x => x.Id == id);
            if (user == null) return NotFound();

            if(user.LockoutEnd==null || user.LockoutEnd < DateTime.Now)
            {
                user.LockoutEnd = DateTime.Now.AddYears(1);
            }
            else
            {
                user.LockoutEnd = DateTime.Now;
            }
            _conext.SaveChanges();
            return RedirectToAction("Index", "Users", new {area="Admin"});

        }

        public IActionResult Delete(string id)
        {
            var user=_conext.applicationUsers.FirstOrDefault(u=>u.Id==id);
            if(user == null) return NotFound();

            _conext.Remove(user);
            _unitOfWork.Complete();
            return Json(new { success = true, message = "Product has been deleted successfully." });

        }



    }
}
