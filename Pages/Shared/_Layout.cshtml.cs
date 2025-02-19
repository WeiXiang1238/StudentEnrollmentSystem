using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentEnrollmentSystem.Database;
using StudentEnrollmentSystem.Database.Entity;

namespace StudentEnrollmentSystem.Pages
{
    public class LayoutModel : PageModel
    {
        private readonly CoreContext _context;

        public LayoutModel(CoreContext context)
        {
            _context = context;
        }

        public Student Student { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var studentEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            Student = await _context.Students.FirstOrDefaultAsync(s => s.Email == studentEmail);

            if (Student == null)
            {
                return RedirectToPage("/Login");
            }

            return Page();
        }
    }
}
