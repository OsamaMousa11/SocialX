
using SocialX.Core.Domain.Entites;
using SocialX.Core.DTO.ProfileDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.Core.MappingProfile
{
    public class ProfileConfig : AutoMapper.Profile
    {
        public ProfileConfig()
        {

            CreateMap<Profile, ProfileResponse>()
            .ForMember(dest => dest.UserName,
                opt => opt.MapFrom(src =>
                    src.User != null ? src.User.UserName : string.Empty))
            .ForMember(dest => dest.Bio,
                opt => opt.MapFrom(src => src.Bio ?? string.Empty))
            .ForMember(dest => dest.ProfileImageUrl,
                opt => opt.MapFrom(src => src.ProfileImageUrl ?? string.Empty))
            .ForMember(dest => dest.ProfileBackgroundImageUrl,
                opt => opt.MapFrom(src => src.ProfileBackgroundImageUrl ?? string.Empty));


            CreateMap<ProfileAddRequest, Profile>()
          .ForMember(dest => dest.Id, opt => opt.Ignore())
          .ForMember(dest => dest.ProfileImageUrl, opt => opt.Ignore())
          .ForMember(dest => dest.ProfileBackgroundImageUrl, opt => opt.Ignore())
          .ForMember(dest => dest.User, opt => opt.Ignore())
          .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
          .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());


            CreateMap<ProfileUpdateRequest, Profile>()
                .ForMember(dest => dest.NickName,
                    opt => opt.Condition(src => src.NickName != null))
                .ForMember(dest => dest.Bio,
                    opt => opt.Condition(src => src.Bio != null))
                .ForMember(dest => dest.ProfileImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.ProfileBackgroundImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

        }
    }
}
