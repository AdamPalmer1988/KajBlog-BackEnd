using System.ComponentModel.DataAnnotations;
namespace KajBlog_BackEnd.Models
{
    public class Blog : BaseModel
    {
        [Key]
        public int BlogId { get; set; }

        public string UserId { get; set; }

        public string Category { get; set; }
        public string SubjectLine { get; set; }
        public string BlogBody { get; set; }
        public string GiphyPull { get; set; }
        public List<Favorite> Favorites { get; set; }
    }
}
