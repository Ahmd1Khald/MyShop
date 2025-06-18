using Microsoft.AspNetCore.Mvc;
using MyShop.Entities.Models;
using MyShop.DataAccess;
using MyShop.Entities.Repositories;


namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        //private readonly ApplicationDBContext _dbContext;
        private IUnitOfWork _unitOfWork;

        // Constructor injection
        public CategoryController(/*ApplicationDBContext dbContext*/ IUnitOfWork unitOfWork)
        {
            //_dbContext = dbContext;
            _unitOfWork = unitOfWork;
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
                //_dbContext.Categories.Add(category);
                //_dbContext.SaveChanges();
                _unitOfWork.Category.Add(category);
                _unitOfWork.Complete();

                TempData["Create"] = "Item was Created Successfully";
                return RedirectToAction("Index");
            }
            return View(category);
        }
        #endregion

        #region Read
        public IActionResult Index()
        {
            //List<Category> category = _dbContext.Categories.ToList();
            var category = _unitOfWork.Category.GetAll();
            return View(category);
        }
        #endregion

        #region Update
        [HttpGet]
        public IActionResult Edit(int id)
        {
            //var category = _dbContext.Categories.FirstOrDefault(c => c.Id == id);
            var category = _unitOfWork.Category.GetFirstOrDefualt(c => c.Id == id);
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
                //_dbContext.Categories.Update(category);
                //_dbContext.SaveChanges();
                _unitOfWork.Category.update(category);
                _unitOfWork.Complete();

                TempData["Updated"] = "Item was Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(category);
        }
        #endregion

        #region Delete
        [HttpGet]
        public IActionResult Delete(int id)
        {
            //var category = _dbContext.Categories.FirstOrDefault(c => c.Id == id);
            var category = _unitOfWork.Category.GetFirstOrDefualt(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public IActionResult Delete(Category category)
        {
            //_dbContext.Categories.Remove(category);
            //_dbContext.SaveChanges();

            _unitOfWork.Category.Remove(category);
            _unitOfWork.Complete();

            TempData["Deleted"] = "Item was Deleted Successfully";
            return RedirectToAction("Index");
        }
        #endregion

        #endregion






    }
}
