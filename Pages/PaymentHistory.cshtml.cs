using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentEnrollmentSystem.Database;
using StudentEnrollmentSystem.Database.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class PaymentHistoryModel : PageModel
{
    private readonly CoreContext _context;

    public PaymentHistoryModel(CoreContext context)
    {
        _context = context;
    }

    [BindProperty]
    public DateTime FromDate { get; set; } = DateTime.Now.AddMonths(-6);

    [BindProperty]
    public DateTime ToDate { get; set; } = DateTime.Now;

    public List<Payment> Payments { get; set; } = new();
    public Student? Student { get; set; }
    public Semester? Semester { get; set; }
    public bool IsDataLoaded { get; set; } = false;

    public async Task<IActionResult> OnGetAsync()
    {
        var studentEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        Student = await _context.Students.FirstOrDefaultAsync(s => s.Email == studentEmail);
        SystemSetting? setting = await _context.SystemSettings.SingleOrDefaultAsync(e => e.SystemSettingID == 1);
        if (setting != null)
        {
            Semester = await _context.Semesters.SingleOrDefaultAsync(e => e.SemesterID == setting.CurrentSemester);
        }
        if (Student == null)
        {
            return RedirectToPage("/Login");
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var studentEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        Student = await _context.Students.FirstOrDefaultAsync(s => s.Email == studentEmail);
        SystemSetting? setting = await _context.SystemSettings.SingleOrDefaultAsync(e => e.SystemSettingID == 1);
        if (setting != null)
        {
            Semester = await _context.Semesters.SingleOrDefaultAsync(e => e.SemesterID == setting.CurrentSemester);
        }
        if (Student == null)
        {
            return RedirectToPage("/Login");
        }

        Payments = await _context.Payments
            .Where(p => p.StudentID == Student.StudentID && p.PaymentDate >= FromDate && p.PaymentDate <= ToDate.AddDays(1).AddTicks(-1))
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();

        IsDataLoaded = true; // To control error message display
        return Page();
    }
}
