using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Entities.Models.ViewModels
{
    public class ShoppingCart

    {
        public Product product { get; set; }
        [Range(1,100,ErrorMessage ="You Must enter value between 1 to 100")]
        public int  Count { get; set; }
    }
}
