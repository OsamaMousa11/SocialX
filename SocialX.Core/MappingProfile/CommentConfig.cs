using AutoMapper;
using SocialX.Core.Domain.Entites;
using SocialX.Core.DTO.CommentDto;
using System;


namespace SocialX.Core.MappingProfile
{
    public class CommentConfig: AutoMapper.Profile
    {
        public CommentConfig()
        {

            CreateMap<Comment, CommentResponse>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.NickName, opt => opt.MapFrom(src => src.User.Profile != null ? src.User.Profile.NickName : src.User.UserName))
                .ForMember(dest => dest.ProfileImageUrl, opt => opt.MapFrom(src => src.User.Profile != null ? src.User.Profile.ProfileImageUrl : null))
                .ForMember(dest => dest.Attachments, opt => opt.MapFrom(src => src.Attachments))
                .ForMember(dest => dest.LikesCount, opt => opt.Ignore())
                .ForMember(dest => dest.RepliesCount, opt => opt.Ignore())
                .ForMember(dest => dest.IsLikedByCurrentUser, opt => opt.Ignore());
              

            CreateMap<CommentAddRequest, Comment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Tweet, opt => opt.Ignore())
                .ForMember(dest => dest.ParentComment, opt => opt.Ignore())
                .ForMember(dest => dest.Replies, opt => opt.Ignore())
                .ForMember(dest => dest.Attachments, opt => opt.Ignore())
                .ForMember(dest => dest.Likes, opt => opt.Ignore());
        }
    }
}
