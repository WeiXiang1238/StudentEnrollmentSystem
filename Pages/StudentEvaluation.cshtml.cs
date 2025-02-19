using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentEnrollmentSystem.Database;
using StudentEnrollmentSystem.Database.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

public class StudentEvaluationModel : PageModel
{
    private readonly CoreContext _context;

    public StudentEvaluationModel(CoreContext context)
    {
        _context = context;
    }

    [BindProperty]
    [Required(ErrorMessage = "Please select a course.")]
    public int EnrollmentID { get; set; } // Now linked to Enrollment

    [BindProperty]
    [Required(ErrorMessage = "Please provide a rating between 1 and 5.")]
    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
    public int Rating { get; set; }

    [BindProperty]
    [StringLength(500, ErrorMessage = "Comments cannot exceed 500 characters.")]
    public string Comments { get; set; }

    public List<Enrollment> EnrolledCourses { get; set; } = new();
    public List<TeachingEvaluation> PreviousEvaluations { get; set; } = new();
    public string Message { get; set; }
    public Student Student { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var studentEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        Student = await _context.Students.FirstOrDefaultAsync(s => s.Email == studentEmail);

        if (Student == null)
        {
            return RedirectToPage("/Login");
        }

        // Get active enrollments
        var enrolledCourses = await _context.Enrollments
            .Include(e => e.Course)
            .Include(e => e.Semester)
            .Where(e => e.StudentID == Student.StudentID && e.Status == "Active")
            .ToListAsync();

        // Get evaluations that already exist
        var evaluatedEnrollmentIds = await _context.TeachingEvaluations
            .Where(te => te.Enrollment.StudentID == Student.StudentID)
            .Select(te => te.EnrollmentID)
            .ToListAsync();

        // Filter unenrolled courses that haven't been evaluated
        EnrolledCourses = enrolledCourses
            .Where(e => !evaluatedEnrollmentIds.Contains(e.EnrollmentID))
            .ToList();

        // Get previous evaluations
        PreviousEvaluations = await _context.TeachingEvaluations
            .Include(te => te.Enrollment.Course)
            .Include(te => te.Enrollment.Semester)
            .Where(te => te.Enrollment.StudentID == Student.StudentID)
            .OrderByDescending(te => te.SubmittedAt)
            .ToListAsync();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var studentEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        var student = await _context.Students.FirstOrDefaultAsync(s => s.Email == studentEmail);

        if (student == null)
        {
            return RedirectToPage("/Login");
        }

        var enrollment = await _context.Enrollments
            .Include(e => e.Course)
            .Include(e => e.Semester)
            .FirstOrDefaultAsync(e => e.EnrollmentID == EnrollmentID && e.StudentID == student.StudentID && e.Status == "Active");

        if (enrollment == null)
        {
            TempData["Message"] = "Invalid enrollment selection.";
            return RedirectToPage();
        }

        // Check if evaluation already exists
        var existingEvaluation = await _context.TeachingEvaluations
            .FirstOrDefaultAsync(te => te.EnrollmentID == EnrollmentID);

        if (existingEvaluation != null)
        {
            TempData["Message"] = "You have already evaluated this course.";
            return RedirectToPage();
        }

        var evaluation = new TeachingEvaluation
        {
            EnrollmentID = enrollment.EnrollmentID,
            Rating = Rating,
            Comments = Comments,
            SubmittedAt = DateTime.Now
        };

        _context.TeachingEvaluations.Add(evaluation);
        await _context.SaveChangesAsync();

        TempData["Message"] = "Evaluation submitted successfully!";
        return RedirectToPage();
    }
}
