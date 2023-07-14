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
    public class ADMINCurriculaController : Controller
    {
        private readonly AppDbContext _context;

        public ADMINCurriculaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Curricula
        public async Task<IActionResult> Index()
        {
            IEnumerable<Curriculum> appDbContext = _context.Curriculum.Include(c => c.Decision).ToList();
            return View(appDbContext);
        }

        // GET: Curricula/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Curriculum == null)
            {
                return NotFound();
            }

            var curriculum = await _context.Curriculum
                .Include(c => c.Decision)
                .FirstOrDefaultAsync(m => m.CurriculumId == id);
            var curri_sub= _context.Curriculum_Subject.Include(p => p.Subject).Where(pro => pro.CurriculumId==id).OrderBy(p=>p.Semester).ToList();
            ViewBag.Curri_sub=curri_sub;
            if (curriculum == null)
            {
                return NotFound();
            }

            return View(curriculum);
        }

        // GET: Curricula/Create
        public IActionResult Create()
        {
            ViewData["DecisionNo"] = new SelectList(_context.Decision, "DecisionNo", "DecisionNo");
            return View();
        }

         // POST: Curricula/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CurriculumId,CurriculumCode,NameVn,NameEn,Decription,DecisionNo,TotalCredit,IsActive")] Curriculum curriculum)
        {
            
            if (ModelState.IsValid)
            {
                Curriculum checkCode= await _context.Curriculum.Include(p => p.Decision).FirstOrDefaultAsync(pro => pro.CurriculumCode.Equals(curriculum.CurriculumCode));
                if (checkCode == null)
                {
                    _context.Add(curriculum);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["Error"] = "Wrong: Curriculum is already exist !";
                }
            }
            ViewData["DecisionNo"] = new SelectList(_context.Decision, "DecisionNo", "DecisionNo", curriculum.DecisionNo);
            return View(curriculum);
        }

        // GET: Curricula/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Curriculum == null)
            {
                return NotFound();
            }

            var curriculum = await _context.Curriculum.FindAsync(id);
            if (curriculum == null)
            {
                return NotFound();
            }
            ViewData["DecisionNo"] = new SelectList(_context.Decision, "DecisionNo", "DecisionNo", curriculum.DecisionNo);
            return View(curriculum);
        }

        // POST: Curricula/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CurriculumId,CurriculumCode,NameVn,NameEn,Decription,DecisionNo,TotalCredit,IsActive")] Curriculum curriculum)
        {
            if (id != curriculum.CurriculumId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(curriculum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CurriculumExists(curriculum.CurriculumId))
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
            ViewData["DecisionNo"] = new SelectList(_context.Decision, "DecisionNo", "DecisionNo", curriculum.DecisionNo);
            return View(curriculum);
        }

        // GET: Curricula/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Curriculum == null)
            {
                return NotFound();
            }

            var curriculum = await _context.Curriculum
                .Include(c => c.Decision)
                .FirstOrDefaultAsync(m => m.CurriculumId == id);
            if (curriculum == null)
            {
                return NotFound();
            }
            return View(curriculum);
        }

        // POST: Curricula/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Curriculum == null)
            {
                return Problem("Entity set 'AppDbContext.Curriculum'  is null.");
            }
            //DA SUA THEM DELETE 
            var curriculum = await _context.Curriculum.FindAsync(id);
            var check = _context.Curriculum_Subject.Where(p => p.CurriculumId == id).ToList();
            var combo = _context.Combo.Where(p => p.CurriculumId == id).ToList();
            foreach (var item in combo)
            {
                var com_sub = _context.Combo_Subject.Where(p => p.ComboId == item.ComboId).ToList();
                _context.Combo_Subject.RemoveRange(com_sub);
            }
            _context.Combo.RemoveRange(combo);
            _context.Curriculum_Subject.RemoveRange(check);
            _context.Curriculum.Remove(curriculum);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// PHAN THEM
        public async Task<IActionResult> AddSubject(string semester)
        {
            string add = Request.Form["add"];
            int curriId = Convert.ToInt32(Request.Form["curriId"]);
            
            if (add != null && !add.Trim().Equals(""))
            {
                Subject sj = _context.Subject.Include(p => p.Status).FirstOrDefault(pro => pro.SubjectCode.Equals(add.Trim()));
                if (sj != null)
                {

                    var check = _context.Curriculum_Subject.Where(p => p.CurriculumId == curriId).ToList();
                    CurriculumSubject check_element = (from element in check
                                        where element.SubjectCode.Trim().Equals(sj.SubjectCode.Trim())
                                        select element).FirstOrDefault();
                    if (check_element==null)
                    {
                        
                        CurriculumSubject cu_sub = new CurriculumSubject();
                        cu_sub.SubjectCode = sj.SubjectCode;
                        cu_sub.CurriculumId = curriId;
                        if (semester != null)
                        {
                            int semester_int = Convert.ToInt32(semester);
                            cu_sub.Semester = semester_int;
                        }
                        _context.Curriculum_Subject.Add(cu_sub);
                        _context.SaveChanges();
                        return new RedirectResult(url: "/ADMINCurricula/Details/" + curriId, permanent: true, preserveMethod: true);
                    }
                    else
                    {
                        TempData["error"] = "Sorry: This subject already exists, please add another subject !";
                        return new RedirectResult(url: "/ADMINCurricula/Details/" + curriId, permanent: true, preserveMethod: true);
                    }
                }
                else
                {
                    TempData["error"] = "Sorry: Subject not found, please enter valid subject code !";
                    return new RedirectResult(url: "/ADMINCurricula/Details/" + curriId, permanent: true, preserveMethod: true);
                }
            }
            TempData["error"] = "Sorry: Subject not found, please enter valid subject code !";
            return new RedirectResult(url: "/ADMINCurricula/Details/" + curriId, permanent: true, preserveMethod: true);
        }

        /// PHAN THEM
        public async Task<IActionResult> DeleteSubject(int id, int cu_sub_ID)
        {
            //CurriculumSubject cu_sub = new CurriculumSubject();
            //cu_sub.SubjectCode = subCode;
            //cu_sub.CurriculumId = id;
            CurriculumSubject cu_sub= _context.Curriculum_Subject.Find(cu_sub_ID);
            var combo = _context.Combo.Where(p => p.CurriculumId==id).ToList();
            foreach(var item in combo)
            {
                var com_sub = _context.Combo_Subject.Where(p => p.ComboId==item.ComboId).ToList();
               foreach(var  com in com_sub)
                {
                   if(com.SubjectCode.Equals(cu_sub.SubjectCode))
                    {
                        _context.Combo_Subject.Remove(com);
                        _context.SaveChanges();
                    }
                }
            }
            _context.Curriculum_Subject.Remove(cu_sub);
            _context.SaveChanges();
            return new RedirectResult(url: "/ADMINCurricula/Details/" + id, permanent: true, preserveMethod: true);
        }

        private bool CurriculumExists(int id)
        {
          return (_context.Curriculum?.Any(e => e.CurriculumId == id)).GetValueOrDefault();
        }
    }
}
