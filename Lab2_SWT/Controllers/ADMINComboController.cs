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
    public class ADMINComboController : Controller
    {
        private readonly AppDbContext _context;

        public ADMINComboController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ADMINCombo
        public async Task<IActionResult> Index(int? id)
        {
            Curriculum curri = _context.Curriculum.FirstOrDefault(p => p.CurriculumId == id);
            ViewBag.Curriculum = curri;
            var listCombo = _context.Combo.Include(p => p.Curriculum).Where(pro => pro.CurriculumId == id).ToList();
            return View(listCombo);  
        }

        // GET: ADMINCombo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            Combo combsub = _context.Combo.FirstOrDefault(p => p.ComboId == id);
            ViewBag.Combosub = combsub;
            var listSubject = _context.Combo_Subject.Include(a => a.Subject).Include(b => b.Combo).Where(pro => pro.ComboId == id).ToList();
            return View(listSubject);
        }

        // GET: ADMINCombo/Create
        public IActionResult Create(string id)
        {
            ViewData["CurriculumId"] = id;
            return View();
        }

        // POST: ADMINCombo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ComboId,ComboNameVn,ComboNameEn,Note,Tag,CurriculumId")] Combo combo)
        {
            if (ModelState.IsValid)
            {
                Combo checkId = _context.Combo.Include(p => p.Curriculum).FirstOrDefault(p => p.ComboNameEn.Equals(combo.ComboNameEn.Trim()));
                if (checkId == null)
                {
                    _context.Add(combo);
                    await _context.SaveChangesAsync();
                    //return RedirectToAction(nameof(Index));
                    return new RedirectResult(url: "/ADMINCombo/Index/" + combo.CurriculumId, permanent: true, preserveMethod: true);
                }
                else
                {
                    TempData["Error"] = "Wrong: Curriculum is already exist !";
                }
            }
            ViewData["CurriculumId"] = combo.CurriculumId;
            return View(combo);
        }

        // GET: ADMINCombo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Combo == null)
            {
                return NotFound();
            }

            var combo = await _context.Combo.FindAsync(id);
            if (combo == null)
            {
                return NotFound();
            }
            ViewData["CurriculumId"] = combo.CurriculumId;
            return View(combo);
        }

        // POST: ADMINCombo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( [Bind("ComboId,ComboNameVn,ComboNameEn,Note,Tag,CurriculumId")] Combo combo)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(combo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComboExists(combo.ComboId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return new RedirectResult(url: "/ADMINCombo/Index/" + combo.CurriculumId, permanent: true, preserveMethod: true);
            }
            ViewData["CurriculumId"] = new SelectList(_context.Curriculum, "CurriculumId", "CurriculumCode", combo.CurriculumId);
            return View(combo);
        }

        // GET: ADMINCombo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Combo == null)
            {
                return NotFound();
            }
            var combo = await _context.Combo
                .Include(c => c.Curriculum)
                .FirstOrDefaultAsync(m => m.ComboId == id);
            if (combo == null)
            {
                return NotFound();
            }

            return View(combo);
        }

        // POST: ADMINCombo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Combo == null)
            {
                return Problem("Entity set 'AppDbContext.Combo'  is null.");
            }
            var combo = await _context.Combo.FindAsync(id);
            var comboS = _context.Combo_Subject.Where(p => p.ComboId == id).ToList();
             _context.Combo_Subject.RemoveRange(comboS);
            if (combo != null)
            {
                _context.Combo.Remove(combo);
                await _context.SaveChangesAsync();
            }
            return new RedirectResult(url: "/ADMINCombo/Index/" + combo.CurriculumId, permanent: true, preserveMethod: true);
        }
        public IActionResult AddSubject()
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
        public async Task<IActionResult> AddSubject([Bind("id,ComboId,SubjectCode")] ComboSubject comboSubject)
        {
            string add = Request.Form["add"];
            int comboId = Convert.ToInt32(Request.Form["comboId"]);
            if (add != null && !add.Trim().Equals(""))
            {
                Subject sj = _context.Subject.Include(p => p.Status).FirstOrDefault(pro => pro.SubjectCode.Equals(add.Trim()));
                if (sj != null)
                {

                    var check = _context.Combo_Subject.Where(p => p.ComboId == comboId).ToList();
                    ComboSubject check_element = (from element in check
                                                       where element.SubjectCode.Trim().Equals(sj.SubjectCode.Trim())
                                                       select element).FirstOrDefault();
                    if (check_element == null)
                    {
                        ComboSubject cu_sub = new ComboSubject();
                        cu_sub.SubjectCode = sj.SubjectCode;
                        cu_sub.ComboId = comboId;
                        _context.Combo_Subject.Add(cu_sub);
                        _context.SaveChanges();
                    }
                    else
                    {
                        return new RedirectResult(url: "/ADMINCombo/Details/" + comboId, permanent: true, preserveMethod: true);
                    }
                }
                else
                {
                    return new RedirectResult(url: "/ADMINCombo/Details/" + comboId, permanent: true, preserveMethod: true);
                }
            }
            return new RedirectResult(url: "/ADMINCombo/Details/" + comboId, permanent: true, preserveMethod: true);
        }
        //public async Task<IActionResult> DeleteSubject(int? id)
        //{
        //    if (id == null || _context.Combo_Subject == null)
        //    {
        //        return NotFound();
        //    }

        //    var comboSubject = await _context.Combo_Subject
        //        .Include(c => c.Combo)
        //        .Include(c => c.Subject)
        //        .FirstOrDefaultAsync(m => m.id == id);
        //    if (comboSubject == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(comboSubject);
        //}

        //// POST: ADMINCombo/Delete/5
        //[HttpPost, ActionName("DeleteSubject")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteSubjectConfirmed(int id)
        //{
        //    if (_context.Combo_Subject == null)
        //    {
        //        return Problem("Entity set 'AppDbContext.Combo_Subject'  is null.");
        //    }
        //    var comboS = await _context.Combo_Subject.FindAsync(id);
        //    //var comboS = _context.Combo_Subject.Where(p => p.id == id).ToList();
        //    //_context.Combo_Subject.RemoveRange(comboS);
        //    if (comboS != null)
        //    {
        //        _context.Combo_Subject.Remove(comboS);
        //    }
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Details));
        //}
        //public async Task<IActionResult> DeleteSubject(int id)
        //{
        //    if (_context.Combo_Subject == null)
        //    {
        //        return Problem("Entity set 'AppDbContext.Combo_Subject'  is null.");
        //    }
        //    var comboS = await _context.Combo_Subject.FindAsync(id);
        //    //var comboS = _context.Combo_Subject.Where(p => p.id == id).ToList();
        //    //_context.Combo_Subject.RemoveRange(comboS);
        //    if (comboS != null)
        //    {
        //        _context.Combo_Subject.Remove(comboS);
        //    }
        //    await _context.SaveChangesAsync();
        //    return new RedirectResult(url: "/ADMINCombo/Details/" + id, permanent: true, preserveMethod: true);
        //}
        public async Task<IActionResult> DeleteSubject(int? id)
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

        // POST: ComboSubjects/Delete/5
        [HttpPost, ActionName("DeleteSubject")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSubjectConfirmed(int id)
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
            return new RedirectResult(url: "/ADMINCombo/Details/" + comboSubject.id, permanent: true, preserveMethod: true);
        }
        private bool ComboExists(int id)
        {
          return (_context.Combo?.Any(e => e.ComboId == id)).GetValueOrDefault();
        }
    }
}
