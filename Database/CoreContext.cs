using Microsoft.EntityFrameworkCore;
using StudentEnrollmentSystem.Database.Entity;

namespace StudentEnrollmentSystem.Database
{
    public class CoreContext : DbContext
    {
        public CoreContext(DbContextOptions<CoreContext> options) : base(options) { }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<ContactUs> ContactUs { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<TeachingEvaluation> TeachingEvaluations { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<SystemSetting> SystemSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentID);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseID);
        }
    }
}
