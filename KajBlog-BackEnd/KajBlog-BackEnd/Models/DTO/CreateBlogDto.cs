﻿namespace KajBlog_BackEnd.Models.DTO
{
    public class CreateBlogDto
    {
        public string Category { get; set; }
        public string SubjectLine { get; set; }
        public string BlogBody { get; set; }
        public string GiphyPull { get; set; }
    }
}
