using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentEnrollmentSystem.Database;
using StudentEnrollmentSystem.Database.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ContactUsListModel : PageModel
{
    private readonly CoreContext _context;

    public ContactUsListModel(CoreContext context)
    {
        _context = context;
    }

    public List<ContactUs> ContactUsList { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string SearchEmail { get; set; } = string.Empty;

    [BindProperty(SupportsGet = true)]
    public string SearchSubject { get; set; } = string.Empty;

    [BindProperty(SupportsGet = true)]
    public int CurrentPage { get; set; } = 1;

    public int TotalPages { get; set; }

    private const int PageSize = 10; // Limit results per page
    public Student Student { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        Student = await _context.Students.FirstOrDefaultAsync(s => s.Email == email);
        Admin admin = await _context.Admins.FirstOrDefaultAsync(s => s.Email == email);

        if (Student == null && admin == null)
        {
            return RedirectToPage("/Login");
        }

        if (admin != null)
        {
            Student = new Student()
            {
                Email = admin.Email,
                FullName = admin.Name,
                StudentID = admin.AdminID
            };
        }

        var query = _context.ContactUs.AsQueryable();

        // Apply search filters
        if (!string.IsNullOrEmpty(SearchEmail))
        {
            query = query.Where(c => c.StudentEmail.Contains(SearchEmail));
        }

        if (!string.IsNullOrEmpty(SearchSubject))
        {
            query = query.Where(c => c.Subject.Contains(SearchSubject));
        }

        // Get total count for pagination
        int totalRecords = await query.CountAsync();
        TotalPages = (int)Math.Ceiling(totalRecords / (double)PageSize);

        // Fetch paginated results
        ContactUsList = await query
            .OrderByDescending(c => c.SubmittedAt)
            .Skip((CurrentPage - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();

        return Page();
    }
}
