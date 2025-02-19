using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentEnrollmentSystem.Database;
using StudentEnrollmentSystem.Database.Entity;
using System.Threading.Tasks;

namespace StudentEnrollmentSystem.Pages
{
    public class UpdateBankDetailModel : PageModel
    {
        private readonly CoreContext _context;

        public UpdateBankDetailModel(CoreContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Student Student { get; set; }

        public string Message { get; set; } = string.Empty;
        public bool IsSuccess { get; set; } = false;

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
            var studentEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var studentToUpdate = await _context.Students.FirstOrDefaultAsync(s => s.Email == studentEmail);

            if (studentToUpdate == null)
            {
                Message = "User not found.";
                return Page();
            }

            // Update only bank details
            studentToUpdate.Bank = Student.Bank;
            studentToUpdate.BankAccName = Student.BankAccName;
            studentToUpdate.BankAccNum = Student.BankAccNum;

            await _context.SaveChangesAsync();
            Student = await _context.Students.FirstOrDefaultAsync(s => s.Email == studentEmail);

            IsSuccess = true;
            Message = "Bank details updated successfully!";
            return Page();
        }
    }
}
