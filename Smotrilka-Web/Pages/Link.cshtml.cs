using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Smotrilka_Web.Services;
using Markdig;
using Smotrilka_Web.Models;

namespace Smotrilka_Web.Pages
{
    public class LinkModel : PageModel
    {
        private readonly ApiService _apiService;

        public LinkModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty]
        public AddCommentRequest CommentRequest { get; set; }

        public LinkFullInfo LinkInfo { get; set; }
        public string DescriptionHtml { get; set; }
        public bool IsUserAuthenticated { get; set; }
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            IsUserAuthenticated = Request.Cookies.ContainsKey("userLogin") &&
                                 Request.Cookies.ContainsKey("userPassword");

            LinkInfo = await _apiService.GetFullLinkInfoAsync(id);

            if (LinkInfo == null)
            {
                return Page();
            }

            // ������������ Markdown � HTML ��� ��������
            if (!string.IsNullOrEmpty(LinkInfo.Description))
            {
                DescriptionHtml = Markdown.ToHtml(LinkInfo.Description);
            }
            else
            {
                DescriptionHtml = "<p>�������� �����������.</p>";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!IsUserAuthenticated)
            {
                return RedirectToPage("/Login");
            }

            if (string.IsNullOrEmpty(CommentRequest.Text))
            {
                ErrorMessage = "����������� �� ����� ���� ������";
                return await OnGetAsync(CommentRequest.LinkId);
            }

            var success = await _apiService.AddCommentAsync(CommentRequest);

            if (success)
            {
                SuccessMessage = "����������� ������� ��������!";
                // ������� �����
                CommentRequest.Text = string.Empty;
                ModelState.Clear();
            }
            else
            {
                ErrorMessage = "������ ��� ���������� �����������. ��������� ��������� ������.";
            }

            // ������������� �������� ��� ���������� ������������
            return await OnGetAsync(CommentRequest.LinkId);
        }
    }
}