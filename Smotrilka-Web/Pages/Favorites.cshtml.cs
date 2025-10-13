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
                Favorites = await _apiService.GetFavoritesAsync();

                if (Favorites == null)
                {
                    ErrorMessage = "Ошибка при загрузке избранного. Проверьте ваши учетные данные.";
                    Favorites = new List<SearchResponse>();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка при загрузке избранного: {ex.Message}";
                Favorites = new List<SearchResponse>();
            }

            return Page();
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
                    return await OnGetAsync();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка: {ex.Message}";
                return await OnGetAsync();
            }
        }

        private bool IsUserAuthenticated()
        {
            return Request.Cookies.ContainsKey("userLogin") && Request.Cookies.ContainsKey("userPassword");
        }
    }
}