using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentEnrollmentSystem.Database;
using StudentEnrollmentSystem.Database.Entity;
using System.Collections.Generic;
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
    public int CourseID { get; set; }

    [BindProperty]
    public int Rating { get; set; }

    [BindProperty]
    public string Comments { get; set; }

    public List<Course> EnrolledCourses { get; set; }
    public List<TeachingEvaluation> PreviousEvaluations { get; set; }
    public string Message { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var studentEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        var student = await _context.Students.FirstOrDefaultAsync(s => s.Email == studentEmail);

        if (student == null)
        {
            return RedirectToPage("/Login");
        }

        // Get courses the student is enrolled in
        EnrolledCourses = await _context.Enrollments
            .Include(e => e.Course)
            .Where(e => e.StudentID == student.StudentID && e.Status == "Active")
            .Select(e => e.Course)
            .ToListAsync();

        // Get previous evaluations
        PreviousEvaluations = await _context.TeachingEvaluations
            .Include(te => te.Course)
            .Where(te => te.StudentID == student.StudentID)
            .ToListAsync();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var studentEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        var student = await _context.Students.FirstOrDefaultAsync(s => s.Email == studentEmail);

        if (student == null)
        {
            return RedirectToPage("/Login");
        }

        var evaluation = new TeachingEvaluation
        {
            StudentID = student.StudentID,
            CourseID = CourseID,
            Rating = Rating,
            Comments = Comments
        };

        _context.TeachingEvaluations.Add(evaluation);
        await _context.SaveChangesAsync();

        Message = "Evaluation submitted successfully!";
        return RedirectToPage();
    }
}
