using Smotrilka_Web.Models;
using System.Text.Json;
using System.Text;

namespace Smotrilka_Web.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _httpClient.BaseAddress = new Uri("http://158.160.120.54:9090");
        }

        private (string login, string password) GetCredentials()
        {
            var context = _httpContextAccessor.HttpContext;
            var login = context.Request.Cookies["userLogin"];
            var password = context.Request.Cookies["userPassword"];
            return (login, password);
        }

        public async Task<bool> LoginAsync(string login, string password)
        {
            try
            {
                var response = await _httpClient.PostAsync($"/login?login={login}&password={password}", null);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RegisterAsync(RegisterRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/register", request);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<SearchResponse>> SearchAsync(string query)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/search?q={Uri.EscapeDataString(query)}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<SearchResponse>>(json) ?? new List<SearchResponse>();
                }
            }
            catch
            {
                // Log error
            }
            return new List<SearchResponse>();
        }

        public async Task<bool> AddLinkAsync(LinkRequest request)
        {
            try
            {
                var (login, password) = GetCredentials();
                var fullRequest = new LinkRequest
                {
                    Login = login,
                    Password = password,
                    Description = request.Description,
                    Name = request.Name,
                    Link = request.Link,
                    Tags = request.Tags
                };

                var response = await _httpClient.PostAsJsonAsync("/link", fullRequest);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }


        public async Task<bool> AddCommentAsync(CommentRequest request)
        {
            try
            {
                var (login, password) = GetCredentials();
                request.Login = login;
                request.Password = password;

                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("/comment", content);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddToFavoritesAsync(int linkId)
        {
            try
            {
                var (login, password) = GetCredentials();
                var response = await _httpClient.PostAsync($"/favorites/add?login={login}&password={password}&linkId={linkId}", null);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveFromFavoritesAsync(int linkId)
        {
            try
            {
                var (login, password) = GetCredentials();
                var response = await _httpClient.DeleteAsync($"/favorites/remove?login={login}&password={password}&linkId={linkId}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
        public async Task<LinkFullInfo> GetFullLinkInfoAsync(int linkId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/link/full?linkId={linkId}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<LinkFullInfo>(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting full link info: {ex.Message}");
            }
            return null;
        }

        public async Task<bool> AddCommentAsync(AddCommentRequest request)
        {
            try
            {
                var (login, password) = GetCredentials();
                var commentRequest = new CommentRequest
                {
                    Login = login,
                    Password = password,
                    LinkId = request.LinkId,
                    Text = request.Text
                };

                var json = JsonSerializer.Serialize(commentRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("/comment", content);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ReactAsync(ReactionRequest request)
        {
            try
            {
                var (login, password) = GetCredentials();
                var reactionRequest = new ReactionRequest
                {
                    Login = login,
                    Password = password,
                    LinkId = (long)request.LinkId,
                    Reaction = request.Reaction
                };

                var json = JsonSerializer.Serialize(reactionRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("/react", content);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<SearchResponse>> GetFavoritesAsync()
        {
            try
            {
                var (login, password) = GetCredentials();
                var response = await _httpClient.GetAsync($"/favorites?login={login}&password={password}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<SearchResponse>>(json) ?? new List<SearchResponse>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting favorites: {ex.Message}");
            }
            return new List<SearchResponse>();
        }
    }
}
