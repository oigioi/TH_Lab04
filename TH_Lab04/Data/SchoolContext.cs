using Microsoft.EntityFrameworkCore;
using TH_Lab04.Models;

namespace TH_Lab04.Data
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options)
            : base(options) { }

        // Định nghĩa các DbSet
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Learner> Learners { get; set; }
        public virtual DbSet<Enrollment> Enrollments { get; set; }
        public virtual DbSet<Major> Majors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Major>().ToTable(nameof(Major));
            modelBuilder.Entity<Course>().ToTable(nameof(Course));
            modelBuilder.Entity<Learner>().ToTable(nameof(Learner));
            modelBuilder.Entity<Enrollment>().ToTable(nameof(Enrollment));
        }
    }
}
