using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PresentationLayer.Pages.Books.Models;

namespace PresentationLayer.Pages.Books
{
    [Authorize(Policy = "AdminsOnly")]
    public class AddBookModel : PageModel
    {
        private readonly IBookService _bookService;
        public AddBookModel(IBookService bookService)
        {
            _bookService = bookService;
        }
        [BindProperty]
        public AddBookForm Form { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!Form.ValidateForm())
            {
                return Page();
            }
            try
            {
                Book book = new Book()
                {
                    Title = Form.Title,
                    Author = Form.Author,
                    ISBN = Form.ISBN,
                };
                await _bookService.AddNewBook(book);
                return RedirectToPage("/Books/Index");
            }
            catch (InvalidISBNFormatException)
            {
                ModelState.AddModelError(string.Empty, "Invalid ISBN Format! ISBN Must consist of only numbers with a length of 13");
                return Page();
            }
        }
        public void OnGet()
        {
        }
    }
}
