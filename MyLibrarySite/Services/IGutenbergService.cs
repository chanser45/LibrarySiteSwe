using MyLibrarySite.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MyLibrarySite.Services
{
    public interface IGutenbergService
    {
        Task<List<Book>> SearchBooksAsync(string query);
        Task<List<Book>> GetPopularBooksAsync();
        
        Task<Book?> GetBookByIdAsync(int id);
    }
}