﻿using System.ComponentModel.DataAnnotations;

namespace MyShop.Entities.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Product> Products { get; set; }
        public DateTime CreatedTime { get; set; }=DateTime.Now;

    }
}
