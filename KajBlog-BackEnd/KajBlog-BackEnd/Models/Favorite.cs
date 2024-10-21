using System.ComponentModel.DataAnnotations;

namespace KajBlog_BackEnd.Models;

public class Favorite : BaseModel
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required]
    public int BlogId { get; set; }

    public Blog Blog { get; set; }

}
