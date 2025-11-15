using System.ComponentModel.DataAnnotations.Schema;

namespace TH_Lab04.Models
{
    public class Course
    {
        public Course()
        {
            Enrollments = new HashSet<Enrollment>();
        }

        // Không để CSDL tự sinh khóa
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CourseID { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }

        // Navigation Property
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}