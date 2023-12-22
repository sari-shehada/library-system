using BusinessLogicLayer.Entities;
using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace PresentationLayer.Pages.MyBorrowedBooks
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IAuthService _authService;
        private readonly IBookService _bookService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IAuthService authService, IBookService bookService, ILogger<IndexModel> logger)
        {
            _authService = authService;
            _bookService = bookService;
            _logger = logger;
        }


        public List<BorrowedBookDTO> BorrowedBooks { get; set; } = new List<BorrowedBookDTO>();


        public async Task<IActionResult> OnGetAsync()
        {
            Guid userId;
            if(Guid.TryParse(User.FindFirstValue("Id"),out userId))
            {
                try
                {
                IEnumerable<BorrowedBookDTO> result = await _bookService.GetUserBorrowedBooks(userId);
                BorrowedBooks = result.ToList();
                return Page();
                }
                //TODO: 
                catch(Exception)
                {
                    return RedirectToPage("/Logout");
                }
            }
            return RedirectToPage("/Logout");
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            Guid borrowingId;
            if (Guid.TryParse(id, out borrowingId))
            {
                await _bookService.ReturnBook(borrowingId);
            }
            return RedirectToPage();
        }
    }
}
