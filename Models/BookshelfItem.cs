using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyLibrarySite.Models
{
    public class BookshelfItem
    {
        [Key]
        public int BookshelfItemId { get; set; }

        public int BookId { get; set; }

        [Required]
        public string UserId { get; set; }

        public string Title { get; set; }

        public string Authors { get; set; }

        public int LastPageRead { get; set; } = 0;
    }
}