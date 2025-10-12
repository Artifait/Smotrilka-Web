using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Smotrilka_Web.Models;
using Smotrilka_Web.Services;

namespace Smotrilka_Web.Pages
{
    public class AddLinkModel : PageModel
    {
        private readonly ApiService _apiService;
        private readonly StickerService _stickerService;

        public AddLinkModel(ApiService apiService, StickerService stickerService)
        {
            _apiService = apiService;
            _stickerService = stickerService;
        }

        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string Link { get; set; }

        [BindProperty]
        public string Tags { get; set; }

        [BindProperty]
        public string Description { get; set; }

        [BindProperty]
        public string StickersJson { get; set; }

        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        public void OnGet()
        {
            if (!IsUserAuthenticated())
            {
                Response.Redirect("/Login");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!IsUserAuthenticated())
            {
                return RedirectToPage("/Login");
            }

            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Link))
            {
                ErrorMessage = "Название и ссылка обязательны для заполнения";
                return Page();
            }

            try
            {
                var linkRequest = new LinkRequest
                {
                    Name = Name,
                    Link = Link,
                    Description = Description ?? "",
                    Tags = Tags?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                              .Select(t => t.Trim())
                              .ToList() ?? new List<string>()
                };

                var success = await _apiService.AddLinkAsync(linkRequest);

                if (success)
                {
                    SuccessMessage = "Ссылка успешно добавлена!";
                    // Очищаем форму
                    Name = string.Empty;
                    Link = string.Empty;
                    Tags = string.Empty;
                    Description = string.Empty;
                    StickersJson = string.Empty;

                    ModelState.Clear();
                }
                else
                {
                    ErrorMessage = "Ошибка при добавлении ссылки. Проверьте введенные данные.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Произошла ошибка: {ex.Message}";
            }

            return Page();
        }

        private bool IsUserAuthenticated()
        {
            return Request.Cookies.ContainsKey("userLogin") && Request.Cookies.ContainsKey("userPassword");
        }
    }
}