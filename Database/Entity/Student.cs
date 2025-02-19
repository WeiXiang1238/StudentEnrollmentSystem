using System.ComponentModel.DataAnnotations;

namespace StudentEnrollmentSystem.Database.Entity
{
    public class Student
    {
        [Key]
        public int StudentID { get; set; }
        [Required]
        [StringLength(20)]
        public string MatricNo { get; set; }
        [Required]
        [StringLength(20)]
        public string ICNumber { get; set; }
        [Required]
        [StringLength(200)]
        public string Program { get; set; }
        [Required]
        [StringLength(200)]
        public string FullName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [StringLength(15)]
        public string ContactNumber { get; set; }
        [StringLength(200)]
        public string? Address1 { get; set; }
        [StringLength(200)]
        public string? Address2 { get; set; }
        [StringLength(8)]
        public string? PostCode { get; set; }
        [StringLength(100)]
        public string? City { get; set; }
        [StringLength(100)]
        public string? State { get; set; }
        [StringLength(100)]
        public string? Country { get; set; }
        [StringLength(100)]
        public string? EmergencyContactRelationship { get; set; }
        [StringLength(200)]
        public string? EmergencyContactName { get; set; }
        [StringLength(20)]
        public string? EmergencyContactPhoneNumber { get; set; }
        public string BankAccNum { get; set; }
        public string BankAccName { get; set; }
        public string Bank { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
