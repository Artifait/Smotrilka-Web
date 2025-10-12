using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Smotrilka_Web.Services;

namespace Smotrilka_Web.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly ApiService _apiService;

        public RegisterModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty]
        public string Login { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public string ConfirmPassword { get; set; }

        public string LoginError { get; set; }
        public string PasswordError { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Password != ConfirmPassword)
            {
                PasswordError = "Пароли не совпадают";
                return Page();
            }

            var request = new Models.RegisterRequest
            {
                Login = Login,
                Password = Password,
                ConfirmPassword = ConfirmPassword
            };

            var success = await _apiService.RegisterAsync(request);
            if (success)
            {
                Response.Cookies.Append("userLogin", Login);
                Response.Cookies.Append("userPassword", Password);
                return RedirectToPage("/Index");
            }

            LoginError = "Пользователь с таким логином уже существует";
            return Page();
        }
    }
}
