using BusinessLogicLayer.Entities;
using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PresentationLayer.Pages.Login.Models;
using System.Security.Claims;

namespace PresentationLayer.Pages.Login
{
    public class LoginModel : PageModel
    {
        private readonly IAuthService _authService;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(IAuthService authService, ILogger<LoginModel> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [BindProperty]
        public LoginCredentials Credentials { get; set; }
        public void OnGet()
        {
        }


        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var identity = await _authService.LoginUser(Credentials.Username,Credentials.Password);

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("Cookies", principal);

                return RedirectToPage("/Index");
            }
            catch (InvalidCredentialsException)
            {
                ModelState.AddModelError(string.Empty, "Invalid credentials!");
                return Page();
            }

        }

    }
}
