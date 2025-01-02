using Microsoft.AspNetCore.Mvc;
using MyLibrarySite.Services;
using MyLibrarySite.Models;
using System.Threading.Tasks;
using System.Linq;

using System.Net.Http;
using System.Text.Json;
namespace MyLibrarySite.Controllers
{
    public class HomeController : Controller
    {
       
        private readonly IGutenbergService _gutenbergService;

        public HomeController(IGutenbergService gutenbergService)
        {
            _gutenbergService = gutenbergService;
        }

        public async Task<IActionResult> Index()
{
    var popularBooks = await _gutenbergService.GetPopularBooksAsync();  
    ViewBag.PopularBooks = popularBooks; 
    return View();
}

        [HttpPost]
        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return RedirectToAction("Index");
            }

            var books = await _gutenbergService.SearchBooksAsync(query);
            return View("SearchResult", books);
        }
    }
}