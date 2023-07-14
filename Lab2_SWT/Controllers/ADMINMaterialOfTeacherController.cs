using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LMMProject.Data;
using LMMProject.Models;

namespace LMMProject.Controllers
{
    public class ADMINMaterialOfTeacherController : Controller
    {
        private int count_Index;
        private readonly AppDbContext _context;

        public ADMINMaterialOfTeacherController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ADMINMaterialOfTeacher
        public async Task<IActionResult> Index(string status, string sub_code)
        {
            
            List<MaterialOfTeacher> list =  await _context.MaterialOfTeacher.Include(m => m.Account).Include(m => m.Subject).ToListAsync();
            
                if (!string.IsNullOrEmpty(status))
                {
                if (!status.Equals("") && !status.Equals("all"))
                {
                    if (!string.IsNullOrEmpty(sub_code))
                    {
                        list = await _context.MaterialOfTeacher.Include(m => m.Account).Include(m => m.Subject).Where(m => m.Status.Equals(status)).Where(m => m.SubjectCode.Contains(sub_code)).ToListAsync();
                    }
                    else
                    {
                        list = await _context.MaterialOfTeacher.Include(m => m.Account).Include(m => m.Subject).Where(m => m.Status.Equals(status)).ToListAsync();
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(sub_code))
                    {
                        list = await _context.MaterialOfTeacher.Include(m => m.Account).Include(m => m.Subject).Where(m => m.SubjectCode.Contains(sub_code)).ToListAsync();
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(sub_code))
                {
                    list = await _context.MaterialOfTeacher.Include(m => m.Account).Include(m => m.Subject).Where(m => m.SubjectCode.Contains(sub_code)).ToListAsync();
                }
            }
            return View(list);
        }

        // GET: ADMINMaterialOfTeacher/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MaterialOfTeacher == null)
            {
                return NotFound();
            }

            var materialOfTeacher = await _context.MaterialOfTeacher.FindAsync(id);
            if (materialOfTeacher == null)
            {
                return NotFound();
            }
            materialOfTeacher.Status = "approved";
            _context.Update(materialOfTeacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: ADMINMaterialOfTeacher/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MaterialOfTeacher == null)
            {
                return NotFound();
            }

            var materialOfTeacher = await _context.MaterialOfTeacher
                .Include(m => m.Account)
                .Include(m => m.Subject)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (materialOfTeacher == null)
            {
                return NotFound();
            }

            return View(materialOfTeacher);
        }

        // POST: ADMINMaterialOfTeacher/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.MaterialOfTeacher == null)
            {
                return Problem("Entity set 'AppDbContext.MaterialOfTeacher'  is null.");
            }
            var materialOfTeacher = await _context.MaterialOfTeacher.FindAsync(id);
            if (materialOfTeacher != null)
            {
                _context.MaterialOfTeacher.Remove(materialOfTeacher);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MaterialOfTeacherExists(int id)
        {
          return (_context.MaterialOfTeacher?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
