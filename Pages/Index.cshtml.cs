using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentEnrollmentSystem.Database;
using StudentEnrollmentSystem.Database.Entity;
using System.Threading.Tasks;

public class IndexModel : PageModel
{
    private readonly CoreContext _context;

    public IndexModel(CoreContext context)
    {
        _context = context;
    }

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

        return Page();
    }
}
