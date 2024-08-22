using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Myshop.Utilities;
using MyShop.Entities.Models;
using MyShop.Entities.Models.ViewModels;
using MyShop.Entities.Repositiories;
using System.IO;



namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.AdminRole)]

    public class ProductController : Controller
    {

        private IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            //ProductCategoryViewModel make it for product include category
            var listofcat = _unitOfWork.category.GetAllCategories().ToList();
            var products = _unitOfWork.product.GetAll();

            var viewModel = new ProductCategoryViewModel
            {
                Products = products,
                Categories = listofcat
            };

            return View(viewModel);

        }
        [HttpGet]
        //public IActionResult GetData()
        //{
        //    var listofcat = _unitOfWork.category.GetAllCategories().ToList();
        //    var products = _unitOfWork.product.GetAll();

        //    var viewModel = new ProductCategoryViewModel
        //    {
        //        Products = products,
        //        Categories = listofcat
        //    };
        //    return Json(viewModel);
        //}
        [HttpGet]
        public IActionResult Create()
        {
            ProductVm productvm = new ProductVm()
            {
                Product = new Product(),
                CategoryList = _unitOfWork.category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return View(productvm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductVm ProductVM, IFormFile file)
        {


            if (ModelState.IsValid)
            {
                //file part for img
                string RootPath = _webHostEnvironment.WebRootPath;
                if (RootPath != null)
                {
                    string filename = Guid.NewGuid().ToString();
                    var uploadPath = Path.Combine(RootPath, "Images", "Products");
                    var extension = Path.GetExtension(file.FileName);

                    // Ensure the directory exists
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    using (var filestream = new FileStream(Path.Combine(uploadPath, filename + extension), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    //database part
                    ProductVM.Product.Img = @"Images/Products/" + filename + extension;
                }
                //_context.Categories.Add(Product);
                _unitOfWork.product.Add(ProductVM.Product);
                //_context.SaveChanges();
                _unitOfWork.Complete();

                TempData["Create"] = "Data has Created Succesfully";

                return RedirectToAction("Index");
            }
            return View(ProductVM.Product);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null) NotFound();
            ProductVm productvm = new ProductVm()
            {
                Product = _unitOfWork.product.GetFirstOrDefualt(x => x.Id == id),
                CategoryList = _unitOfWork.category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return View(productvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductVm Productvm , IFormFile file)
        {
            if (ModelState.IsValid)
            {
                //file part for img
                string RootPath = _webHostEnvironment.WebRootPath;
                if (RootPath != null)
                {   
                    string filename = Guid.NewGuid().ToString();
                    var uploadPath = Path.Combine(RootPath, "Images", "Products");
                    var extension = Path.GetExtension(file.FileName);

                    if (Productvm.Product.Img != null)
                    {
                        var oldimg=Path.Combine(RootPath,Productvm.Product.Img.TrimStart('\\'));
                        if (System.IO.File.Exists(oldimg))
                        {
                            System.IO.File.Delete(oldimg);

                        }
                    }
                    // Ensure the directory exists
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    using (var filestream = new FileStream(Path.Combine(uploadPath, filename + extension), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    //database part
                    Productvm.Product.Img = @"Images/Products/" + filename + extension;

                }

                // _context.Categories.Update(Product);
                _unitOfWork.product.update(Productvm.Product);
                // _context.SaveChanges();
                _unitOfWork.Complete();
                TempData["Update"] = "Data has Updated Succesfully";

                return RedirectToAction("Index");
            }
     
            return View(Productvm.Product);
        }
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null) NotFound();
        //    //var Product = _context.Categories.Find(id);

        //    ProductVm productvm = new ProductVm()
        //    {
        //        Product = _unitOfWork.product.GetFirstOrDefualt(x => x.Id == id),
        //        CategoryList = _unitOfWork.category.GetAll().Select(x => new SelectListItem
        //        {
        //            Text = x.Name,
        //            Value = x.Id.ToString()
        //        })
        //    };
        //    return View(productvm);

        //}

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return Json(new { success = false, message = "Invalid product ID." });
            }

            var product = _unitOfWork.product.GetFirstOrDefualt(x => x.Id == id);
            if (product == null)
            {
                return Json(new { success = false, message = "Error while deleting. Product not found." });
            }

            _unitOfWork.product.Remove(product);

            var oldimg = Path.Combine(_webHostEnvironment.WebRootPath, product.Img.TrimStart('\\'));
            if (System.IO.File.Exists(oldimg))
            {
                System.IO.File.Delete(oldimg);
            }

            _unitOfWork.Complete();

            return Json(new { success = true, message = "Product has been deleted successfully." });
        }



    }
}
