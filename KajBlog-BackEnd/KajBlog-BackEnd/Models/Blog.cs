using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KajBlog_BackEnd.Models
{
    public class Blog
    {
        [Key]
        public int BlogID { get; set; }
        public string UserID { get; set; } 
        public string Category { get; set; }
        public string SubjectLine { get; set; }
        public string BlogBody { get; set; }
        public DateTime Timestamp { get; set; }
        public string GiphyPull { get; set; } 


    }
}
