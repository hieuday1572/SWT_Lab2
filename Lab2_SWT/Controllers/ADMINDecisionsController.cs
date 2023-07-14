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
    public class ADMINDecisionsController : Controller
    {
        private readonly AppDbContext _context;

        public ADMINDecisionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ADMINDecisions
        public async Task<IActionResult> Index()
        {
              return _context.Decision != null ? 
                          View(await _context.Decision.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.Decision'  is null.");
        }

        // GET: ADMINDecisions/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Decision == null)
            {
                return NotFound();
            }

            var decision = await _context.Decision
                .FirstOrDefaultAsync(m => m.DecisionNo.Equals(id));
            if (decision == null)
            {
                return NotFound();
            }

            return View(decision);
        }

        // GET: ADMINDecisions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ADMINDecisions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DecisionNo,DecisionName,Note,CreateDate")] Decision decision)
        {
            var check = _context.Decision.FirstOrDefault(p=>p.DecisionNo.Equals(decision.DecisionNo));
            if (check == null)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(decision);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            TempData["error"] = "Wrong: Decision is already exist !";
            return View(decision);
        }

        // GET: ADMINDecisions/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Decision == null)
            {
                return NotFound();
            }

            var decision = await _context.Decision.FindAsync(id);
            if (decision == null)
            {
                return NotFound();
            }
            return View(decision);
        }

        // POST: ADMINDecisions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("DecisionNo,DecisionName,Note,CreateDate")] Decision decision)
        {
            if (id != decision.DecisionNo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(decision);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DecisionExists(decision.DecisionNo))
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
            return View(decision);
        }

        // GET: ADMINDecisions/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Decision == null)
            {
                return NotFound();
            }

            var decision = await _context.Decision
                .FirstOrDefaultAsync(m => m.DecisionNo.Equals(id));
            if (decision == null)
            {
                return NotFound();
            }

            return View(decision);
        }

        // POST: ADMINDecisions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Decision == null)
            {
                return Problem("Entity set 'AppDbContext.Decision'  is null.");
            }
            var decision = await _context.Decision.FindAsync(id);
            if (decision != null)
            {
                var curri = _context.Curriculum.Where(p=>p.DecisionNo.Equals(id)).ToList();
                var syllabus = _context.Syllabus.Where(p => p.DecisionNo.Equals(id)).ToList();
                foreach(var cur in curri)
                {
                    cur.DecisionNo = null;
                    _context.Curriculum.Update(cur);
                }
                foreach (var syl in syllabus)
                {
                    syl.DecisionNo = null;
                    _context.Syllabus.Update(syl);
                }
                _context.Decision.Remove(decision);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DecisionExists(string id)
        {
          return (_context.Decision?.Any(e => e.DecisionNo == id)).GetValueOrDefault();
        }
    }
}
