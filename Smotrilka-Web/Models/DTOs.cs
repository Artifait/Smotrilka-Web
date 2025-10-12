namespace Smotrilka_Web.Models
{
    public class LoginRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class RegisterRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class LinkRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public List<string> Tags { get; set; } = new();
        public string Description { get; set; }
    }

    public class CommentRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public int LinkId { get; set; }
        public string Text { get; set; }
    }

    public class ReactionRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public long LinkId { get; set; }
        public int Reaction { get; set; }
    }

    public class SearchResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public int Rating { get; set; }
        public List<string> Tags { get; set; } = new();
    }

    public class LinkViewModel : SearchResponse
    {
        public string Description { get; set; }
        public List<StickerViewModel> Stickers { get; set; } = new();
        public List<CommentViewModel> Comments { get; set; } = new();
        public bool IsFavorite { get; set; }
    }

    public class CommentViewModel
    {
        public string Author { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class StickerViewModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string DisplayValue { get; set; }
        public string Tooltip { get; set; }
    }
}
