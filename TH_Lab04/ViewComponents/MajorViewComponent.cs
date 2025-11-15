using Microsoft.AspNetCore.Mvc;
using TH_Lab04.Data;
using System.Linq; // Cần thiết cho .ToList()

namespace MyWebApp.ViewComponents
{
    public class MajorViewComponent : ViewComponent
    {
        private readonly SchoolContext db;

        public MajorViewComponent(SchoolContext context)
        {
            db = context;
        }

        // Phương thức chính được gọi khi View Component được invoke
        public IViewComponentResult Invoke()
        {
            // Lấy danh sách Majors
            var majors = db.Majors.ToList();
            return View(majors);
        }
    }
}