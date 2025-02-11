using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentEnrollmentSystem.Database;
using StudentEnrollmentSystem.Database.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json;

public class PaymentModel : PageModel
{
    private readonly CoreContext _context;

    public PaymentModel(CoreContext context)
    {
        _context = context;
    }

    public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public Student? Student { get; set; }
    public Semester? Semester { get; set; }
    public decimal Amount { get; set; }
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

        Enrollments = await _context.Enrollments
            .Where(e => e.StudentID == Student.StudentID && e.SemesterID == Semester.SemesterID && !e.PaymentDate.HasValue)
            .Include(e => e.Course)
            .ToListAsync();

        foreach (var enroll in Enrollments)
        {
            Amount += enroll.Course.Amount;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostVerifyPasswordAsync(string password, string paymentMethod)
    {
        try
        {
            var studentEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            Student = await _context.Students.FirstOrDefaultAsync(s => s.Email == studentEmail);
            SystemSetting? setting = await _context.SystemSettings.SingleOrDefaultAsync(e => e.SystemSettingID == 1);
            if (setting != null)
            {
                Semester = await _context.Semesters.SingleOrDefaultAsync(e => e.SemesterID == setting.CurrentSemester);
            }

            if (Student == null )
            {
                return new JsonResult(new { success = false });
            }

            var now = DateTime.Now;

            Enrollments = await _context.Enrollments
            .Where(e => e.StudentID == Student.StudentID && e.SemesterID == Semester.SemesterID)
            .Include(e => e.Course)
            .ToListAsync();

            foreach (var enroll in Enrollments)
            {
                enroll.PaymentDate = now;
                Amount += enroll.Course.Amount;
            }

            Payment payment = new Payment()
            {
                StudentID = Student.StudentID,
                PaymentMethod = paymentMethod,
                Amount = Amount,
                PaymentDate = now,
            };

            _context.Payments.Add(payment);

            await _context.SaveChangesAsync();

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };

            return new JsonResult(new { success = true, data = payment }, options);
        }
        catch (Exception ex)
        {
            return new JsonResult(new { success = false, message = "Error: " + ex.Message });
        }
    }
}
