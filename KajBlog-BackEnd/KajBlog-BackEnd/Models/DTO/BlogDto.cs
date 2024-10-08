namespace KajBlog_BackEnd.Models.DTO;

public class BlogDto
{
    public int BlogId { get; set; }
    public string UserId { get; set; }
    public string Category { get; set; }
    public string SubjectLine { get; set; }
    public string BlogBody { get; set; }
    public string GiphyPull { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow; 
}
