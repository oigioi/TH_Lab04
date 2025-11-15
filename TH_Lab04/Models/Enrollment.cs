namespace TH_Lab04.Models
{
    public class Enrollment
    {
        public int EnrollmentID { get; set; }

        // Foreign Keys
        public int CourseID { get; set; }
        public int LearnerID { get; set; }

        public float Grade { get; set; }

        // Navigation Properties
        public virtual Learner? Learner { get; set; }
        public virtual Course? Course { get; set; }
    }
}
