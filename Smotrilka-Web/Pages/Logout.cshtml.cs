using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Smotrilka_Web.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnPost()
        {
            Response.Cookies.Delete("userLogin");
            Response.Cookies.Delete("userPassword");
            return RedirectToPage("/Index");
        }
        public IActionResult OnGet()
        {
            {
                Response.Cookies.Delete("userLogin");
                Response.Cookies.Delete("userPassword");
                return RedirectToPage("/Index");
            }
        }
    }
}