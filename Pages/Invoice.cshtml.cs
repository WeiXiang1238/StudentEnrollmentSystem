using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentEnrollmentSystem.Database;
using StudentEnrollmentSystem.Database.Entity;
using System.Threading.Tasks;

public class InvoiceModel : PageModel
{
    private readonly CoreContext _context;

    public InvoiceModel(CoreContext context)
    {
        _context = context;
    }

    public Payment Invoice { get; set; }
    public Student Student { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Invoice = await _context.Payments.FirstOrDefaultAsync(p => p.PaymentID == id);

        if (Invoice == null)
        {
            return NotFound();
        }

        Student = await _context.Students.FirstOrDefaultAsync(s => s.StudentID == Invoice.StudentID);

        return Page();
    }
}
