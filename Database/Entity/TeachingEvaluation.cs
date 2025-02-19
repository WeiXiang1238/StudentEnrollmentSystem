using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentEnrollmentSystem.Database.Entity
{
    public class TeachingEvaluation
    {
        [Key]
        public int EvaluationID { get; set; }

        [ForeignKey("Enrollment")]
        public int EnrollmentID { get; set; }

        [Required]
        public int Rating { get; set; }

        [StringLength(1000)]
        public string Comments { get; set; }

        public DateTime SubmittedAt { get; set; } = DateTime.Now;

        public Enrollment Enrollment { get; set; }

        [NotMapped]
        public int StudentID => Enrollment.StudentID;

        [NotMapped]
        public int CourseID => Enrollment.CourseID;

        [NotMapped]
        public int SemesterID => Enrollment.SemesterID;
    }
}
