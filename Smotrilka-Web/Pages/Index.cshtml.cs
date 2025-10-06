using Microsoft.AspNetCore.Mvc.RazorPages;
using Smotrilka_Web.Services;

namespace Smotrilka_Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly BackendService _backendService;

        public IndexModel(BackendService backendService)
        {
            _backendService = backendService;
        }

        public string CurrentUser { get; set; }

        public void OnGet()
        {
            CurrentUser = Request.Cookies["CurrentUser"];
        }
    }
}
