using Microsoft.AspNetCore.Mvc;
using MyShop.Entities.Repositories;
using MyShop.Entities.ViewModels;

namespace MyShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        //private readonly ApplicationDBContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var products = _unitOfWork.Product.GetAll();
            return View(products);
        }

        public IActionResult ProductDetails(int id)
        {
            var product = _unitOfWork.Product.GetFirstOrDefualt(x=>x.Id == id,includeWord:"Category");
            ShoppingCart shoppingCart = new ShoppingCart()
            {
                Product = product,
                Count = 1,
            };
            return View(shoppingCart);
        }
    }
}
