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
    public class ADMINComboSubjectsController : Controller
    {
        private readonly AppDbContext _context;

        public ADMINComboSubjectsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ADMINComboSubjects
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Combo_Subject.Include(c => c.Combo).Include(c => c.Subject);
            return View(await appDbContext.ToListAsync());
        }

        // GET: ADMINComboSubjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Combo_Subject == null)
            {
                return NotFound();
            }

            var comboSubject = await _context.Combo_Subject
                .Include(c => c.Combo)
                .Include(c => c.Subject)
                .FirstOrDefaultAsync(m => m.id == id);
            if (comboSubject == null)
            {
                return NotFound();
            }

            return View(comboSubject);
        }

        // GET: ADMINComboSubjects/Create
        public IActionResult Create()
        {
            ViewData["ComboId"] = new SelectList(_context.Combo, "ComboId", "ComboId");
            ViewData["SubjectCode"] = new SelectList(_context.Subject, "SubjectCode", "SubjectCode");
            return View();
        }

        // POST: ADMINComboSubjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,ComboId,SubjectCode")] ComboSubject comboSubject)
        {
            if (ModelState.IsValid)
            {
                _context.Add(comboSubject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ComboId"] = new SelectList(_context.Combo, "ComboId", "ComboId", comboSubject.ComboId);
            ViewData["SubjectCode"] = new SelectList(_context.Subject, "SubjectCode", "SubjectCode", comboSubject.SubjectCode);
            return View(comboSubject);
        }

        // GET: ADMINComboSubjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Combo_Subject == null)
            {
                return NotFound();
            }

            var comboSubject = await _context.Combo_Subject.FindAsync(id);
            if (comboSubject == null)
            {
                return NotFound();
            }
            ViewData["ComboId"] = new SelectList(_context.Combo, "ComboId", "ComboId", comboSubject.ComboId);
            ViewData["SubjectCode"] = new SelectList(_context.Subject, "SubjectCode", "SubjectCode", comboSubject.SubjectCode);
            return View(comboSubject);
        }

        // POST: ADMINComboSubjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,ComboId,SubjectCode")] ComboSubject comboSubject)
        {
            if (id != comboSubject.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comboSubject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComboSubjectExists(comboSubject.id))
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
            ViewData["ComboId"] = new SelectList(_context.Combo, "ComboId", "ComboId", comboSubject.ComboId);
            ViewData["SubjectCode"] = new SelectList(_context.Subject, "SubjectCode", "SubjectCode", comboSubject.SubjectCode);
            return View(comboSubject);
        }

        // GET: ADMINComboSubjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Combo_Subject == null)
            {
                return NotFound();
            }

            var comboSubject = await _context.Combo_Subject
                .Include(c => c.Combo)
                .Include(c => c.Subject)
                .FirstOrDefaultAsync(m => m.id == id);
            if (comboSubject == null)
            {
                return NotFound();
            }

            return View(comboSubject);
        }

        // POST: ADMINComboSubjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Combo_Subject == null)
            {
                return Problem("Entity set 'AppDbContext.Combo_Subject'  is null.");
            }
            var comboSubject = await _context.Combo_Subject.FindAsync(id);
            if (comboSubject != null)
            {
                _context.Combo_Subject.Remove(comboSubject);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComboSubjectExists(int id)
        {
          return (_context.Combo_Subject?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
