using Microsoft.AspNetCore.Mvc;
using MyShop.Entities.Models;
using MyShop.DataAccess;
using MyShop.Entities.Repositories;
using MyShop.Entities.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;


namespace MyShop.Web.Controllers
{
    public class ProductController : Controller
    {
        //private readonly ApplicationDBContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        // Constructor injection
        public ProductController(/*ApplicationDBContext dbContext*/ IUnitOfWork unitOfWork,IWebHostEnvironment webHostEnvironment)
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
                CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem{
                
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
                    using (var filestream = new FileStream(Path.Combine(Uploud,fileName+ext),FileMode.Create))
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
            var product = _unitOfWork.Product.GetAll();
            return View(product);
        }
        #endregion

        #region Update
        [HttpGet]
        public IActionResult Edit(int id)
        {
            //var product = _dbContext.Products.FirstOrDefault(c => c.Id == id);
            var product = _unitOfWork.Product.GetFirstOrDefualt(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                //_dbContext.Products.Update(product);
                //_dbContext.SaveChanges();
                _unitOfWork.Product.update(product);
                _unitOfWork.Complete();

                TempData["Updated"] = "Item was Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(product);
        }
        #endregion

        #region Delete
        [HttpGet]
        public IActionResult Delete(int id)
        {
            //var product = _dbContext.Products.FirstOrDefault(c => c.Id == id);
            var product = _unitOfWork.Product.GetFirstOrDefualt(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public IActionResult Delete(Product product)
        {
            //_dbContext.Products.Remove(product);
            //_dbContext.SaveChanges();

            _unitOfWork.Product.Remove(product);
            _unitOfWork.Complete();

            TempData["Deleted"] = "Item was Deleted Successfully";
            return RedirectToAction("Index");
        }
        #endregion

        #endregion






    }
}
