using Microsoft.EntityFrameworkCore;
using TH_Lab04.Models;

namespace TH_Lab04.Data
{
    public class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new SchoolContext(serviceProvider
                .GetRequiredService<DbContextOptions<SchoolContext>>()))
            {
                // Đảm bảo CSDL được tạo (nếu chưa tồn tại)
                context.Database.EnsureCreated();

                // Kiểm tra xem đã có dữ liệu Major chưa
                if (context.Majors.Any())
                {
                    return; // DB đã có dữ liệu, thoát
                }

                // 1. Tạo Majors
                var majors = new Major[] {
                    new Major{MajorName="IT"},
                    new Major{MajorName="Economics"},
                    new Major{MajorName="Mathematics"}
                };
                context.Majors.AddRange(majors);
                context.SaveChanges();

                // 2. Tạo Learners (sử dụng MajorID đã lưu)
                var learners = new Learner[] {
                    new Learner { FirstMidName = "Carson", LastName = "Alexander",
                        EnrollmentDate = DateTime.Parse("2005-09-01"), MajorID = majors.Single(m => m.MajorName == "IT").MajorID },
                    new Learner { FirstMidName = "Meredith", LastName = "Alonso",
                        EnrollmentDate = DateTime.Parse("2002-09-01"), MajorID = majors.Single(m => m.MajorName == "Economics").MajorID }
                };
                context.Learners.AddRange(learners);
                context.SaveChanges();

                // 3. Tạo Courses
                var courses = new Course[] {
                    new Course{CourseID = 1050, Title="Chemistry", Credits=3},
                    new Course{CourseID = 4022, Title="Microeconomics", Credits=3},
                    new Course{CourseID = 4041, Title="Macroeconomics", Credits=3}
                };
                context.Courses.AddRange(courses);
                context.SaveChanges();

                // 4. Tạo Enrollments
                var enrollments = new Enrollment[]{
                    new Enrollment {LearnerID = learners.Single(l => l.LastName == "Alexander").LearnerID, CourseID = 1050, Grade = 5.5f},
                    new Enrollment {LearnerID = learners.Single(l => l.LastName == "Alexander").LearnerID, CourseID = 4022, Grade = 7.5f},
                    new Enrollment {LearnerID = learners.Single(l => l.LastName == "Alonso").LearnerID, CourseID = 1050, Grade = 3.5f},
                    new Enrollment {LearnerID = learners.Single(l => l.LastName == "Alonso").LearnerID, CourseID = 4041, Grade = 7f}
                };
                context.Enrollments.AddRange(enrollments);
                context.SaveChanges();
            }
        }
    }
}
