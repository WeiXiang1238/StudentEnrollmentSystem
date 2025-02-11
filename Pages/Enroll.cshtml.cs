using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentEnrollmentSystem.Database;
using StudentEnrollmentSystem.Database.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

public class EnrollModel : PageModel
{
    private readonly CoreContext _context;

    public EnrollModel(CoreContext context)
    {
        _context = context;
    }

    [BindProperty]
    public string SelectedCoursesJson { get; set; }

    public Student? Student { get; set; }
    public Semester? Semester { get; set; }
    public List<Course> AvailableCourses { get; set; } = new List<Course>();

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

        AvailableCourses = await _context.Courses.ToListAsync();

        return Page();
    }

    [HttpPost]
    public async Task<IActionResult> OnPostAsync([FromBody] List<Course> selectedCourses)
    {
        try
        {
            if(selectedCourses == null)
            {
                return new JsonResult(new { success = false, message = "Error" });
            }
            var studentEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            Student = await _context.Students.FirstOrDefaultAsync(s => s.Email == studentEmail);
            SystemSetting? setting = await _context.SystemSettings.SingleOrDefaultAsync(e => e.SystemSettingID == 1);
            if (setting != null)
            {
                Semester = await _context.Semesters.SingleOrDefaultAsync(e => e.SemesterID == setting.CurrentSemester);
            }

            if (Student == null)
            {
                return new JsonResult(new { success = false, message = "User not found." });
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            foreach (var course in selectedCourses)
            {
                var enrollment = new Enrollment
                {
                    StudentID = Student.StudentID,
                    CourseID = course.CourseID,
                    SemesterID = Semester.SemesterID,
                    EnrollmentDate = DateTime.Now,
                    PaymentDate = null,
                    Status = "Active"
                };
                _context.Enrollments.Add(enrollment);
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return new JsonResult(new { success = true, message = "Enrollment successful!" });
        }
        catch (Exception ex)
        {
            return new JsonResult(new { success = false, message = "Error: " + ex.Message });
        }
    }
}