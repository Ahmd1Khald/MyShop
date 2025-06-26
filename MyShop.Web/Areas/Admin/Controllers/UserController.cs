using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.DataAccess;
using MyShop.Utilities;
using System.Security.Claims;

namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDBContext _dBContext;

        public UserController(ApplicationDBContext dBContext) 
        {
            _dBContext = dBContext;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userId = claim.Value;
            return View(_dBContext.ApplicationUsers.Where(u=>u.Id != userId).ToList());
        }

        public IActionResult LockUnLock(string? id)
        {
            var user = _dBContext.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            if (user.LockoutEnd == null || user.LockoutEnd < DateTime.Now)
            {
                user.LockoutEnd = DateTime.Now.AddDays(1);
            }
            else 
            {
                user.LockoutEnd = DateTime.Now;
            }
            _dBContext.SaveChanges();

            return RedirectToAction("Index","User",new { area = "Admin"});
        }
    }
}
