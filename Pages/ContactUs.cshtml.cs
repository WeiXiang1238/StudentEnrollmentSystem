using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentEnrollmentSystem.Database;
using StudentEnrollmentSystem.Database.Entity;
using System.Threading.Tasks;

public class ContactUsModel : PageModel
{
    private readonly CoreContext _context;

    public ContactUsModel(CoreContext context)
    {
        _context = context;
    }

    [BindProperty]
    public string StudentEmail { get; set; }
    [BindProperty]
    public string Subject { get; set; }
    [BindProperty]
    public string Message { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        var inquiry = new ContactUs
        {
            StudentEmail = StudentEmail,
            Subject = Subject,
            Message = Message
        };

        _context.ContactUs.Add(inquiry);
        await _context.SaveChangesAsync();

        Message = "Your message has been sent successfully!";
        return Page();
    }
}
