using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentEnrollmentSystem.Database;
using StudentEnrollmentSystem.Database.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ReceiptModel : PageModel
{
    private readonly CoreContext _context;

    public ReceiptModel(CoreContext context)
    {
        _context = context;
    }

    public Payment? Payment { get; set; }
    public Student? Student { get; set; }
    public List<Enrollment> Enrollments { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Payment = await _context.Payments.FindAsync(id);

        if (Payment == null)
        {
            return NotFound();
        }

        Student = await _context.Students.FindAsync(Payment.StudentID);

        // Fetch enrollments linked to the payment
        Enrollments = await _context.Enrollments
            .Include(e => e.Course)
            .Where(e => e.PaymentID == Payment.PaymentID)
            .ToListAsync();

        return Page();
    }
}
