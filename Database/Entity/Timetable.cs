using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentEnrollmentSystem.Database.Entity
{
    public class Timetable
    {
        [Key]
        public int TimetableID { get; set; }

        [ForeignKey("Course")]
        public int CourseID { get; set; }

        [Required]
        public string Day { get; set; } // "Monday", "Tuesday"

        [Required]
        public string Time { get; set; } // "10:00 AM - 12:00 PM"

        public string Venue { get; set; }

        public Course Course { get; set; }
    }
}
