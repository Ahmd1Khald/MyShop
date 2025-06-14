using Microsoft.AspNetCore.Mvc;
using MyShop.Web.Date;
using MyShop.Web.Models;

namespace MyShop.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDBContext _dbContext;

        // Constructor injection
        public CategoryController(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region CURD Operations

        #region Create
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
                _dbContext.Categories.Add(category);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }
        #endregion

        #region Read
        public IActionResult Index()
        {
            List<Category> category = _dbContext.Categories.ToList();
            return View(category);
        }
        #endregion

        #region Update
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _dbContext.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Categories.Update(category);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }
        #endregion

        #region Delete
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var category = _dbContext.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public IActionResult Delete(Category category)
        {
            _dbContext.Categories.Remove(category);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion

        #endregion






    }
}
