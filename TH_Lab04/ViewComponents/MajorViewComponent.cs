using Microsoft.AspNetCore.Mvc;
using TH_Lab04.Data;
using System.Linq; 

namespace MyWebApp.ViewComponents
{
    public class MajorViewComponent : ViewComponent
    {
        private readonly SchoolContext db;

        public MajorViewComponent(SchoolContext context)
        {
            db = context;
        }
        public IViewComponentResult Invoke()
        {
            var majors = db.Majors.ToList();
            return View(majors);
        }
    }
}