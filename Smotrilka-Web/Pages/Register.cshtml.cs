using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Smotrilka_Web.Services;
using Smotrilka_Web.Models;

namespace Smotrilka_Web.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly BackendService _backendService;

        public RegisterModel(BackendService backendService)
        {
            _backendService = backendService;
        }

        [BindProperty]
        public RegisterRequest RegisterRequest { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var success = await _backendService.RegisterAsync(RegisterRequest);

            if (success)
            {
                Response.Cookies.Append("CurrentUser", RegisterRequest.Login);
                Response.Cookies.Append("UserLogin", RegisterRequest.Login);
                Response.Cookies.Append("UserPassword", RegisterRequest.Password);
                return RedirectToPage("/Index");
            }

            ModelState.AddModelError(string.Empty, "Registration failed");
            return Page();
        }
    }
}
