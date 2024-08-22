using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Myshop.Utilities;
using MyShop.DataAcess;
using MyShop.Entities.Models;
using MyShop.Entities.Repositiories;



namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.AdminRole)]

    public class CategoryController : Controller
    {

        private IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var categories = _unitOfWork.category.GetAll();
            return View(categories);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
            {

                //_context.Categories.Add(category);
                _unitOfWork.category.Add(category);
                //_context.SaveChanges();
                _unitOfWork.Complete();

                TempData["Create"] = "Data has Created Succesfully";

                return RedirectToAction("Index");
            }
            return View(category);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null) NotFound();
            //var category = _context.Categories.Find(id);
            var category = _unitOfWork.category.GetFirstOrDefualt(x => x.Id == id);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (!ModelState.IsValid)
            {

                // _context.Categories.Update(category);
                _unitOfWork.category.update(category);
                // _context.SaveChanges();
                _unitOfWork.Complete();
                TempData["Update"] = "Data has Updated Succesfully";

                return RedirectToAction("Index");
            }
            return View(category);
        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null) NotFound();
            //_context.Categories.Find(id)

            var category = _unitOfWork.category.GetFirstOrDefualt(x => x.Id == id);


            return View(category);
        }
        [HttpPost]
        public IActionResult DeleteCategory(int? id)
        {
            var category = _unitOfWork.category.GetFirstOrDefualt(x => x.Id == id);

            if (category == null) NotFound();

            //_context.Categories.Remove(category);
            _unitOfWork.category.Remove(category);
            // _context.SaveChanges();
            _unitOfWork.Complete();
            TempData["Delete"] = "Data has deleted Succesfully";

            return RedirectToAction("Index");

        }


    }
}
