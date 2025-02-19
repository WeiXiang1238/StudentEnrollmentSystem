using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentEnrollmentSystem.Database;
using StudentEnrollmentSystem.Enums;
using StudentEnrollmentSystem.Extensions;
using StudentEnrollmentSystem.Database.Entity;

namespace StudentEnrollmentSystem.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly CoreContext _context;

        public LoginModel(ILogger<IndexModel> logger, CoreContext context)
        {
            _logger = logger;
            _context = context;
        }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }
        [BindProperty] 
        public int UserType { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "Email and Password are required.";
                return Page();
            }

            if (UserType == Role.Student.GetValue())
            {
                Student student = await _context.Students.FirstOrDefaultAsync(s => s.Email == Email);

                if (student == null)
                {
                    ErrorMessage = "Invalid credentials.";
                    return Page();
                }

                if (!BCrypt.Net.BCrypt.Verify(Password, student.Password))
                {
                    ErrorMessage = "Invalid credentials.";
                    return Page();
                }
            }
            else if (UserType == Role.Admin.GetValue())
            {
                Admin admin = await _context.Admins.FirstOrDefaultAsync(a => a.Email == Email);

                if (admin == null)
                {
                    ErrorMessage = "Invalid credentials.";
                    return Page();
                }

                if (!BCrypt.Net.BCrypt.Verify(Password, admin.Password))
                {
                    ErrorMessage = "Invalid credentials.";
                    return Page();
                }
            }

            HttpContext.Session.Clear();
            HttpContext.Session.SetString("UserEmail", Email);
            HttpContext.Session.SetString("UserRole", UserType.ToString());
            HttpContext.Response.Cookies.Delete(".AspNetCore.Session");

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, Email),
            new Claim(ClaimTypes.Role, UserType.ToString())
        };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            return RedirectToPage(UserType == Role.Admin.GetValue() ? "/Index" : "/Index");
        }
    }
}
