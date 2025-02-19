using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentEnrollmentSystem.Database.Entity
{
    public class Enrollment
    {
        [Key]
        public int EnrollmentID { get; set; }
        [ForeignKey("Student")]
        public int StudentID { get; set; }
        [ForeignKey("Course")]
        public int CourseID { get; set; }
        [ForeignKey("Semester")]
        public int SemesterID { get; set; }
        [ForeignKey("Payment")]
        public int? PaymentID { get; set; }
        [Required]
        public DateTime EnrollmentDate { get; set; }
        [StringLength(20)]
        public string Status { get; set; }
        [Required]
        public Student Student { get; set; }
        [Required]
        public Course Course { get; set; }
        [Required]
        public Semester Semester { get; set; }
        public Payment? Payment { get; set; }
        public DateTime? DropDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        [Required]
        [StringLength(10)]
        public string Action { get; set; }
    }
}
