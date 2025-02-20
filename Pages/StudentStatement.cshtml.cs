using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentEnrollmentSystem.Database;
using StudentEnrollmentSystem.Database.Entity;

public class StudentStatementModel : PageModel
{
    private readonly CoreContext _context;

    public StudentStatementModel(CoreContext context)
    {
        _context = context;
    }

    [BindProperty]
    public DateTime FromDate { get; set; } = DateTime.Now.AddMonths(-6);

    [BindProperty]
    public DateTime ToDate { get; set; } = DateTime.Now;

    public List<StatementEntry> StatementData { get; set; }

    public decimal TotalDue { get; set; }
    public decimal TotalPaid { get; set; }
    public Student Student { get; set; }
    public Semester Semester { get; set; }
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

        // Fetch all semesters first to process client-side
        var semesters = await _context.Semesters.ToListAsync();

        // Fetch enrollments
        var enrollments = await _context.Enrollments
            .Include(e => e.Course)
            .Include(e => e.Semester)
            .Where(e => e.StudentID == Student.StudentID && e.EnrollmentDate >= FromDate && e.EnrollmentDate <= ToDate.AddDays(1).AddTicks(-1))
            .Select(e => new StatementEntry
            {
                Date = e.EnrollmentDate.Date, // Ignore time
                Process = "ENROL",
                Particulars = "TUIT",
                DocumentNo = e.Course.CourseCode,
                Semester = e.Semester.Name,
                AmountDue = e.Course.Amount,
                AmountPaid = 0,
                TotalDuePaid = e.Course.Amount
            })
            .ToListAsync();

        // Fetch payments and assign semester dynamically
        var payments = await _context.Payments
            .Include(p => p.Student)
            .Where(p => p.StudentID == Student.StudentID && p.PaymentDate >= FromDate && p.PaymentDate <= ToDate.AddDays(1).AddTicks(-1))
            .ToListAsync(); // Fetch first, process in-memory

        var paymentEntries = payments.Select(p => new StatementEntry
        {
            Date = p.PaymentDate.Date, // Ignore time
            Process = "PAYMENT",
            Particulars = p.PaymentMethod,
            DocumentNo = $"PAY-{p.PaymentID}",
            Semester = semesters
                .FirstOrDefault(s => p.PaymentDate >= s.StartDate && p.PaymentDate <= s.EndDate)?.Name ?? "Unknown",
            AmountDue = 0,
            AmountPaid = p.Amount,
            TotalDuePaid = -p.Amount
        }).ToList();

        // **Group by Date (Ignoring Time), Process, and Semester**
        StatementData = enrollments.Concat(paymentEntries)
            .GroupBy(x => new { Date = x.Date.Date, x.Process, x.Semester }) // Fix: Use `Date.Date`
            .Select(g => new StatementEntry
            {
                Date = g.Key.Date,
                Process = g.Key.Process,
                Particulars = g.First().Particulars,
                DocumentNo = string.Join(", ", g.Select(x => x.DocumentNo)), // Merge documents
                Semester = g.Key.Semester,
                AmountDue = g.Sum(x => x.AmountDue),
                AmountPaid = g.Sum(x => x.AmountPaid),
                TotalDuePaid = g.Sum(x => x.TotalDuePaid)
            })
            .OrderBy(x => x.Date)
            .ToList();

        // **Calculate Totals**
        TotalDue = StatementData.Where(x => x.Process == "ENROL").Sum(x => x.AmountDue);
        TotalPaid = StatementData.Where(x => x.Process == "PAYMENT").Sum(x => x.AmountPaid);

        return Page();
    }



    public class StatementEntry
    {
        public DateTime Date { get; set; }
        public string Process { get; set; }
        public string Particulars { get; set; }
        public string DocumentNo { get; set; }
        public string Semester { get; set; }
        public decimal AmountDue { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal TotalDuePaid { get; set; }
    }
}
