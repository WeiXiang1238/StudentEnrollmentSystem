using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentEnrollmentSystem.Database;
using StudentEnrollmentSystem.Database.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class AddDropCourseModel : PageModel
{
    private readonly CoreContext _context;

    public AddDropCourseModel(CoreContext context)
    {
        _context = context;
    }

    [BindProperty]
    public int SelectedCourseID { get; set; }

    public Student Student { get; set; }
    public Semester CurrentSemester { get; set; }
    public List<Course> AvailableCourses { get; set; } = new();
    public List<Course> EnrolledCourses { get; set; } = new();
    public List<Enrollment> EnrollmentHistory { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        var studentEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        Student = await _context.Students.FirstOrDefaultAsync(s => s.Email == studentEmail);

        if (Student == null)
        {
            return RedirectToPage("/Login");
        }

        // Get current semester
        var systemSetting = await _context.SystemSettings.SingleOrDefaultAsync(s => s.SystemSettingID == 1);
        CurrentSemester = await _context.Semesters.FindAsync(systemSetting?.CurrentSemester);

        if (CurrentSemester == null)
        {
            TempData["Message"] = "No active semester found!";
            TempData["IsSuccess"] = false;
            return Page();
        }

        // Get available courses for the current semester
        AvailableCourses = await _context.Courses
            .Where(c => !_context.Enrollments.Any(e => e.StudentID == Student.StudentID && e.CourseID == c.CourseID && e.SemesterID == CurrentSemester.SemesterID && e.Status == "Active"))
            .ToListAsync();

        // Get enrolled courses
        EnrolledCourses = await _context.Enrollments
            .Include(e => e.Course)
            .Where(e => e.StudentID == Student.StudentID && e.SemesterID == CurrentSemester.SemesterID && e.Status == "Active")
            .Select(e => e.Course)
            .ToListAsync();

        // Get enrollment history
        EnrollmentHistory = await _context.Enrollments
            .Include(e => e.Course)
            .Where(e => e.StudentID == Student.StudentID && e.SemesterID == CurrentSemester.SemesterID)
            .ToListAsync();

        return Page();
    }

    public async Task<IActionResult> OnPostAddCourseAsync()
    {
        var studentEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        var student = await _context.Students.FirstOrDefaultAsync(s => s.Email == studentEmail);

        if (student == null)
        {
            TempData["Message"] = "User not found!";
            TempData["IsSuccess"] = false;
            return RedirectToPage();
        }

        var systemSetting = await _context.SystemSettings.SingleOrDefaultAsync(s => s.SystemSettingID == 1);
        var semester = await _context.Semesters.FindAsync(systemSetting?.CurrentSemester);

        if (semester == null)
        {
            TempData["Message"] = "No active semester found!";
            TempData["IsSuccess"] = false;
            return RedirectToPage();
        }

        var enrollment = new Enrollment
        {
            StudentID = student.StudentID,
            CourseID = SelectedCourseID,
            SemesterID = semester.SemesterID,
            EnrollmentDate = DateTime.Now,
            Status = "Active",
            Action = "Add"
        };

        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync();

        TempData["Message"] = "Course successfully added!";
        TempData["IsSuccess"] = true;

        return RedirectToPage("/Payment");
    }

    public async Task<IActionResult> OnPostDropCourseAsync()
    {
        var studentEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        var student = await _context.Students.FirstOrDefaultAsync(s => s.Email == studentEmail);

        if (student == null)
        {
            TempData["Message"] = "User not found!";
            TempData["IsSuccess"] = false;
            return RedirectToPage();
        }

        var enrollment = await _context.Enrollments
            .FirstOrDefaultAsync(e => e.StudentID == student.StudentID && e.CourseID == SelectedCourseID && e.Status == "Active");

        if (enrollment != null)
        {
            enrollment.Status = "Dropped";
            enrollment.DropDate = DateTime.Now;

            await _context.SaveChangesAsync();
            TempData["Message"] = "Course successfully dropped!";
            TempData["IsSuccess"] = true;
        }
        else
        {
            TempData["Message"] = "Enrollment not found!";
            TempData["IsSuccess"] = false;
        }

        return RedirectToPage();
    }
}
