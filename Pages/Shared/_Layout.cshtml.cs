using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentEnrollmentSystem.Database.Entity;
using StudentEnrollmentSystem.Enums;

namespace StudentEnrollmentSystem.Pages
{
    public class LayoutModel : PageModel
    {
        private readonly ILogger<LayoutModel> _logger;

        public LayoutModel(ILogger<LayoutModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
