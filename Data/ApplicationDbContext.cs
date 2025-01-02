using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyLibrarySite.Models;

namespace MyLibrarySite.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<BookshelfItem> BookshelfItems { get; set; }
public DbSet<UpdateLastPageRequest> lastPageRequests { get; set; }
        
    }
}