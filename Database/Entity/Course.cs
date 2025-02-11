using System.ComponentModel.DataAnnotations;

namespace StudentEnrollmentSystem.Database.Entity
{
    public class Course
    {
        [Key]
        public int CourseID { get; set; }

        [Required]
        public string CourseCode { get; set; }

        [Required]
        [StringLength(150)]
        public string CourseName { get; set; }

        [Required]
        public string Day { get; set; } // "Monday", "Tuesday"

        [Required]
        public TimeSpan StartTime { get; set; } // Use TimeSpan for better time calculations

        [Required]
        public TimeSpan EndTime { get; set; }

        [Required]
        public string Venue { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public int Credits { get; set; }

        public string Faculty { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
