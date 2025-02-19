using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentEnrollmentSystem.Database;
using StudentEnrollmentSystem.Database.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class AddDropHistoryModel : PageModel
{
    private readonly CoreContext _context;

    public AddDropHistoryModel(CoreContext context)
    {
        _context = context;
    }

    public Student Student { get; set; }
    public List<EnrollmentHistoryEntry> History { get; set; } = new();
    public Semester Semester { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var studentEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        Student = await _context.Students.FirstOrDefaultAsync(s => s.Email == studentEmail);

        if (Student == null)
        {
            return RedirectToPage("/Login");
        }

        SystemSetting? setting = await _context.SystemSettings.SingleOrDefaultAsync(e => e.SystemSettingID == 1);
        if (setting != null)
        {
            Semester = await _context.Semesters.SingleOrDefaultAsync(e => e.SemesterID == setting.CurrentSemester);
        }

        // Fetch enrollments for the student
        var enrollments = await _context.Enrollments
            .Include(e => e.Course)
            .Include(e => e.Semester)
            .Where(e => e.StudentID == Student.StudentID)
            .OrderByDescending(e => e.EnrollmentDate)
            .ToListAsync();

        // Process enrollment history
        History = enrollments
            .SelectMany(e =>
            {
                var historyEntries = new List<EnrollmentHistoryEntry>
                {
                    // Always show "Add" record when enrolled
                    new EnrollmentHistoryEntry
                    {
                        CourseCode = e.Course.CourseCode,
                        CourseName = e.Course.CourseName,
                        Semester = e.Semester?.Name,
                        Date = e.EnrollmentDate,
                        Action = "Add"
                    }
                };

                // If course was dropped, add a "Drop" record
                if (e.DropDate.HasValue)
                {
                    historyEntries.Add(new EnrollmentHistoryEntry
                    {
                        CourseCode = e.Course.CourseCode,
                        CourseName = e.Course.CourseName,
                        Semester = e.Semester?.Name,
                        Date = e.DropDate.Value,
                        Action = "Drop"
                    });
                }

                return historyEntries;
            })
            .OrderByDescending(e => e.Date)
            .ToList();

        return Page();
    }

    public class EnrollmentHistoryEntry
    {
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string Semester { get; set; }
        public DateTime Date { get; set; }
        public string Action { get; set; } // "Add" or "Drop"
    }
}
