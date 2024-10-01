using System.ComponentModel.DataAnnotations;

namespace KajBlog_BackEnd.Models;

public class Favorite
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int BlogId { get; set; }

}
