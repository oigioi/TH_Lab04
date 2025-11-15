using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TH_Lab04.Data;
using TH_Lab04.Models;
using TH_Lab04.Data;
using TH_Lab04.Models;

namespace MyWebApp.Controllers
{
    public class LearnerController : Controller
    {
        private readonly SchoolContext db;

        public LearnerController(SchoolContext context)
        {
            db = context;
        }

        // GET: /Learner/Index
        public IActionResult Index(int? id)
        {
            IQueryable<Learner> learners = db.Learners.Include(m => m.Major);
            if (id != null)
            {
                learners = learners.Where(l => l.MajorID == id);
            }
            return View(learners.ToList());
        }

        // GET: /Learner/LearnerByMajorID
        public IActionResult LearnerByMajorID(int mid)
        {
            var learners = db.Learners.Where(l => l.MajorID == mid).Include(m => m.Major).ToList();

            return PartialView("LearnerTable", learners);
        }

        public IActionResult Create()
        {
            ViewBag.MajorID = new SelectList(db.Majors, "MajorID", "MajorName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("FirstMidName, LastName, MajorID, EnrollmentDate")] Learner learner)
        {
            if (ModelState.IsValid)
            {
                db.Learners.Add(learner);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MajorID = new SelectList(db.Majors, "MajorID", "MajorName", learner.MajorID);
            return View(learner);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();
            var learner = db.Learners.Find(id);
            if (learner == null) return NotFound();
            ViewBag.MajorID = new SelectList(db.Majors, "MajorID", "MajorName", learner.MajorID);
            return View(learner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("LearnerID,FirstMidName,LastName,MajorID,EnrollmentDate")] Learner learner)
        {
            if (id != learner.LearnerID) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(learner);
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!db.Learners.Any(e => e.LearnerID == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MajorID = new SelectList(db.Majors, "MajorID", "MajorName", learner.MajorID);
            return View(learner);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            var learner = db.Learners.Include(l => l.Major).Include(l => l.Enrollments)
                .FirstOrDefault(m => m.LearnerID == id);
            if (learner == null) return NotFound();
            if (learner.Enrollments.Any()) return Content("This learner has enrollments, cannot delete!");
            return View(learner);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var learner = db.Learners.Find(id);
            if (learner != null) db.Learners.Remove(learner);
            db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}