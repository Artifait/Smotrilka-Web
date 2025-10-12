using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Smotrilka_Web.Services;

namespace Smotrilka_Web.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ApiService _apiService;

        public LoginModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty]
        public string Login { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string LoginError { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Password))
            {
                LoginError = "Заполните все поля";
                return Page();
            }

            var success = await _apiService.LoginAsync(Login, Password);
            if (success)
            {
                Response.Cookies.Append("userLogin", Login);
                Response.Cookies.Append("userPassword", Password);
                return RedirectToPage("/Index");
            }

            LoginError = "Неверный логин или пароль";
            return Page();
        }
    }
}
