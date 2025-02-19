using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentEnrollmentSystem.Database;
using StudentEnrollmentSystem.Database.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class InvoiceListModel : PageModel
{
    private readonly CoreContext _context;

    public InvoiceListModel(CoreContext context)
    {
        _context = context;
    }

    public List<Payment> Invoices { get; set; } = new();
    public List<SelectListItem> Semesters { get; set; } = new();
    public Student? Student { get; set; }
    public int CurrentSemesterID { get; set; }

    [BindProperty(SupportsGet = true)]
    public int SelectedSemesterID { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var studentEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        Student = await _context.Students.FirstOrDefaultAsync(s => s.Email == studentEmail);

        if (Student == null)
        {
            return RedirectToPage("/Login");
        }

        // Get current semester
        var systemSetting = await _context.SystemSettings.FirstOrDefaultAsync();
        CurrentSemesterID = systemSetting?.CurrentSemester ?? 0;

        Semesters = await _context.Semesters
            .Select(s => new SelectListItem { Value = s.SemesterID.ToString(), Text = s.Name })
            .ToListAsync();

        // Set the default semester selection to the current semester
        if (SelectedSemesterID == 0 && CurrentSemesterID != 0)
        {
            SelectedSemesterID = CurrentSemesterID;
        }

        if (SelectedSemesterID != 0)
        {
            Invoices = await _context.Payments
                .Where(p => _context.Enrollments
                    .Any(e => e.PaymentID == p.PaymentID && e.SemesterID == SelectedSemesterID))
                .ToListAsync();
        }

        return Page();
    }
}
