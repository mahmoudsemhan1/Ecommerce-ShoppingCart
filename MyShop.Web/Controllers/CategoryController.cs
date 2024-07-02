using Microsoft.AspNetCore.Mvc;
using MyShop.Web.Data;
using MyShop.Web.Models;

namespace MyShop.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbConext _context;

        public CategoryController(ApplicationDbConext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
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
            if (ModelState.IsValid)
            {

                _context.Categories.Add(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null) NotFound();
            var category = _context.Categories.Find(id);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {

                _context.Categories.Update(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }


    }
}
