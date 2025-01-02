using Microsoft.AspNetCore.Mvc;
using MyLibrarySite.Models;
using MyLibrarySite.Services;
using System.Linq;
using System.Threading.Tasks;

namespace MyLibrarySite.Controllers
{
    public class BookController : Controller
    {
        private readonly IGutenbergService _gutenbergService;

        public BookController(IGutenbergService gutenbergService)
        {
            _gutenbergService = gutenbergService;
        }

        public async Task<IActionResult> Read(int id)
        {
            var book = await _gutenbergService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound("Kitap bulunamadı.");
            }

           
            if (book.Formats.TryGetValue("text/html", out string? htmlUrl) ||
                book.Formats.TryGetValue("text/plain", out htmlUrl))
            {
                ViewBag.HtmlUrl = htmlUrl;
                return View(book);
            }

            return Content("Bu kitap okunabilir bir formatta mevcut değil.");
        }
    }
}
