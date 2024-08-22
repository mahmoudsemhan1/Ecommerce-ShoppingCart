using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Myshop.Utilities;
using MyShop.DataAcess;
using MyShop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        public readonly UserManager<ApplicationUser> _userManager;
        public readonly RoleManager<IdentityRole> _roleManager;
        public readonly ApplicationDbContext _context;


        public DbInitializer(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }


        public void Initialize()
        {
            //migrations
            try
            {
                if (_context.Database.GetPendingMigrations().Count()>0) 
                {
                    _context.Database.Migrate();
                }

            }
            catch(Exception ) 
            {
                throw;
            }

            //Roles
            if (!_roleManager.RoleExistsAsync(SD.AdminRole).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.AdminRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.EditorRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.CustomerRole)).GetAwaiter().GetResult();

                //User

                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "Admin@myshop.com",
                    Email = "Admin@myshop.com",
                    Name = "Adminsitreator",
                    PhoneNumber = "123456",
                    Address = "Cairo",
                    City = "cairo",
                },"Mahmoudsemhan123456#").GetAwaiter().GetResult() ;

                ApplicationUser user= _context.applicationUsers.FirstOrDefault(u=>u.Email== "Admin@myshop.com");

                _userManager.AddToRoleAsync(user, SD.AdminRole).GetAwaiter().GetResult();
            }

            return;
        }
    }
}
