using SocialX.Core.Domain.Entites;
using SocialX.Core.DTO.FollowDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.Core.MappingProfile
{
    public class FollowMappingProfile : AutoMapper.Profile
    {
        public FollowMappingProfile()
        {
        
            CreateMap<Follow, FollowDto>()
                .ForMember(dest => dest.FollowerName, opt => opt.MapFrom(src => src.Follower.UserName))
                .ForMember(dest => dest.FollowerEmail, opt => opt.MapFrom(src => src.Follower.Email))
                .ForMember(dest => dest.FollowingName, opt => opt.MapFrom(src => src.Following.UserName))
                .ForMember(dest => dest.FollowingEmail, opt => opt.MapFrom(src => src.Following.Email));

            CreateMap<Follow, FollowerDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Follower.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Follower.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Follower.Email))
                .ForMember(dest => dest.FollowedAt, opt => opt.MapFrom(src => src.CreatedAt));

          
            CreateMap<Follow, FollowingDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Following.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Following.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Following.Email))
                .ForMember(dest => dest.FollowedAt, opt => opt.MapFrom(src => src.CreatedAt));
        }
    }
}
