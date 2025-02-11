using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentEnrollmentSystem.Database.Entity
{
    public class Payment
    {
        [Key]
        public int PaymentID { get; set; }

        [ForeignKey("Student")]
        public int StudentID { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        [StringLength(50)]
        public string PaymentMethod { get; set; }

        public Student Student { get; set; }
    }
}
