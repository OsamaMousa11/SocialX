using AutoMapper;
using SocialX.Core.DTO.ProfileDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.Core.MappingProfile
{
    public class ProfileConfig : Profile
    {
        public ProfileConfig()
        {
          
            CreateMap<ProfileAddRequest, Profile>() ;


            CreateMap<ProfileUpdateRequest, Profile>();
 

            
            CreateMap<Profile, ProfileResponse>();
        }
    }
}
