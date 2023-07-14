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
    public class ADMINSyllabusController : Controller
    {
        private readonly AppDbContext _context;

        public ADMINSyllabusController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Syllabus
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Syllabus.Include(s => s.Decision);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Syllabus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var syllabus = await _context.Syllabus
                .Include(s => s.Decision)
                .Include(s => s.Assessments)
                .FirstOrDefaultAsync(m => m.SyllabusId == id);
            if (syllabus == null)
            {
                return NotFound();
            }

            return View(syllabus);
        }

        // GET: Syllabus/Create
        public IActionResult Create()
        {
            ViewBag.Sub = _context.Subject.ToList();
            ViewData["DecisionNo"] = new SelectList(_context.Decision, "DecisionNo", "DecisionNo");
            return View();
        }

        // POST: Syllabus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SyllabusId,SyllabusNameVn,SyllabusNameEn,SubjectCode,NoCredit,DegreeLevel,TimeAllocation,PreRequisite,Description,StudentTask,Tool,ScoringScale,DecisionNo,IsApproved,Note,MinAvgMarkToPass,IsActive")] Syllabus syllabus)
        {


            try
            {
                if (SyllabusExists(syllabus.SyllabusId))
                {
                    return View(syllabus);
                }

                Syllabus syl = new Syllabus();
                syl.SyllabusNameVn = syllabus.SyllabusNameVn;
                syl.SyllabusNameEn = syllabus.SyllabusNameEn;
                syl.SubjectCode = syllabus.SubjectCode;
                syl.NoCredit = syllabus.NoCredit;

                syl.DegreeLevel = syllabus.DegreeLevel;
                syl.TimeAllocation = syllabus.TimeAllocation;
                syl.PreRequisite = syllabus.PreRequisite;
                syl.Description = syllabus.Description;
                syl.StudentTask = syllabus.StudentTask;
                syl.Tool = syllabus.Tool;
                syl.ScoringScale = syllabus.ScoringScale;
                syl.DecisionNo = syllabus.DecisionNo;
                syl.IsApproved = syllabus.IsApproved;
                syl.Note = syllabus.Note;
                syl.MinAvgMarkToPass = syllabus.MinAvgMarkToPass;
                syl.IsActive = syllabus.IsActive;
                _context.Syllabus.AddAsync(syl);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw ex;

            }


            ViewData["DecisionNo"] = new SelectList(_context.Decision, "DecisionNo", "DecisionNo", syllabus.DecisionNo);
            return RedirectToAction(nameof(Index));


        }

        // GET: Syllabus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Syllabus == null)
            {
                return NotFound();
            }

            var syllabus = await _context.Syllabus.FindAsync(id);
            if (syllabus == null)
            {
                return NotFound();
            }
            ViewBag.Sub = _context.Subject.ToList();
            ViewData["DecisionNo"] = new SelectList(_context.Decision, "DecisionNo", "DecisionNo", syllabus.DecisionNo);
            return View(syllabus);
        }

        // POST: Syllabus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SyllabusId,SyllabusNameVn,SyllabusNameEn,SubjectCode,NoCredit,DegreeLevel,TimeAllocation,PreRequisite,Description,StudentTask,Tool,ScoringScale,DecisionNo,IsApproved,Note,MinAvgMarkToPass,IsActive")] Syllabus syllabus)
        {
            if (id != syllabus.SyllabusId)
            {
                return NotFound();
            }


            try
            {
                var syl = await _context.Syllabus.SingleOrDefaultAsync(s => s.SyllabusId == id);
                syl.SyllabusNameVn = syllabus.SyllabusNameVn;
                syl.SyllabusNameEn = syllabus.SyllabusNameEn;
                syl.SubjectCode = syllabus.SubjectCode;
                syl.NoCredit = syllabus.NoCredit;

                syl.DegreeLevel = syllabus.DegreeLevel;
                syl.TimeAllocation = syllabus.TimeAllocation;
                syl.PreRequisite = syllabus.PreRequisite;
                syl.Description = syllabus.Description;
                syl.StudentTask = syllabus.StudentTask;
                syl.Tool = syllabus.Tool;
                syl.ScoringScale = syllabus.ScoringScale;
                syl.DecisionNo = syllabus.DecisionNo;
                syl.IsApproved = syllabus.IsApproved;
                syl.Note = syllabus.Note;
                syl.MinAvgMarkToPass = syllabus.MinAvgMarkToPass;
                syl.IsActive = syllabus.IsActive;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (!SyllabusExists(syllabus.SyllabusId))
                {
                    return NotFound();
                }
                else
                {
                    throw ex;
                }
            }


            ViewData["DecisionNo"] = new SelectList(_context.Decision, "DecisionNo", "DecisionNo", syllabus.DecisionNo);
            return RedirectToAction(nameof(Index));
        }

        // GET: Syllabus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Syllabus == null)
            {
                return NotFound();
            }

            var syllabus = await _context.Syllabus
                .Include(s => s.Decision)
                .FirstOrDefaultAsync(m => m.SyllabusId == id);
            if (syllabus == null)
            {
                return NotFound();
            }

            return View(syllabus);
        }

        // POST: Syllabus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var syllabus = await _context.Syllabus.Include(s => s.Assessments).FirstOrDefaultAsync(s => s.SyllabusId == id);
            if (syllabus == null)
            {
                // Syllabus not found, handle the error
                return NotFound();
            }

            // Remove the related assessments
            _context.Assessment.RemoveRange(syllabus.Assessments);

            // Proceed with deleting the syllabus
            _context.Syllabus.Remove(syllabus);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        private bool SyllabusExists(int id)
        {
            bool isExist = (_context.Syllabus?.Any(e => e.SyllabusId == id)).GetValueOrDefault();
            return isExist;
        }

    }
}
