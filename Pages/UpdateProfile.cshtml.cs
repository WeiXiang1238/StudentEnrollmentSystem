using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentEnrollmentSystem.Database;
using StudentEnrollmentSystem.Database.Entity;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace StudentEnrollmentSystem.Pages
{
    public class UpdateProfileModel : PageModel
    {
        private readonly CoreContext _context;

        public UpdateProfileModel(CoreContext context)
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

            // Update only the relevant fields
            studentToUpdate.FullName = Student.FullName;
            studentToUpdate.ContactNumber = Student.ContactNumber;
            studentToUpdate.Address1 = Student.Address1;
            studentToUpdate.Address2 = Student.Address2;
            studentToUpdate.PostCode = Student.PostCode;
            studentToUpdate.City = Student.City;
            studentToUpdate.State = Student.State;
            studentToUpdate.Country = Student.Country;
            studentToUpdate.EmergencyContactName = Student.EmergencyContactName;
            studentToUpdate.EmergencyContactRelationship = Student.EmergencyContactRelationship;
            studentToUpdate.EmergencyContactPhoneNumber = Student.EmergencyContactPhoneNumber;

            await _context.SaveChangesAsync();

            IsSuccess = true;
            Message = "Profile updated successfully!";
            return Page();
        }

    }
}