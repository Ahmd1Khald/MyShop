using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.Entities.Models;
using MyShop.Entities.Repositories;
using MyShop.Entities.ViewModels;
using System.Security.Claims;

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

        //[HttpGet("{ProductId:int}")]
        public IActionResult ProductDetails(int ProductId)
        {
            ShoppingCart shoppingCart = new ShoppingCart()
            {
                ProductId = ProductId,
                Product = _unitOfWork.Product.GetFirstOrDefualt(x => x.Id == ProductId, includeWord: "Category"),
                Count = 1,
            };
            return View(shoppingCart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult ProductDetails(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.ApplicationUserId = claim.Value;

            _unitOfWork.ShoppingCart.Add(shoppingCart);
            _unitOfWork.Complete();

            return RedirectToAction("Index");
        }
    }
}
