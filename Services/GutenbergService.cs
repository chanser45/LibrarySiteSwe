using MyLibrarySite.Models;
using System.Text.Json;
using System.Net.Http;
namespace MyLibrarySite.Services
{using System.Collections.Generic;
using System.Linq;
    public class GutenbergService : IGutenbergService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;
        private const string baseUrl = "https://gutendex.com/books"; 

        public GutenbergService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true 
            };
        }
public async Task<List<Book>> GetPopularBooksAsync()
        {
            var response = await _httpClient.GetStringAsync("https://gutendex.com/books?sort=popular");
            var data = JsonSerializer.Deserialize<GutenbergResponse>(response);

            
            return data?.Results ?? new List<Book>();
        }
       

public async Task<Book> GetBookByIdAsync(int id)
    {
        
        return new Book
        {
            Id = id,
            Title = "Sample Book",
            Formats = new Dictionary<string, string>
            {
                { "text/html", $"https://www.gutenberg.org/files/{id}/{id}-h/{id}-h.htm" }
            }
        };
    }
        public async Task<List<Book>> SearchBooksAsync(string query)
        {
            var url = $"{baseUrl}?search={query}";
            var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            
            using var document = JsonDocument.Parse(content);
            var root = document.RootElement;
            var results = root.GetProperty("results");

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var books = JsonSerializer.Deserialize<List<Book>>(results.ToString(), options);
            return books ?? new List<Book>();
        }
    }
}