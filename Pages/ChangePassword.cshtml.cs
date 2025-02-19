using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentEnrollmentSystem.Database;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using StudentEnrollmentSystem.Database.Entity;

namespace StudentEnrollmentSystem.Pages
{
    public class ChangePasswordModel : PageModel
    {
        private readonly CoreContext _context;

        public ChangePasswordModel(CoreContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ChangePasswordInputModel Input { get; set; } = new();

        public string Message { get; set; } = string.Empty;
        public bool IsSuccess { get; set; } = false;
        public Student? Student { get; set; }

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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var studentEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            Student = await _context.Students.FirstOrDefaultAsync(s => s.Email == studentEmail);

            if (Student == null)
            {
                Message = "User not found.";
                return Page();
            }

            if (!BCrypt.Net.BCrypt.Verify(Input.CurrentPassword, Student.Password))
            {
                ModelState.AddModelError("Input.CurrentPassword", "Incorrect current password.");
                return Page();
            }

            Student.Password = BCrypt.Net.BCrypt.HashPassword(Input.NewPassword);
            await _context.SaveChangesAsync();

            IsSuccess = true;
            Message = "Password changed successfully!";
            return Page();
        }

        public class ChangePasswordInputModel
        {
            [Required(ErrorMessage = "Current password is required.")]
            public string CurrentPassword { get; set; }

            [Required(ErrorMessage = "New password is required.")]
            [MinLength(6, ErrorMessage = "New password must be at least 6 characters long.")]
            public string NewPassword { get; set; }

            [Required(ErrorMessage = "Confirm password is required.")]
            [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
            public string ConfirmPassword { get; set; }
        }
    }
}
