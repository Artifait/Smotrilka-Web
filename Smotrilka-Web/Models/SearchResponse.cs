namespace Smotrilka_Web.Models
{
    public class SearchResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public int Rating { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }
}
