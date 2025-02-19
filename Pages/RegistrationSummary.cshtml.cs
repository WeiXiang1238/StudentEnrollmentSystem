using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentEnrollmentSystem.Database;
using StudentEnrollmentSystem.Database.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class RegistrationSummaryModel : PageModel
{
    private readonly CoreContext _context;

    public RegistrationSummaryModel(CoreContext context)
    {
        _context = context;
    }

    public Student Student { get; set; }
    public Semester CurrentSemester { get; set; }
    public List<Enrollment> EnrolledCourses { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        var studentEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        Student = await _context.Students
            .FirstOrDefaultAsync(s => s.Email == studentEmail);

        if (Student == null)
        {
            return RedirectToPage("/Login");
        }

        var systemSetting = await _context.SystemSettings.FirstOrDefaultAsync();
        CurrentSemester = await _context.Semesters
            .FirstOrDefaultAsync(s => s.SemesterID == systemSetting.CurrentSemester);

        // **Fetch only Active Enrolled Courses (Excludes Dropped Courses)**
        EnrolledCourses = await _context.Enrollments
            .Include(e => e.Course)
            .Include(e => e.Semester) // Ensure Semester details are available
            .Where(e => e.StudentID == Student.StudentID && e.SemesterID == CurrentSemester.SemesterID && e.Status == "Active")
            .ToListAsync();

        return Page();
    }
}
