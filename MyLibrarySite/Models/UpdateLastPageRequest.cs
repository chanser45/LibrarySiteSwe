namespace MyLibrarySite.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class UpdateLastPageRequest
{
    [Key]
    public int pageId { get; set; }
    public int BookId { get; set; }
    public int LastPage { get; set; }
}
