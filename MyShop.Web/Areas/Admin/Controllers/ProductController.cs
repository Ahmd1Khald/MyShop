﻿using Microsoft.AspNetCore.Mvc;
using MyShop.Entities.Models;
using MyShop.DataAccess;
using MyShop.Entities.Repositories;
using MyShop.Entities.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;


namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        //private readonly ApplicationDBContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        // Constructor injection
        public ProductController(/*ApplicationDBContext dbContext*/ IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            //_dbContext = dbContext;
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        #region CURD Operations

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            ProductVM productVM = new ProductVM()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
                {

                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductVM productVM, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                //WWWRoot path
                string RootPath = _webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    // Generate random number
                    string fileName = Guid.NewGuid().ToString();
                    // Path
                    var Uploud = Path.Combine(RootPath, @"Images\Products");
                    // extention EX:jpg
                    var ext = Path.GetExtension(file.FileName);
                    // Upload file with random name and extension
                    using (var filestream = new FileStream(Path.Combine(Uploud, fileName + ext), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }

                    productVM.Product.Image = @"Images\Products\" + fileName + ext;
                }

                //_dbContext.Products.Add(product);
                //_dbContext.SaveChanges();
                _unitOfWork.Product.Add(productVM.Product);
                _unitOfWork.Complete();

                TempData["Create"] = "Item was Created Successfully";
                return RedirectToAction("Index");
            }
            return View(productVM.Product);
        }
        #endregion

        #region Read
        public IActionResult Index()
        {
            //List<Product> product = _dbContext.Products.ToList();
            //var product = _unitOfWork.Product.GetAll();
            return View();
        }

        [HttpGet]
        public IActionResult GetData()
        {
            var categories = _unitOfWork.Product.GetAll(includeWord: "Category");
            return Json(new { data = categories });
        }
        #endregion

        #region Update
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            //var product = _dbContext.Products.FirstOrDefault(c => c.Id == id);
            var product = _unitOfWork.Product.GetFirstOrDefualt(c => c.Id == id);

            ProductVM productVM = new ProductVM()
            {
                Product = product,
                CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
                {

                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            return View(productVM);
        }

        [HttpPost]
        public IActionResult Edit(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                //WWWRoot path
                string RootPath = _webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    // Generate random number
                    string fileName = Guid.NewGuid().ToString();
                    // Path
                    var Uploud = Path.Combine(RootPath, @"Images\Products");
                    // extention EX:jpg
                    var ext = Path.GetExtension(file.FileName);

                    if (productVM.Product.Image != null)
                    {
                        // Remove the old image from resources
                        var oldImage = Path.Combine(RootPath, productVM.Product.Image.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImage))
                        {
                            System.IO.File.Delete(oldImage);
                        }

                    }
                    // Upload file with random name and extension
                    using (var filestream = new FileStream(Path.Combine(Uploud, fileName + ext), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    productVM.Product.Image = @"Images\Products\" + fileName + ext;
                }
                //_dbContext.Products.Update(product);
                //_dbContext.SaveChanges();
                _unitOfWork.Product.update(productVM.Product);
                _unitOfWork.Complete();

                TempData["Updated"] = "Item was Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(productVM.Product);
        }
        #endregion

        #region Delete

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            //_dbContext.Products.Remove(product);
            //_dbContext.SaveChanges();

            var productInDb = _unitOfWork.Product.GetFirstOrDefualt(x => x.Id == id);
            if (productInDb == null)
            {
                return Json(new { success = false, message = "Error While Deleting" });
            }

            _unitOfWork.Product.Remove(productInDb);
            // Remove the old image from resources
            var oldImage = Path.Combine(_webHostEnvironment.WebRootPath, productInDb.Image.TrimStart('\\'));
            if (System.IO.File.Exists(oldImage))
            {
                System.IO.File.Delete(oldImage);
            }


            _unitOfWork.Complete();
            return Json(new { success = true, message = "Product has been Deleted" });
        }
        #endregion

        #endregion






    }
}
