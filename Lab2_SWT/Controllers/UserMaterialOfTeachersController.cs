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
    public class UserMaterialOfTeachersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _Accessor;
        public UserMaterialOfTeachersController(AppDbContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _Accessor = accessor;
        }

        // GET: MaterialOfTeachers
        public async Task<IActionResult> Index(string status, string sub_code)
        {
            string userName = _Accessor.HttpContext.Session.GetString("Username");
            var list = _context.MaterialOfTeacher.Include(m => m.Account).Include(m => m.Subject).Where(p => p.TeacherUsername.Equals(userName)).ToList();
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

        // GET: MaterialOfTeachers/Create
        public IActionResult Create()
        {
            ViewData["TeacherUsername"] = new SelectList(_context.Account, "UserName", "UserName");
            ViewData["SubjectCode"] = new SelectList(_context.Subject, "SubjectCode", "SubjectCode");
            return View();
        }

        // POST: MaterialOfTeachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,TeacherUsername,SubjectCode,URL,Status")] MaterialOfTeacher materialOfTeacher)
        {
            Subject sj = _context.Subject.Include(p => p.Status).FirstOrDefault(pro => pro.SubjectCode.Equals(materialOfTeacher.SubjectCode.Trim()));
            if (sj != null)
            {
                    _context.Add(materialOfTeacher);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "Please enter valid subject code !";
            return View(materialOfTeacher);
        }

        // GET: MaterialOfTeachers/Edit/5
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
            return View(materialOfTeacher);
        }

        // POST: MaterialOfTeachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,TeacherUsername,SubjectCode,URL,Status")] MaterialOfTeacher materialOfTeacher)
        {
            if (id != materialOfTeacher.Id)
            {
                return NotFound();
            }
            Subject sj = _context.Subject.Include(p => p.Status).FirstOrDefault(pro => pro.SubjectCode.Equals(materialOfTeacher.SubjectCode.Trim()));
            if (sj != null)
            {
                    try
                    {
                        materialOfTeacher.Status = "waiting";
                        _context.Update(materialOfTeacher);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!MaterialOfTeacherExists(materialOfTeacher.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index)); 
            }
            TempData["error"] = "Please enter valid subject code !";
            return View(materialOfTeacher);
        }

        // GET: MaterialOfTeachers/Delete/5
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

        // POST: MaterialOfTeachers/Delete/5
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
