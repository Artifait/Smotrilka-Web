using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Smotrilka_Web.Models;
using Smotrilka_Web.Services;

namespace Smotrilka_Web.Pages
{
    public class AddLinkModel : PageModel
    {
        private readonly BackendService _backendService;

        public AddLinkModel(BackendService backendService)
        {
            _backendService = backendService;
        }

        [BindProperty]
        public LinkRequest LinkRequest { get; set; }

        public string CurrentUser { get; set; }

        public void OnGet()
        {
            CurrentUser = Request.Cookies["CurrentUser"];
            LinkRequest = new LinkRequest
            {
                Login = Request.Cookies["UserLogin"],
                Password = Request.Cookies["UserPassword"]
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var success = await _backendService.AddLinkAsync(LinkRequest);

            if (success)
            {
                return RedirectToPage("/Index");
            }

            ModelState.AddModelError(string.Empty, "Failed to add link");
            return Page();
        }
    }

}
