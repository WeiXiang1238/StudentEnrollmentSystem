using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentEnrollmentSystem.Database;
using StudentEnrollmentSystem.Database.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

public class ContactUsModel : PageModel
{
    private readonly CoreContext _context;

    public ContactUsModel(CoreContext context)
    {
        _context = context;
    }

    [BindProperty]
    [Required(ErrorMessage = "Please select a category.")]
    public string SelectedCategory { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Subject is required.")]
    [StringLength(100, ErrorMessage = "Subject cannot exceed 100 characters.")]
    public string Subject { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Message is required.")]
    [StringLength(500, ErrorMessage = "Message cannot exceed 500 characters.")]
    public string Message { get; set; }

    public Student Student { get; set; }

    public List<string> Categories { get; set; } = new()
    {
        "General Inquiry",
        "Technical Support",
        "Billing Issue",
        "Course Enrollment",
        "Others"
    };

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
            return Page();  // Return to the form if validation fails
        }

        var studentEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        Student = await _context.Students.FirstOrDefaultAsync(s => s.Email == studentEmail);

        if (Student == null)
        {
            return RedirectToPage("/Login");
        }

        var inquiry = new ContactUs
        {
            Category = SelectedCategory,
            StudentEmail = Student.Email,
            Subject = Subject,
            Message = Message,
            SubmittedAt = DateTime.Now
        };

        _context.ContactUs.Add(inquiry);
        await _context.SaveChangesAsync();

        TempData["Message"] = "Your message has been sent successfully!";
        return RedirectToPage();
    }
}
