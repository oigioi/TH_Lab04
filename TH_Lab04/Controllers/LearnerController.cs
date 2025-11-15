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

        // GET: /Learner/Index (List - R) - Hiển thị View Component và vùng chứa AJAX
        public IActionResult Index(int? id)
        {
            // Đây là logic cho lần tải trang đầu tiên (hoặc lọc đồng bộ)
            IQueryable<Learner> learners = db.Learners.Include(m => m.Major);
            if (id != null)
            {
                learners = learners.Where(l => l.MajorID == id);
            }

            // Trả về danh sách Learner đầy đủ hoặc đã lọc cho Partial View ban đầu
            return View(learners.ToList());
        }

        // GET: /Learner/LearnerByMajorID (Xử lý AJAX)
        // Action này chỉ trả về Partial View chứa danh sách Learner đã lọc
        public IActionResult LearnerByMajorID(int mid)
        {
            // Lọc Learner theo MajorID
            var learners = db.Learners.Where(l => l.MajorID == mid).Include(m => m.Major).ToList();

            // Trả về Partial View "LearnerTable"
            return PartialView("LearnerTable", learners);
        }

        // --- CÁC ACTION CRUD CÓ SẴN TỪ BÀI LAB 4 ---

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