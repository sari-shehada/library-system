using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace PresentationLayer.Pages.Books
{
    [Authorize(Policy = "AdminsOnly")]
    public class IndexModel : PageModel
    {
        private readonly IBookService _bookService;

        public IndexModel(IBookService bookService)
        {
            _bookService = bookService;
        }
        public List<Book> Books { get; set; } = new List<Book>();
        public async Task OnGetAsync()
        {
            IEnumerable<Book> books = await _bookService.GetAllBooks();
            Books = books.ToList();
            return;
        }
    }
}
