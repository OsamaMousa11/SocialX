using SocialX.Core.Domain.Entites;
using SocialX.Core.DTO.MentionDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
namespace SocialX.Core.MappingProfile
{
    public class MentionConfig:AutoMapper.Profile
    {
      public MentionConfig() {
      


                    CreateMap<Tweet, MentionResponse>()
             .ForMember(dest => dest.AuthorId,
                 opt => opt.MapFrom(src => src.UserId))
             .ForMember(dest => dest.AuthorUserName,
                 opt => opt.MapFrom(src => src.User.UserName))
             .ForMember(dest => dest.AuthorNickName,
                 opt => opt.MapFrom(src => src.User.Profile != null ? src.User.Profile.NickName : src.User.Name))
             .ForMember(dest => dest.AuthorProfileImageUrl,
                 opt => opt.MapFrom(src => src.User.Profile != null ? src.User.Profile.ProfileImageUrl : null))
             .ForMember(dest => dest.LikesCount,
                 opt => opt.Ignore())
             .ForMember(dest => dest.CommentsCount,
                 opt => opt.Ignore())
             .ForMember(dest => dest.RetweetsCount,
                 opt => opt.Ignore())
             .ForMember(dest => dest.IsLikedByCurrentUser,
                 opt => opt.Ignore())
             .ForMember(dest => dest.IsBookmarkedByCurrentUser, opt => opt.Ignore());

        }
    }
}
