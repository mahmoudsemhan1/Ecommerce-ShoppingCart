using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Myshop.Utilities;
using MyShop.DataAcess;
using System.Security.Claims;

namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class UsersController : Controller
    {
        private readonly ApplicationDbConext _conext;

        public UsersController(ApplicationDbConext conext)
        {
            _conext = conext;
        }

        public IActionResult Index()
        {
            var claimsidentity = (ClaimsIdentity)User.Identity;
            var claim=claimsidentity.FindFirst(ClaimTypes.NameIdentifier);

            string userid = claim.Value;
            return View(_conext.applicationUsers.Where(x=>x.Id!=userid).ToList());
        }
    }
}
