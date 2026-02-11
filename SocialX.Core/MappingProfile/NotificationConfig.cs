using AutoMapper;
using SocialX.Core.Domain.Entites;
using SocialX.Core.DTO.NotificationDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Profile = AutoMapper.Profile;

namespace SocialX.Core.MappingProfile
{
    public class NotificationConfig:Profile
    {
        public NotificationConfig()
        {
            CreateMap<Notification, NotificationDto>()
            .ForMember(dest => dest.ReceiverUserName, opt => opt.MapFrom(src => src.ReceiverUser.UserName))
            .ForMember(dest => dest.SenderUserName, opt => opt.MapFrom(src => src.SenderUser.UserName));

          
            CreateMap<CreateNotificationDto, Notification>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.ReceiverUser, opt => opt.Ignore())
                .ForMember(dest => dest.SenderUser, opt => opt.Ignore());
        }
    }
}
