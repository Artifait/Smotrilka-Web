using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Smotrilka_Web.Models;
using Smotrilka_Web.Services;

namespace Smotrilka_Web.Pages
{
    public class FavoritesModel : PageModel
    {
        private readonly ApiService _apiService;

        public FavoritesModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        public List<SearchResponse> Favorites { get; set; } = new();
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!IsUserAuthenticated())
            {
                return RedirectToPage("/Login");
            }

            try
            {
                // В реальном приложении здесь нужно получить userId из кук или базы данных
                // Пока используем заглушку - нужно будет доработать после реализации пользователей на бэкенде
                var userId = 1; // Заглушка

                // В текущем API нет прямого метода для получения избранного по userId
                // Нужно доработать бэкенд или использовать другой подход
                // Временно показываем сообщение о необходимости доработки
                ErrorMessage = "Функционал избранного находится в разработке";

                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка при загрузке избранного: {ex.Message}";
                return Page();
            }
        }

        public async Task<IActionResult> OnPostRemoveFavoriteAsync(int linkId)
        {
            if (!IsUserAuthenticated())
            {
                return RedirectToPage("/Login");
            }

            try
            {
                var success = await _apiService.RemoveFromFavoritesAsync(linkId);
                if (success)
                {
                    return RedirectToPage();
                }
                else
                {
                    ErrorMessage = "Ошибка при удалении из избранного";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка: {ex.Message}";
                return Page();
            }
        }

        private bool IsUserAuthenticated()
        {
            return Request.Cookies.ContainsKey("userLogin") && Request.Cookies.ContainsKey("userPassword");
        }
    }
}