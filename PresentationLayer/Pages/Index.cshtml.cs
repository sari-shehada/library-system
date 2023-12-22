using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PresentationLayer.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost(string searchTerm, string searchType)
        {
            if (searchTerm is null || searchTerm == "")
            {
                return Page();
            }
            return RedirectToPage("/Search/Results", new { searchTerm, searchType });
        }
    }
}