using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentEnrollmentSystem.Database.Entity
{
    public class TeachingEvaluation
    {
        [Key]
        public int EvaluationID { get; set; }

        [ForeignKey("Student")]
        public int StudentID { get; set; }

        [ForeignKey("Course")]
        public int CourseID { get; set; }

        [Required]
        public int Rating { get; set; } // Scale of 1-5

        public string Comments { get; set; }

        // Navigation Properties
        public Student Student { get; set; }
        public Course Course { get; set; }
    }
}
