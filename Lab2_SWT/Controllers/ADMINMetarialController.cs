using LMMProject.Data;
using LMMProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LMMProject.Controllers
{
    public class ADMINMetarialController : Controller
    {
        private readonly AppDbContext _context;
        public ADMINMetarialController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var materials = await _context.Material.Include(s => s.Subject).OrderByDescending(s => s.MaterialId).ToListAsync();
            return View(materials);
        }

        public async Task<IActionResult> Create()
        {
            var subjects = await _context.Subject.Include(s => s.Status).ToListAsync();

            return View(subjects);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitCreate(Material model)
        {
            _context.Material.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var material = await _context.Material.FirstOrDefaultAsync(m => m.MaterialId == id);

            if (material == null)
            {
                return NotFound();
            }

            var subjects = await _context.Subject.Include(s => s.Status).ToListAsync();

            return View(new MaterialFilterModel()
            {
                Material = material,
                Subjects = subjects
            });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var material = await _context.Material.FirstOrDefaultAsync(m => m.MaterialId == id);

            if (material == null)
            {
                return NotFound();
            }

            _context.Material.Remove(material);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> SubmitUpdate(Material model)
        {
            var material = await _context.Material.FirstOrDefaultAsync(m => m.MaterialId == model.MaterialId);

            if (material == null)
            {
                return NotFound();
            }

            material.MaterialDescription = model.MaterialDescription;
            material.Author = model.Author;
            material.Publisher = model.Publisher;
            material.PublishedDate = model.PublishedDate;
            material.Url = model.Url;
            material.SubjectCode = model.SubjectCode;

            _context.Material.Update(material);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
