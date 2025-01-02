using System.Text.Json.Serialization;

namespace MyLibrarySite.Models
{
    public class Book
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        
        [JsonPropertyName("authors")]
        public List<Author> Authors { get; set; } = new();

        [JsonPropertyName("formats")]
        public Dictionary<string, string> Formats { get; set; } = new();

        public int BookshelfItemId { get; set; }
    }

    public class Author
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}