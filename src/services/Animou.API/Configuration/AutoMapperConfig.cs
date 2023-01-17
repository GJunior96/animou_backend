using Animou.API.DTOs;
using Animou.Business.Models;
using AutoMapper;

namespace Animou.API.Configuration
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<Comment, CommentDTO>().ReverseMap();
            CreateMap<Like, LikeDislikeDTO>().ReverseMap();
            CreateMap<Dislike, LikeDislikeDTO>().ReverseMap();
        }
    }
}
