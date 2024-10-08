using AutoMapper;
using KajBlog_BackEnd.Models.DTO;

namespace KajBlog_BackEnd.Models.Profiles;

public class BlogProfile : Profile
{
    public BlogProfile()
    {
        CreateMap<BlogDto, Blog>();
        CreateMap<UpdateBlogDto, Blog>();
        CreateMap<CreateBlogDto, Blog>();
        CreateMap<Blog, BlogDto>();
    }
}
