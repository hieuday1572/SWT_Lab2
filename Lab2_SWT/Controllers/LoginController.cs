using LMMProject.Data;
using LMMProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI;

namespace LMMProject.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;       

        public LoginController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var account = new Account();
            return View(account);
        }
        [HttpPost]
        public async Task<IActionResult> Index(Account account)
        {
            if (!ModelState.IsValid)
            {
                return View(account);
            }
            Account user = await _context.Account.Include(p => p.Role).FirstOrDefaultAsync(pro => pro.UserName.Equals(account.UserName));
            if (user != null)
            {
                if (user.Password.Equals(account.Password))      
                {
                    HttpContext.Session.SetString("Username", user.UserName);
                    HttpContext.Session.SetInt32("role", (int)user.RoleId);
                    
                    if (user.RoleId == 1)
                    {
                        return RedirectToAction("Dashboard", "ADMIN");
                    }
                    else
                    {
                        return RedirectToAction("Index", "UserHome");
                    }
                    
                }
                TempData["Error"] = "Wrong: please try again!";
                return View(account);
            }
            TempData["Error"] = "Wrong: please try again!";
            return View(account);

        }
    }
}
