using Microsoft.EntityFrameworkCore;
using MyShop.DataAcess;
using MyShop.Entities.Models;
using MyShop.Entities.Repositiories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.Implementation
{
    public class ApplicationUserRepository : GenericRepositiory<ApplicationUser>,IApplicationUserRepository
    {
        private readonly ApplicationDbConext _context;
        public ApplicationUserRepository(ApplicationDbConext context): base(context)
        {
            _context=context;
            
        }

       
    }
}
