using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LMMProject.Data;
using LMMProject.Models;
using MySqlX.XDevAPI;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
namespace LMMProject.Controllers
{
    public class ADMINSessionController : Controller
    {
        private readonly AppDbContext _context;

        public ADMINSessionController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Sessions
        public async Task<IActionResult> Index(string? id)
        {
            Subject sub = _context.Subject.FirstOrDefault(s => s.SubjectCode == id);
            ViewBag.SubjectCode = sub;
            var listSession = _context.Session.Include(s => s.Subject).Where(sus => sus.SubjectCode == id).ToList();
            return View(listSession);
        }
        // GET: Sessions/Create
        public IActionResult Create()
        {
            ViewData["SubjectCode"] = new SelectList(_context.Subject, "SubjectCode", "SubjectCode");
            return View();
        }

        // POST: Sessions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SessionId,Topic,LearningTeachingType,StudentMaterials,Constructivequestion,SubjectCode")] LMMProject.Models.Session session)
        {
            try
            {
                if (SessionExists(session.SessionId))
                {
                    return View(session);
                }
                Models.Session ses = new Models.Session();
                ses.SessionId = session.SessionId;
                ses.Topic = session.Topic;
                ses.LearningTeachingType = session.LearningTeachingType;
                ses.StudentMaterials = session.StudentMaterials;
                ses.Constructivequestion = session.Constructivequestion;
                ses.SubjectCode = session.SubjectCode;
                _context.Session.AddAsync(ses);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            ViewData["SubjectCode"] = new SelectList(_context.Subject, "SubjectCode", "SubjectCode", session.SubjectCode);
            return new RedirectResult(url: "/ADMINSession/Index/" + session.SubjectCode, permanent: true, preserveMethod: true);
        }

        // GET: Sessions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Session == null)
            {
                return NotFound();
            }

            var session = await _context.Session.FindAsync(id);
            if (session == null)
            {
                return NotFound();
            }
            ViewData["SubjectCode"] = new SelectList(_context.Subject, "SubjectCode", "SubjectCode", session.SubjectCode);
            return View(session);
        }

        // POST: Sessions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( [Bind("SessionId,Topic,LearningTeachingType,StudentMaterials,Constructivequestion,SubjectCode")] Models.Session session)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(session);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SessionExists(session.SessionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return new RedirectResult(url: "/ADMINSession/Index/" + session.SubjectCode, permanent: true, preserveMethod: true);
            }
            ViewData["SubjectCode"] = new SelectList(_context.Subject, "SubjectCode", "SubjectCode", session.SubjectCode);
            return View(session);
        }

        // GET: Sessions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Session == null)
            {
                return NotFound();
            }

            var session = await _context.Session
                .Include(s => s.Subject)
                .FirstOrDefaultAsync(m => m.SessionId == id);
            if (session == null)
            {
                return NotFound();
            }

            return View(session);
        }

        // POST: Sessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var session = await _context.Session.Include(s => s.Subject).FirstOrDefaultAsync(s => s.SessionId == id);
            if(session == null)
            {
                return NotFound();
            }
            _context.Session.RemoveRange(session);
            await _context.SaveChangesAsync();
            return new RedirectResult(url: "/ADMINSession/Index/" + session.SubjectCode, permanent: true, preserveMethod: true);
        }

        private bool SessionExists(int id)
        {
          return (_context.Session?.Any(e => e.SessionId == id)).GetValueOrDefault();
        }
    }
}
