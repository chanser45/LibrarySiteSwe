using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MyLibrarySite.Data;
using MyLibrarySite.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyLibrarySite.Services;
namespace MyLibrarySite.Controllers
{
    [Authorize] 
    public class BookshelfController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IGutenbergService _gutenbergService;
        public BookshelfController(ApplicationDbContext context, IGutenbergService gutenbergService)
        {
            _context = context;
            _gutenbergService = gutenbergService;
        }

        
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var bookshelfItems = await _context.BookshelfItems
    .Where(b => b.UserId == userId)
    .OrderByDescending(b => b.LastPageRead)
    .ToListAsync();

            return View(bookshelfItems);
        }

        
        [HttpPost]
        public async Task<IActionResult> Add(int bookId, string title, string authors)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            
            var existing = await _context.BookshelfItems
                .FirstOrDefaultAsync(b => b.BookId == bookId && b.UserId == userId);

            if (existing == null)
            {
                var newItem = new BookshelfItem
                {
                    BookId = bookId,
                    Title = title,
                    Authors = authors,
                    UserId = userId,
                    LastPageRead = 0
                };

                _context.BookshelfItems.Add(newItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        
        [HttpPost]
        public async Task<IActionResult> Remove(int bookshelfItemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var item = await _context.BookshelfItems
                .FirstOrDefaultAsync(b => b.BookshelfItemId == bookshelfItemId && b.UserId == userId);

            if (item != null)
            {
                _context.BookshelfItems.Remove(item);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

      
        [HttpPost]
        public async Task<IActionResult> UpdateLastPage(int bookshelfItemId, int lastPage)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var item = await _context.BookshelfItems
                .FirstOrDefaultAsync(b => b.BookshelfItemId == bookshelfItemId && b.UserId == userId);

            if (item != null)
            {
                item.LastPageRead = lastPage;
                _context.BookshelfItems.Update(item);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
[HttpPost]
public async Task<IActionResult> UpdateLastPageOnExit([FromBody] UpdateLastPageRequest request)
{
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

    if (request == null || request.BookId <= 0 || request.LastPage < 0)
        return BadRequest(new { success = false, message = "Invalid data" });

    var book = await _context.BookshelfItems
        .FirstOrDefaultAsync(b => b.BookId == request.BookId && b.UserId == userId);

    if (book != null)
    {
        book.LastPageRead = request.LastPage;
        _context.BookshelfItems.Update(book);
        await _context.SaveChangesAsync();
        return Ok(new { success = true });
    }

    return BadRequest(new { success = false, message = "Book not found" });
}

        [HttpGet]
public async Task<IActionResult> Read(int id)
{
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

    var book = await _context.BookshelfItems
        .FirstOrDefaultAsync(b => b.BookId == id && b.UserId == userId);

    if (book == null)
    {
        return NotFound();
    }

    var bookDetails = await _gutenbergService.GetBookByIdAsync(id);
    if (bookDetails != null)
    {
        bookDetails.BookshelfItemId = book.BookshelfItemId; 
        return View(bookDetails);
    }

    return NotFound();
}

    }
}