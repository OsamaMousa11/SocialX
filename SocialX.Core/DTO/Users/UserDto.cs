using System;
using System.ComponentModel.DataAnnotations;

namespace SocialX.Core.DTOs.User
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string? ProfileImageUrl { get; set; }
        public bool IsFollowing { get; set; }
    }

    public class UserProfileDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }

        public string? NickName { get; set; }
        public string? Bio { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? ProfileBackgroundImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
        public int TweetsCount { get; set; }

        public bool IsFollowing { get; set; }
    }

    public class UpdateProfileDto
    {
        [StringLength(50)]
        public string? Name { get; set; }

        [StringLength(50)]
        public string? NickName { get; set; }

        [StringLength(250)]
        public string? Bio { get; set; }

        public string? ProfileImageUrl { get; set; }
        public string? ProfileBackgroundImageUrl { get; set; }
    }
}