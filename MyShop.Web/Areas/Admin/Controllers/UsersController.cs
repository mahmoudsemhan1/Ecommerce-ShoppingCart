using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Myshop.Utilities;
using MyShop.DataAcess;
using System.Security.Claims;

namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbConext _conext;

        public UsersController(ApplicationDbConext conext)
        {
            _conext = conext;
        }
        [Authorize("AdminRole")]
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

    }
}
