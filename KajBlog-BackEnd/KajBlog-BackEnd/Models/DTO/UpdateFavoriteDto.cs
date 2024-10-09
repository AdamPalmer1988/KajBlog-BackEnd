namespace KajBlog_BackEnd.Models.DTO;

public class UpdateFavoriteDto
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int BlogId { get; set; }
}
