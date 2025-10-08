using Smotrilka_Web.Models;

namespace Smotrilka_Web.Services
{
    public class BackendService
    {
        private readonly HttpClient _httpClient;

        public BackendService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> RegisterAsync(RegisterRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("/register", request);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddLinkAsync(LinkRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("/link", request);
            return response.IsSuccessStatusCode;
        }

        public async Task<string> ReactAsync(ReactionRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("/react", request);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<List<SearchResponse>> SearchLinksAsync(string query)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/search?q={Uri.EscapeDataString(query)}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<SearchResponse>>()
                           ?? new List<SearchResponse>();
                }

                // Логирование ошибки или возврат пустого списка
                Console.WriteLine($"Search error: {response.StatusCode}");
                return new List<SearchResponse>();
            }
            catch (Exception ex)
            {
                // Логирование исключения
                Console.WriteLine($"Search exception: {ex.Message}");
                return new List<SearchResponse>();
            }
        }
    }
}
