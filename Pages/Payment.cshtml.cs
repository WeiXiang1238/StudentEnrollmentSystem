using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentEnrollmentSystem.Database;
using StudentEnrollmentSystem.Database.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

public class PaymentModel : PageModel
{
    private readonly CoreContext _context;

    public PaymentModel(CoreContext context)
    {
        _context = context;
    }

    public List<Enrollment> Enrollments { get; set; } = new();
    public Student? Student { get; set; }
    public Semester? Semester { get; set; }
    public decimal Amount { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var studentEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        Student = await _context.Students.FirstOrDefaultAsync(s => s.Email == studentEmail);

        if (Student == null)
        {
            return RedirectToPage("/Login");
        }

        var systemSetting = await _context.SystemSettings.SingleOrDefaultAsync(e => e.SystemSettingID == 1);
        Semester = systemSetting != null
            ? await _context.Semesters.SingleOrDefaultAsync(e => e.SemesterID == systemSetting.CurrentSemester)
            : null;

        if (Semester == null)
        {
            return Page();
        }

        // Fetch enrollments that have NOT been paid and NOT dropped
        Enrollments = await _context.Enrollments
            .Include(e => e.Course)
            .Where(e => e.StudentID == Student.StudentID
                        && e.SemesterID == Semester.SemesterID
                        && !e.PaymentDate.HasValue
                        && !e.DropDate.HasValue)
            .ToListAsync();

        Amount = Enrollments.Sum(e => e.Course.Amount);

        return Page();
    }

    public async Task<IActionResult> OnPostVerifyPasswordAsync(string password, string paymentMethod)
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
            return new JsonResult(new { success = false, message = "User not found" });
        }

        if (!BCrypt.Net.BCrypt.Verify(password, Student.Password))
        {
            return new JsonResult(new { success = false, message = "Incorrect password" });
        }

        var now = DateTime.Now;
        var enrollments = await _context.Enrollments
            .Include(e => e.Course)
            .Where(e => e.StudentID == Student.StudentID && e.SemesterID == Semester.SemesterID && !e.PaymentDate.HasValue)
            .ToListAsync();

        if (!enrollments.Any())
        {
            return new JsonResult(new { success = false, message = "No courses require payment" });
        }


        Payment payment = new()
        {
            StudentID = Student.StudentID,
            PaymentMethod = paymentMethod,
            Amount = enrollments.Sum(e => e.Course.Amount),
            PaymentDate = now
        };

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync(); // Save first to get PaymentID

        // Assign PaymentID to each enrollment and update PaymentDate
        foreach (var enrollment in enrollments)
        {
            enrollment.PaymentID = payment.PaymentID;
            enrollment.PaymentDate = now;
        }
        await _context.SaveChangesAsync();

        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            WriteIndented = true
        };

        return new JsonResult(new { success = true, data = payment }, options);
    }
}
