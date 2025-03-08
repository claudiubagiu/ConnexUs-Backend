using AutoMapper;
using Backend.API.Models.Domain;
using Backend.API.Models.DTOs;

namespace Backend.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<RegisterRequestDto, ApplicationUser>().ReverseMap();
            CreateMap<AddPostRequestDto, Post>().ReverseMap();
            CreateMap<Post, PostDto>().ReverseMap()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));
            CreateMap<ApplicationUser, ApplicationUserDto>().ReverseMap();
            CreateMap<UpdatePostRequestDto, Post>().ReverseMap();
            CreateMap<Comment, CommentDto>().ReverseMap()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.Post, opt => opt.MapFrom(src => src.Post));
            CreateMap<Comment, AddCommentRequestDto>().ReverseMap();
            CreateMap<Comment, UpdateCommentRequestDto>().ReverseMap();
            CreateMap<ImageUploadRequestDto, Image>()
            .ForMember(dest => dest.FileExtension, opt => opt.MapFrom(src => Path.GetExtension(src.File.FileName)))
            .ForMember(dest => dest.FileSizeInBytes, opt => opt.MapFrom(src => src.File.Length))
            .ForMember(dest => dest.FilePath, opt => opt.MapFrom(src => $"images/{src.FileName}{Path.GetExtension(src.File.FileName)}"));
        }
    }
}
