using System.Text.Json.Serialization;
namespace MyLibrarySite.Models;
 public class GutenbergResponse
{
    [JsonPropertyName("results")]
    public List<Book> Results { get; set; } = new();
}
    public class GutenbergBook
    {
        public int id { get; set; }
        public string title { get; set; }
        public List<GutenbergAuthor> authors { get; set; }
    }

    public class GutenbergAuthor
    {
        public string name { get; set; }
    }

    