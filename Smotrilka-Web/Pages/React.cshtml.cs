using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Smotrilka_Web.Models;
using Smotrilka_Web.Services;

namespace Smotrilka_Web.Pages
{
    public class ReactModel : PageModel
    {
        private readonly BackendService _backendService;

        public ReactModel(BackendService backendService)
        {
            _backendService = backendService;
        }

        [BindProperty]
        public ReactionRequest ReactionRequest { get; set; }

        public string CurrentUser { get; set; }

        public void OnGet()
        {
            CurrentUser = Request.Cookies["CurrentUser"];
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ReactionRequest.Login = Request.Cookies["UserLogin"]!;
            ReactionRequest.Password = Request.Cookies["UserPassword"]!;

            var result = await _backendService.ReactAsync(ReactionRequest);

            if (result.Contains("added") || result.Contains("changed") || result.Contains("removed"))
            {
                return RedirectToPage("/Index");
            }

            ModelState.AddModelError(string.Empty, "Reaction failed");
            return Page();
        }
    }
}
