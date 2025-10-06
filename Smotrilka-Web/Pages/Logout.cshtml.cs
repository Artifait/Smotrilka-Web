using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace Smotrilka_Web.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            Response.Cookies.Delete("CurrentUser");
            Response.Cookies.Delete("UserLogin");
            Response.Cookies.Delete("UserPassword");
            return RedirectToPage("/Index");
        }
        public IActionResult OnPost()
        {
            Response.Cookies.Delete("CurrentUser");
            Response.Cookies.Delete("UserLogin");
            Response.Cookies.Delete("UserPassword");
            return RedirectToPage("/Index");
        }
    }
}