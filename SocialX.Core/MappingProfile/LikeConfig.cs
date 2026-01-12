using AutoMapper;
using SocialX.Core.Domain.Entites;
using SocialX.Core.DTO.LikeDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.Core.MappingProfile
{
    public class LikeConfig:AutoMapper.Profile
    {
        public LikeConfig()
        {
            CreateMap<Like, LikeResponse>();


            CreateMap<Like, LikeResponse>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.NickName, opt => opt.MapFrom(src => src.User.Profile != null ? src.User.Profile.NickName : src.User.Name))
                .ForMember(dest => dest.ProfileImageUrl, opt => opt.MapFrom(src => src.User.Profile != null ? src.User.Profile.ProfileImageUrl : null));

        }
 
    }
}
