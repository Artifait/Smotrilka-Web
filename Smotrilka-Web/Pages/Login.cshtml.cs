using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Smotrilka_Web.Models;
using Smotrilka_Web.Services;

namespace Smotrilka_Web.Pages
{
    public class LoginModel : PageModel
    {
        private readonly BackendService _backendService;

        public LoginModel(BackendService backendService)
        {
            _backendService = backendService;
        }

        [BindProperty]
        public RegisterRequest LoginRequest { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var testRequest = new LinkRequest
            {
                Login = LoginRequest.Login,
                Password = LoginRequest.Password,
                Name = "test",
                Type = "test",
                Link = "http://test.com"
            };

            var success = await _backendService.AddLinkAsync(testRequest);

            if (success)
            {
                Response.Cookies.Append("CurrentUser", LoginRequest.Login);
                Response.Cookies.Append("UserLogin", LoginRequest.Login);
                Response.Cookies.Append("UserPassword", LoginRequest.Password);
                return RedirectToPage("/Index");
            }

            ModelState.AddModelError(string.Empty, "Invalid login or password");
            return Page();
        }
    }
}
