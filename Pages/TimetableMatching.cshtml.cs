using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentEnrollmentSystem.Database;
using StudentEnrollmentSystem.Database.Entity;
using StudentEnrollmentSystem.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class TimetableMatchingModel : PageModel
{
    private readonly CoreContext _context;

    public TimetableMatchingModel(CoreContext context)
    {
        _context = context;
    }

    [BindProperty]
    public int SelectedCourseID { get; set; }

    [BindProperty]
    public string SelectedDay { get; set; }

    [BindProperty]
    public string SelectedStartTime { get; set; }

    [BindProperty]
    public string SelectedEndTime { get; set; }
    public Student? Student { get; set; }
    public List<Course> AvailableCourses { get; set; } = new List<Course>();
    public List<Course> SelectedCourses { get; set; } = new List<Course>();
    public List<Course> MatchedTimetable { get; set; } = new List<Course>();
    public List<(string Day, string StartTime, string EndTime)> NonAvailableTimes { get; set; } = new();
    public bool ShowTimetable { get; set; } = false;
    public Semester Semester { get; set; }

    public async Task<IActionResult> OnGetAsync(bool clearSession = false, bool showTimetable = false)
    {
        if (clearSession)
        {
            HttpContext.Session.Remove("SelectedCourses");
            HttpContext.Session.Remove("NonAvailableTimes");
        }

        ShowTimetable = showTimetable;
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

        // Get all available courses
        AvailableCourses = await _context.Enrollments
            .Include(e => e.Course)
            .Where(e => e.StudentID == Student.StudentID && e.Status == "Active")
            .Select(e => e.Course)
            .ToListAsync();

        if (HttpContext.Session.GetObject<List<Course>>("SelectedCourses") == null)
        {
            SelectedCourses = AvailableCourses;
            HttpContext.Session.SetObject("SelectedCourses", SelectedCourses);
        }
        else
        {
            SelectedCourses = HttpContext.Session.GetObject<List<Course>>("SelectedCourses");

        }
        NonAvailableTimes = HttpContext.Session.GetObject<List<(string, string, string)>>("NonAvailableTimes") ?? new List<(string, string, string)>();

        MatchedTimetable = SelectedCourses
        .Where(course => !NonAvailableTimes.Any(nt =>
            nt.Item1 == course.Day &&
            TimeSpan.Parse(nt.Item2) <= course.StartTime &&
            TimeSpan.Parse(nt.Item3) > course.StartTime)) // Compare using TimeSpan
        .ToList();

        return Page();
    }

    public IActionResult OnGetAA()
    {
        HttpContext.Session.Remove("SelectedCourses");
        HttpContext.Session.Remove("NonAvailableTimes");
        return RedirectToPage(new { clearSession = true } );
    }

    public IActionResult OnPostAddCourse()
    {
        // Get the selected course
        var course = _context.Courses.FirstOrDefault(c => c.CourseID == SelectedCourseID);
        if (course != null)
        {
            var selectedCourses = HttpContext.Session.GetObject<List<Course>>("SelectedCourses") ?? new List<Course>();
            if (!selectedCourses.Any(c => c.CourseID == SelectedCourseID))
            {
                selectedCourses.Add(course);
                HttpContext.Session.SetObject("SelectedCourses", selectedCourses);
            }
        }

        return RedirectToPage();
    }

    public IActionResult OnPostRemoveCourse(int courseId)
    {
        var selectedCourses = HttpContext.Session.GetObject<List<Course>>("SelectedCourses") ?? new List<Course>();

        // Remove the selected course
        selectedCourses.RemoveAll(c => c.CourseID == courseId);

        // Update session
        HttpContext.Session.SetObject("SelectedCourses", selectedCourses);

        return RedirectToPage();
    }

    public IActionResult OnPostRemoveTime(string day, string startTime, string endTime)
    {
        var nonAvailableTimes = HttpContext.Session.GetObject<List<(string, string, string)>>("NonAvailableTimes") ?? new List<(string, string, string)>();

        // Remove the matching time entry
        nonAvailableTimes.RemoveAll(nt => nt.Item1 == day && nt.Item2 == startTime && nt.Item3 == endTime);

        // Update session
        HttpContext.Session.SetObject("NonAvailableTimes", nonAvailableTimes);

        return RedirectToPage();
    }

    public IActionResult OnPostAddTime()
    {
        // Store non-available time in the session or a list (not in DB)
        if (!string.IsNullOrEmpty(SelectedDay) && !string.IsNullOrEmpty(SelectedStartTime) && !string.IsNullOrEmpty(SelectedEndTime))
        {
            if (HttpContext.Session.GetObject<List<(string, string, string)>>("NonAvailableTimes") == null)
            {
                HttpContext.Session.SetObject("NonAvailableTimes", new List<(string, string, string)>());
            }
            string formattedStart = TimeSpan.Parse(SelectedStartTime).ToString(@"hh\:mm");
            string formattedEnd = TimeSpan.Parse(SelectedEndTime).ToString(@"hh\:mm");
            var nonAvailableTimes = HttpContext.Session.GetObject<List<(string, string, string)>>("NonAvailableTimes");
            nonAvailableTimes.Add((SelectedDay, SelectedStartTime, SelectedEndTime));
            HttpContext.Session.SetObject("NonAvailableTimes", nonAvailableTimes);
        }

        return RedirectToPage();
    }

    public IActionResult OnPostGenerateTimetable()
    {
        return RedirectToPage(new { showTimetable = true });
    }

    public IActionResult OnPostBack()
    {
        return RedirectToPage(new { showTimetable = false });
    }
}
