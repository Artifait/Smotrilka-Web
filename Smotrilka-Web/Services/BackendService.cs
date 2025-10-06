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
    }
}
