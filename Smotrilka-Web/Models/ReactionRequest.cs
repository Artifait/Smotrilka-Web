namespace Smotrilka_Web.Models
{
    public class ReactionRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public long LinkId { get; set; }
        public int Reaction { get; set; }
    }
}
