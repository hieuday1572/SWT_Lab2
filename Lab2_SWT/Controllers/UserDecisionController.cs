using LMMProject.Data;
using Microsoft.AspNetCore.Mvc;

namespace LMMProject.Controllers
{
    public class UserDecisionController : Controller
    {
        private readonly AppDbContext _context;
        public UserDecisionController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(string id)
        {
            var decision = _context.Decision.Where(p=>p.DecisionNo.Equals(id)).FirstOrDefault();
            return View(decision);
        }
    }
}
