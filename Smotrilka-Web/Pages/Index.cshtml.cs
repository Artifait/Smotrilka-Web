using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Smotrilka_Web.Models;
using Smotrilka_Web.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Smotrilka_Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly BackendService _backendService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(BackendService backendService, ILogger<IndexModel> logger)
        {
            _backendService = backendService;
            _logger = logger;
        }

        [BindProperty(SupportsGet = true)]
        public string Q { get; set; } = "";

        public List<SearchResponse> SearchResults { get; set; } = new List<SearchResponse>();
        public bool IsSearching { get; set; }
        public bool SearchError { get; set; }

        public async Task OnGetAsync()
        {
            if (!string.IsNullOrEmpty(Q))
            {
                await PerformSearch();
            }
        }

        private async Task PerformSearch()
        {
            IsSearching = true;
            SearchError = false;

            try
            {
                _logger.LogInformation("Performing search for query: {Query}", Q);

                // Âûחמג נואכüםמדמ סונגטסא
                SearchResults = await _backendService.SearchLinksAsync(Q);

                _logger.LogInformation("Search completed. Found {Count} results", SearchResults.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during search for query: {Query}", Q);
                SearchError = true;
                SearchResults = new List<SearchResponse>();
            }
            finally
            {
                IsSearching = false;
            }
        }
    }
}