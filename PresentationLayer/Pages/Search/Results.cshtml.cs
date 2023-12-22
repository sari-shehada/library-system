using BusinessLogicLayer.Entities;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using DataAccessLayer.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace PresentationLayer.Pages.Search
{
    [Authorize]
    public class ResultsModel : PageModel
    {
        private readonly IBookService _bookService;


        private readonly ILogger<ResultsModel> _logger;


        public ResultsModel(IBookService bookService, ILogger<ResultsModel> logger)
        {
            _bookService = bookService;
            _logger = logger;
        }
        public List<Book> SearchResults { get; set; } = new List<Book>();

        public async Task<IActionResult> OnPostAsync(string id)
        {
            Guid bookId;
            if (Guid.TryParse(id, out bookId))
            {

                Guid userId;
                if (Guid.TryParse(User.FindFirstValue("Id"), out userId))
                {
                    await _bookService.BorrowBook(userId, bookId);
                    return RedirectToPage("/MyBorrowedBooks/Index");
                    
                }

            }
            return Page();
        }

        public async Task OnGetAsync(string searchTerm, string searchType)
        {
            switch (searchType)
            {
                case "Title":
                    await SearchByTitle(searchTerm);
                    break;
                case "Author":
                    await SearchByAuthor(searchTerm);
                    break;
                case "ISBN":
                    await SearchByISBN(searchTerm);
                    break;
                default:
                    break;
            }
            return;
        }

        public async Task SearchByAuthor(string searchTerm)
        {
            IEnumerable<Book> books = await _bookService.SearchByAuthor(searchTerm);
            SearchResults = books.ToList();
        }
        public async Task SearchByTitle(string searchTerm)
        {
            IEnumerable<Book> books = await _bookService.SearchByTitle(searchTerm);
            SearchResults = books.ToList();
        }
        public async Task SearchByISBN(string searchTerm)
        {
            try
            {
                Book book = await _bookService.GetByISBN(searchTerm);
                SearchResults = new List<Book>().Append(book).ToList();
            }
            catch (NoResultException)
            {
                return;
            }
        }
    }
}
