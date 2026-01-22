
using SocialX.Core.Domain.Entites;
using SocialX.Core.DTO.TweetDto;
using System;
using System.Collections.Generic;
using System.Linq;


namespace SocialX.Core.MappingProfile
{
    public class TweetConfig : AutoMapper.Profile
    {
        public TweetConfig()
        {
       
            CreateMap<TweetAddRequest, Tweet>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Attachments, opt => opt.Ignore())
                .ForMember(dest => dest.Mentions, opt => opt.Ignore())
                .ForMember(dest => dest.TweetHashtags, opt => opt.Ignore())
                .ForMember(dest => dest.Likes, opt => opt.Ignore())
                .ForMember(dest => dest.Bookmarks, opt => opt.Ignore())
                .ForMember(dest => dest.Comments, opt => opt.Ignore())
                .ForMember(dest => dest.ReTweets, opt => opt.Ignore())
                .ForMember(dest => dest.OriginalTweet, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) 
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

         
            CreateMap<TweetUpdateRequest, Tweet>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
               
                .ForMember(dest => dest.Attachments, opt => opt.Ignore())
                .ForMember(dest => dest.Mentions, opt => opt.Ignore())
                .ForMember(dest => dest.TweetHashtags, opt => opt.Ignore())
                .ForMember(dest => dest.Likes, opt => opt.Ignore())
                .ForMember(dest => dest.Bookmarks, opt => opt.Ignore())
                .ForMember(dest => dest.Comments, opt => opt.Ignore())
                .ForMember(dest => dest.ReTweets, opt => opt.Ignore())
                .ForMember(dest => dest.OriginalTweet, opt => opt.Ignore())
                .ForMember(dest => dest.OriginalTweetId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore()) 
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

           
            CreateMap<Tweet, TweetResponse>()
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.NickName,
                    opt => opt.MapFrom(src => src.User.Profile != null
                        ? src.User.Profile.NickName
                        : src.User.UserName))
                .ForMember(dest => dest.ProfileImageUrl,
                    opt => opt.MapFrom(src => src.User.Profile != null
                        ? src.User.Profile.ProfileImageUrl
                        : null))
                .ForMember(dest => dest.ProfileBackgroundUrl,
                    opt => opt.MapFrom(src => src.User.Profile != null
                        ? src.User.Profile.ProfileBackgroundImageUrl
                        : null))
                .ForMember(dest => dest.Attachments,
                    opt => opt.MapFrom(src => src.Attachments ?? new List<Attachment>()))
                .ForMember(dest => dest.Hashtags,
                    opt => opt.MapFrom(src => src.TweetHashtags != null
                        ? src.TweetHashtags.Select(th => th.Hashtag.Name).ToList()
                        : new List<string>()))
                .ForMember(dest => dest.Mentions, opt => opt.Ignore()) 
                .ForMember(dest => dest.LikesCount, opt => opt.Ignore())   
                .ForMember(dest => dest.CommentsCount, opt => opt.Ignore()) 
                .ForMember(dest => dest.BookmarksCount, opt => opt.Ignore()) 
                .ForMember(dest => dest.RetweetsCount, opt => opt.Ignore()) 
                .ForMember(dest => dest.IsLikedByCurrentUser, opt => opt.Ignore())
                .ForMember(dest => dest.IsBookmarkedByCurrentUser, opt => opt.Ignore())
                .ForMember(dest => dest.IsRetweetedByCurrentUser, opt => opt.Ignore())
                .ForMember(dest => dest.OriginalTweet, opt => opt.Ignore()); 
        }
    }
}