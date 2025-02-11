using System.ComponentModel.DataAnnotations;

namespace StudentEnrollmentSystem.Database.Entity
{
    public class Student
    {
        [Key]
        public int StudentID { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [StringLength(15)]
        public string ContactNumber { get; set; }

        public string Address { get; set; }

        public string BankAccNum { get; set; }

        public string BankAccName { get; set; }

        public string Bank { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
