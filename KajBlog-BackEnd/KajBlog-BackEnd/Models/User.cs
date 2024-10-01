using System.ComponentModel.DataAnnotations;

namespace KajBlog_BackEnd.Models;

public class User
{
    [Key]
    public int UserId { get; set; }

    [Required]
    public string UserName { get; set; }
}
