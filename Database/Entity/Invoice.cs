using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentEnrollmentSystem.Database.Entity
{
    public class Invoice
    {
        [Key]
        public int InvoiceID { get; set; }

        [ForeignKey("Student")]
        public int StudentID { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [StringLength(20)]
        public string Status { get; set; } // "Paid", "Pending", "Overdue"

        // Navigation Property
        public Student Student { get; set; }
    }
}
