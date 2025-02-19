using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentEnrollmentSystem.Database.Entity
{
    public class ContactUs
    {
        [Key]
        public int ContactID { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string StudentEmail { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Message { get; set; }

        public DateTime SubmittedAt { get; set; } = DateTime.Now;
    }
}
