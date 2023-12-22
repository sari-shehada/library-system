using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Pages.Books.Models
{
    public class AddBookForm
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }

        [Required]
        public string ISBN { get; set; }

        public bool ValidateForm()
        {
            return !(Title is null || Author is null || ISBN is null);
        }
    }
}
