using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Pages.Login.Models
{
    public class LoginCredentials
    {
        [Required]
        public string Username { get; set; } = string.Empty;


        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
