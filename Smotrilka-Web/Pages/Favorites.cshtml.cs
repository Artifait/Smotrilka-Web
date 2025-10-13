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
                    ErrorMessage = "������ ��� �������� ����������. ��������� ���� ������� ������.";
                    Favorites = new List<SearchResponse>();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"������ ��� �������� ����������: {ex.Message}";
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
                    ErrorMessage = "������ ��� �������� �� ����������";
                    return await OnGetAsync();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"������: {ex.Message}";
                return await OnGetAsync();
            }
        }

        private bool IsUserAuthenticated()
        {
            return Request.Cookies.ContainsKey("userLogin") && Request.Cookies.ContainsKey("userPassword");
        }
    }
}